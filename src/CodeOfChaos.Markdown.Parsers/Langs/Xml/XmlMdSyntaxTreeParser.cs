// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using System.Collections.Frozen;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Langs.Xml;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
[InjectableSingleton<IXmlMdSyntaxTreeParser>]
public class XmlMdSyntaxTreeParser(IMarkdownConfig config) : IXmlMdSyntaxTreeParser {
    private readonly FrozenDictionary<Type, IXmlSyntaxNodeVisitor> _visitorsByType = config.XmlSyntaxNodeVisitors;
    private readonly FrozenDictionary<string, IXmlSyntaxNodeVisitor> _visitorsByName = config.XmlSyntaxNodeVisitors.ToFrozenDictionary(
        pair => pair.Key.Name,
        pair => pair.Value,
        StringComparer.Ordinal
    );

    private static readonly XmlWriterSettings WriterSettings = new() {
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), // No BOM
        Indent = true,
        OmitXmlDeclaration = false,
        Async = true
    };
    private static readonly XmlWriterSettings SyncWriterSettings = new() {
        Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), // No BOM
        Indent = true,
        OmitXmlDeclaration = false,
        Async = false
    };

    private static readonly XmlReaderSettings ReaderSettings = new() {
        Async = true,
        IgnoreComments = true,
        IgnoreWhitespace = false,
        DtdProcessing = DtdProcessing.Prohibit,
        CloseInput = false
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Deserialize
    /// <inheritdoc />
    public string DeserializeToString(IMdSyntaxTree tree) {
        ArgumentNullException.ThrowIfNull(tree);

        var stringBuilder = new StringBuilder(capacity: 2048);
        using var writer = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
        using var xmlWriter = XmlWriter.Create(writer, SyncWriterSettings);

        xmlWriter.WriteStartDocument();
        xmlWriter.WriteStartElement("MdSyntaxTree");

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            WriteNode(xmlWriter, child);
        }

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndDocument();
        xmlWriter.Flush();

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public async Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(tree);

        await using var stream = new MemoryStream();
        await DeserializeToXmlStreamAsync(stream, tree, ct);
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync(ct);
    }

    /// <inheritdoc />
    public XElement DeserializeToXmlElement(IMdSyntaxTree tree) {
        string xml = DeserializeToString(tree);
        return XElement.Parse(xml);
    }

    /// <inheritdoc />
    public async Task DeserializeToXmlStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(tree);

        await using var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), leaveOpen: true);
        await using var xmlWriter = XmlWriter.Create(writer, WriterSettings);

        await xmlWriter.WriteStartDocumentAsync();
        await xmlWriter.WriteStartElementAsync(prefix: null, localName: "MdSyntaxTree", ns: null);

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            await WriteNodeAsync(xmlWriter, child, ct);
        }

        await xmlWriter.WriteEndElementAsync();
        await xmlWriter.WriteEndDocumentAsync();
        await xmlWriter.FlushAsync();
        await writer.FlushAsync(ct);
    }

    /// <inheritdoc />
    public async Task DeserializeToXmlFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        ArgumentNullException.ThrowIfNull(tree);

        await using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await DeserializeToXmlStreamAsync(fileStream, tree, ct);
    }

    private ValueTask WriteNodeAsync(XmlWriter writer, IMdSyntaxNode node, CancellationToken ct)
        => _visitorsByType.TryGetValue(node.GetType(), out IXmlSyntaxNodeVisitor? visitor)
            ? visitor.WriteToXmlAsync(writer, node, (parentNode, token) => WriteChildrenAsync(writer, parentNode, token), ct)
            : ValueTask.CompletedTask;
    private void WriteNode(XmlWriter writer, IMdSyntaxNode node) {
        if (_visitorsByType.TryGetValue(node.GetType(), out IXmlSyntaxNodeVisitor? visitor)) {
            visitor.WriteToXml(writer, node, parentNode => WriteChildren(writer, parentNode));
        }
    }

    private async ValueTask WriteChildrenAsync(XmlWriter writer, IMdSyntaxNode parentNode, CancellationToken ct) {
        foreach (IMdSyntaxNode child in parentNode.GetChildren()) {
            await WriteNodeAsync(writer, child, ct);
        }
    }
    private void WriteChildren(XmlWriter writer, IMdSyntaxNode parentNode) {
        foreach (IMdSyntaxNode child in parentNode.GetChildren()) {
            WriteNode(writer, child);
        }
    }
    #endregion

    #region Serialize
    /// <inheritdoc />
    public IMdSyntaxTree SerializeStringToSyntaxTree(string input) {
        ArgumentNullException.ThrowIfNull(input);
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(input));
        return SerializeToSyntaxTreeAsync(stream).GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public IMdSyntaxTree SerializeToSyntaxTree(XElement element) {
        ArgumentNullException.ThrowIfNull(element);
        return SerializeStringToSyntaxTree(element.ToString(SaveOptions.DisableFormatting));
    }

    /// <inheritdoc />
    public async Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);

        using XmlReader reader = XmlReader.Create(stream, ReaderSettings);
        return await ReadSyntaxTreeAsync(reader, ct);
    }

    /// <inheritdoc />
    public async Task<IMdSyntaxTree> SerializeFileToSyntaxTreeAsync(string filePath, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return await SerializeToSyntaxTreeAsync(fileStream, ct);
    }

    private async Task<IMdSyntaxTree> ReadSyntaxTreeAsync(XmlReader reader, CancellationToken ct) {
        MdSyntaxTree tree = new();

        XmlNodeType rootType = await reader.MoveToContentAsync();
        if (rootType != XmlNodeType.Element || !reader.LocalName.Equals("MdSyntaxTree", StringComparison.Ordinal)) {
            throw new InvalidOperationException("Invalid XML root element");
        }

        if (reader.IsEmptyElement) {
            await reader.ReadAsync();
            return tree;
        }

        await reader.ReadAsync();

        while (!ct.IsCancellationRequested) {
            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals("MdSyntaxTree", StringComparison.Ordinal)) {
                await reader.ReadAsync();
                break;
            }

            if (reader.NodeType == XmlNodeType.Element) {
                await ReadNodeAsync(tree, reader, tree.RootNode, ct);
                continue;
            }

            if (reader.EOF) break;
            await reader.ReadAsync();
        }

        ct.ThrowIfCancellationRequested();
        return tree;
    }

    private async ValueTask ReadNodeAsync(IMdSyntaxTree tree, XmlReader reader, IMdSyntaxNode parentNode, CancellationToken ct) {
        if (!_visitorsByName.TryGetValue(reader.LocalName, out IXmlSyntaxNodeVisitor? visitor)) {
            await reader.SkipAsync();
            return;
        }

        string elementName = reader.LocalName;
        IMdSyntaxNode node = visitor.ReadStartElement(tree, reader, parentNode);

        if (reader.IsEmptyElement) {
            visitor.ReadTextContent(node, string.Empty);
            await reader.ReadAsync();
            return;
        }

        await reader.ReadAsync();

        StringBuilder? contentBuilder = null;

        while (!ct.IsCancellationRequested) {
            if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals(elementName, StringComparison.Ordinal)) {
                break;
            }

            if (reader.NodeType == XmlNodeType.Element) {
                if (visitor.TryReadSpecialChildElement(node, reader)) {
                    continue;
                }

                await ReadNodeAsync(tree, reader, node, ct);
                continue;
            }

            if (reader.NodeType is XmlNodeType.Text or XmlNodeType.CDATA or XmlNodeType.Whitespace or XmlNodeType.SignificantWhitespace) {
                contentBuilder ??= new StringBuilder();
                contentBuilder.Append(reader.Value);
                await reader.ReadAsync();
                continue;
            }

            if (reader.EOF) break;
            await reader.ReadAsync();
        }

        visitor.ReadTextContent(node, contentBuilder?.ToString() ?? string.Empty);

        if (reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals(elementName, StringComparison.Ordinal)) {
            await reader.ReadAsync();
        }

        ct.ThrowIfCancellationRequested();
    }
    #endregion
}
