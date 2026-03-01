// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Langs.Xml;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IXmlMdSyntaxTreeParser>]
public class XmlMdSyntaxTreeParser : IXmlMdSyntaxTreeParser {
    private readonly Dictionary<Type, IXmlSyntaxNodeVisitor> _visitors = new();
    private readonly Dictionary<string, Type> _nodeTypes = new();

    public static IXmlMdSyntaxTreeParser Instance { get; } = new XmlMdSyntaxTreeParser();

    private static readonly XmlWriterSettings WriterSettings = new() {
        Encoding = Encoding.UTF8,
        Indent = true,
        OmitXmlDeclaration = false,
        Async = true,
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public XmlMdSyntaxTreeParser() {
        RegisterVisitor<BlockQuoteMdSyntaxNode, BlockQuoteXmlSyntaxNodeVisitor>();
        RegisterVisitor<BoldMdSyntaxNode, XmlSyntaxNodeVisitor<BoldMdSyntaxNode>>();
        RegisterVisitor<BreakMdSyntaxNode, XmlSyntaxNodeVisitor<BreakMdSyntaxNode>>();
        RegisterVisitor<CalloutBodyMdSyntaxNode, XmlSyntaxNodeVisitor<CalloutBodyMdSyntaxNode>>();
        RegisterVisitor<CalloutMdSyntaxNode, CalloutXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<CalloutTitleMdSyntaxNode, XmlSyntaxNodeVisitor<CalloutTitleMdSyntaxNode>>();
        RegisterVisitor<CodeBlockMdSyntaxNode, CodeBlockXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<CodeInlineMdSyntaxNode, CodeInlineXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<EmoteMdSyntaxNode, EmoteXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<EscapedCharacterMdSyntaxNode, EscapedCharacterXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<FootnoteDescriptionMdSyntaxNode, FootnoteDescriptionXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<FootnoteReferenceMdSyntaxNode, FootnoteReferenceXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<FrontMatterMdSyntaxNode, FrontMatterXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<HeadingMdSyntaxNode, HeadingXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<HeadingSimpleMdSyntaxNode, HeadingSimpleXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<HighlightMdSyntaxNode, XmlSyntaxNodeVisitor<HighlightMdSyntaxNode>>();
        RegisterVisitor<HorizontalRuleMdSyntaxNode, HorizontalRuleXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<HtmlMdSyntaxNode, HtmlXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<HtmlSpanMdSyntaxNode, HtmlSpanXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ImageMdSyntaxNode, ImageXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ItalicMdSyntaxNode, XmlSyntaxNodeVisitor<ItalicMdSyntaxNode>>();
        RegisterVisitor<LinkMdSyntaxNode, LinkXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ListItemMdSyntaxNode, ListItemXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ListOrderedMdSyntaxNode, ListOrderedXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ListUnOrderedMdSyntaxNode, ListUnOrderedXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<NewLineMdSyntaxNode, XmlSyntaxNodeVisitor<NewLineMdSyntaxNode>>();
        RegisterVisitor<ParagraphMdSyntaxNode, XmlSyntaxNodeVisitor<ParagraphMdSyntaxNode>>();
        RegisterVisitor<ScriptingBodySyntaxNode, ScriptingBodyXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ScriptingExpressionSyntaxNode, ScriptingExpressionXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<ScriptingIfStatementSyntaxNode, ScriptingIfStatementXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<StrikeMdSyntaxNode, XmlSyntaxNodeVisitor<StrikeMdSyntaxNode>>();
        RegisterVisitor<SubScriptMdSyntaxNode, XmlSyntaxNodeVisitor<SubScriptMdSyntaxNode>>();
        RegisterVisitor<SuperScriptMdSyntaxNode, XmlSyntaxNodeVisitor<SuperScriptMdSyntaxNode>>();
        RegisterVisitor<TableCellMdSyntaxNode, TableCellXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<TableMdSyntaxNode, TableXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<TableRowMdSyntaxNode, TableRowXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<TagMdSyntaxNode, TagXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<TemplateMdSyntaxNode, TemplateXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<TextMdSyntaxNode, TextXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<UnderlineMdSyntaxNode, XmlSyntaxNodeVisitor<UnderlineMdSyntaxNode>>();
        RegisterVisitor<UserMdSyntaxNode, UserXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<WikiLinkMdSyntaxNode, WikiLinkXmlMdSyntaxNodeVisitor>();
        RegisterVisitor<WrapperMdSyntaxNode, XmlSyntaxNodeVisitor<WrapperMdSyntaxNode>>();
    }

    private void RegisterVisitor<TNode, TVisitor>() where TNode : MdSyntaxNode<TNode>, new() where TVisitor : XmlSyntaxNodeVisitor<TNode>, new() {
        _visitors[typeof(TNode)] = new TVisitor();
        _nodeTypes[typeof(TNode).Name] = typeof(TNode);
    }

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
        
        await using var stream = new MemoryStream();
        await DeserializeToXmlStreamAsync(stream, tree, ct);
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync(ct);
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
