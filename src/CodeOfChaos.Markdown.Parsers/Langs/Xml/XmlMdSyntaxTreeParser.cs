// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using System.Buffers;
using System.Collections.Frozen;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Langs.Xml;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IXmlMdSyntaxTreeParser>]
public class XmlMdSyntaxTreeParser(IMarkdownConfig config) : IXmlMdSyntaxTreeParser {
    private readonly FrozenDictionary<Type, IXmlSyntaxNodeVisitor> _visitors = config.XmlSyntaxNodeVisitors;
    private readonly FrozenDictionary<string, Type> _nodeTypes = config.XmlSyntaxNodeVisitors.ToFrozenDictionary(
        pair => pair.Key.Name,
        pair => pair.Key
    );

    private static readonly XmlWriterSettings WriterSettings = new() {
        Encoding = Encoding.UTF8,
        Indent = true,
        OmitXmlDeclaration = false,
        Async = true
    };
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Deserialize
    public string DeserializeToString(IMdSyntaxTree tree) {
        XElement rootElement = DeserializeToXmlElement(tree);
        return rootElement.ToString();
    }
    
    public async Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(tree);

        XmlWriterSettings writerSettings = WriterSettings.Clone();
        writerSettings.Encoding = Encoding.UTF8;

        await using var stream = new MemoryStream();
        await using var writer = XmlWriter.Create(stream, writerSettings);

        await writer.WriteStartDocumentAsync();
        await writer.WriteStartElementAsync(null, "MdSyntaxTree", null);

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            await DeserializeNodeAsync(child, writer, ct);
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndDocumentAsync();
        await writer.FlushAsync();

        stream.Position = 0;
        byte[] buffer = ArrayPool<byte>.Shared.Rent((int)stream.Length);
        try {
            int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, (int)stream.Length), ct);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        } finally {
            ArrayPool<byte>.Shared.Return(buffer);
        }

    } 
    
    public XElement DeserializeToXmlElement(IMdSyntaxTree tree) {
        var rootElement = new XElement("MdSyntaxTree");

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            DeserializeNode(child, rootElement);
        }

        return rootElement;
    }

    public async Task DeserializeToXmlStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(tree);

        XElement rootElement = DeserializeToXmlElement(tree);

        await using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        await using var xmlWriter = XmlWriter.Create(writer, WriterSettings);
        await rootElement.WriteToAsync(xmlWriter, ct);
        await xmlWriter.FlushAsync();
        await writer.FlushAsync(ct);
    }

    public async Task DeserializeToXmlFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        ArgumentNullException.ThrowIfNull(tree);

        await using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await DeserializeToXmlStreamAsync(fileStream, tree, ct);
    }

    private void DeserializeNode(IMdSyntaxNode node, XElement parentElement) {
        if (_visitors.TryGetValue(node.GetType(), out IXmlSyntaxNodeVisitor? visitor)) {
            parentElement = visitor.DeserializeToXml(node, parentElement);
        }

        foreach (IMdSyntaxNode child in node.GetChildren()) {
            DeserializeNode(child, parentElement);
        }
    }

    private async Task DeserializeNodeAsync(IMdSyntaxNode node, XmlWriter writer, CancellationToken ct) {
        Type nodeType = node.GetType();

        if (_visitors.TryGetValue(nodeType, out IXmlSyntaxNodeVisitor? visitor)) {
            // Create a temporary XElement to leverage existing visitor logic
            var tempElement = new XElement("temp");
            XElement resultElement = visitor.DeserializeToXml(node, tempElement);

            // Write the result to XmlWriter
            if (resultElement != tempElement && resultElement.Parent == tempElement) {
                resultElement = resultElement.Parent.Elements().First();
            }

            foreach (XElement element in resultElement.Elements()) {
                await element.WriteToAsync(writer, ct);
            }
        }

        foreach (IMdSyntaxNode child in node.GetChildren()) {
            await DeserializeNodeAsync(child, writer, ct);
        }
    }
    #endregion

    #region Serialize
    public IMdSyntaxTree SerializeStringToSyntaxTree(string input) {
        XElement element = XElement.Parse(input);
        return SerializeToSyntaxTree(element);
    }
    public IMdSyntaxTree SerializeToSyntaxTree(XElement element) {
        if (element.Name != "MdSyntaxTree") throw new InvalidOperationException("Invalid XML root element");

        MdSyntaxTree tree = new();

        foreach (XElement child in element.Elements()) {
            SerializeNode(tree, child, tree.RootNode);
        }

        return tree;
    }

    public async Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);

        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        string xmlContent = await reader.ReadToEndAsync(ct);
        XElement rootElement = XElement.Parse(xmlContent);

        return SerializeToSyntaxTree(rootElement);
    }

    public async Task<IMdSyntaxTree> SerializeFileToSyntaxTreeAsync(string filePath, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return await SerializeToSyntaxTreeAsync(fileStream, ct);
    }

    private void SerializeNode(IMdSyntaxTree tree, XElement element, IMdSyntaxNode parentNode) {
        if (element.Name.LocalName.IsNotNullOrWhiteSpace()
            && _nodeTypes.TryGetValue(element.Name.LocalName, out Type? nodeType)
            && _visitors.TryGetValue(nodeType, out IXmlSyntaxNodeVisitor? visitor)) {
            parentNode = visitor.SerializeToNode(tree, element, parentNode);
        }

        foreach (XElement child in element.Elements()) {
            SerializeNode(tree, child, parentNode);
        }
    }
    #endregion
}
