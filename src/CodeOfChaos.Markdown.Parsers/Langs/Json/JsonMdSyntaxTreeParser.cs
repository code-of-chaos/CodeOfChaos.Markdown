// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Parsers.Json.NodeVisitors;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IJsonMdSyntaxTreeParser>]
public class JsonMdSyntaxTreeParser : IJsonMdSyntaxTreeParser {
    private readonly Dictionary<Type, IJsonSyntaxNodeVisitor> _visitors = new();
    private readonly Dictionary<string, Type> _nodeTypes = new();

    private static readonly JsonWriterOptions WriterOptions = new() {
        Indented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    private static readonly JsonSerializerOptions SerializerOptions = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public JsonMdSyntaxTreeParser() {
        RegisterVisitor<BlockQuoteMdSyntaxNode, BlockQuoteJsonSyntaxNodeVisitor>();
        RegisterVisitor<BoldMdSyntaxNode, BoldJsonSyntaxNodeVisitor>();
        RegisterVisitor<CalloutBodyMdSyntaxNode, CalloutBodyJsonSyntaxNodeVisitor>();
        RegisterVisitor<CalloutTitleMdSyntaxNode, CalloutTitleJsonSyntaxNodeVisitor>();
        RegisterVisitor<CalloutMdSyntaxNode, CalloutJsonSyntaxNodeVisitor>();
        RegisterVisitor<CodeBlockMdSyntaxNode, CodeBlockJsonSyntaxNodeVisitor>();
        RegisterVisitor<CodeInlineMdSyntaxNode, CodeInlineJsonSyntaxNodeVisitor>();
        RegisterVisitor<HtmlMdSyntaxNode, HtmlJsonSyntaxNodeVisitor>();
        RegisterVisitor<TextMdSyntaxNode, TextJsonSyntaxNodeVisitor>();
        RegisterVisitor<EmoteMdSyntaxNode, EmoteJsonSyntaxNodeVisitor>();
        RegisterVisitor<EscapedCharacterMdSyntaxNode, EscapedCharacterJsonSyntaxNodeVisitor>();
        RegisterVisitor<HeadingMdSyntaxNode, HeadingJsonSyntaxNodeVisitor>();
        RegisterVisitor<HeadingSimpleMdSyntaxNode, HeadingSimpleJsonSyntaxNodeVisitor>();
        RegisterVisitor<HorizontalRuleMdSyntaxNode, HorizontalRuleJsonSyntaxNodeVisitor>();
        RegisterVisitor<HtmlSpanMdSyntaxNode, HtmlSpanJsonSyntaxNodeVisitor>();
        RegisterVisitor<ImageMdSyntaxNode, ImageJsonSyntaxNodeVisitor>();
        RegisterVisitor<ItalicMdSyntaxNode, ItalicJsonSyntaxNodeVisitor>();
        RegisterVisitor<LinkMdSyntaxNode, LinkJsonSyntaxNodeVisitor>();
        RegisterVisitor<ListItemMdSyntaxNode, ListItemJsonSyntaxNodeVisitor>();
        RegisterVisitor<ListOrderedMdSyntaxNode, ListOrderedJsonSyntaxNodeVisitor>();
        RegisterVisitor<ListUnOrderedMdSyntaxNode, ListUnOrderedJsonSyntaxNodeVisitor>();
        RegisterVisitor<ParagraphMdSyntaxNode, ParagraphJsonSyntaxNodeVisitor>();
        RegisterVisitor<StrikeMdSyntaxNode, StrikeJsonSyntaxNodeVisitor>();
        RegisterVisitor<SubScriptMdSyntaxNode, SubScriptJsonSyntaxNodeVisitor>();
        RegisterVisitor<SuperScriptMdSyntaxNode, SuperScriptJsonSyntaxNodeVisitor>();
        RegisterVisitor<TableCellMdSyntaxNode, TableCellJsonSyntaxNodeVisitor>();
        RegisterVisitor<TableRowMdSyntaxNode, TableRowJsonSyntaxNodeVisitor>();
        RegisterVisitor<TableMdSyntaxNode, TableJsonSyntaxNodeVisitor>();
        RegisterVisitor<TagMdSyntaxNode, TagJsonSyntaxNodeVisitor>();
        RegisterVisitor<UnderlineMdSyntaxNode, UnderlineJsonSyntaxNodeVisitor>();
        RegisterVisitor<NewLineMdSyntaxNode, NewLineJsonSyntaxNodeVisitor>();
        RegisterVisitor<UserMdSyntaxNode, UserJsonSyntaxNodeVisitor>();
        RegisterVisitor<WikiLinkMdSyntaxNode, WikiLinkJsonSyntaxNodeVisitor>();
        RegisterVisitor<TemplateMdSyntaxNode, TemplateJsonSyntaxNodeVisitor>();
        RegisterVisitor<HighlightMdSyntaxNode, HighlightJsonSyntaxNodeVisitor>();
        RegisterVisitor<WrapperMdSyntaxNode, WrapperJsonSyntaxNodeVisitor>();
        RegisterVisitor<FrontMatterMdSyntaxNode, FrontMatterJsonSyntaxNodeVisitor>();
        RegisterVisitor<BreakMdSyntaxNode, BreakJsonSyntaxNodeVisitor>();
        RegisterVisitor<ScriptingBodySyntaxNode, ScriptingBodyJsonSyntaxNodeVisitor>();
        RegisterVisitor<ScriptingExpressionSyntaxNode, ScriptingExpressionJsonSyntaxNodeVisitor>();
        RegisterVisitor<ScriptingIfStatementSyntaxNode, ScriptingIfStatementJsonSyntaxNodeVisitor>();
    }

    private void RegisterVisitor<TNode, TVisitor>() where TNode : MdSyntaxNode<TNode>, new() where TVisitor : BaseJsonSyntaxNodeVisitor<TNode>, new() {
        _visitors[typeof(TNode)] = new TVisitor();
        _nodeTypes[typeof(TNode).Name] = typeof(TNode);
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Deserialize
    public string DeserializeToString(IMdSyntaxTree input) {
        JsonElement element = DeserializeToJsonElement(input);
        return JsonSerializer.Serialize(element, SerializerOptions);
    }
    
    public async Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(tree);
        
        await using var stream = new MemoryStream();
        await DeserializeToJsonStreamAsync(stream, tree, ct);
        stream.Position = 0;
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync(ct);
    } 

    public JsonElement DeserializeToJsonElement(IMdSyntaxTree tree) {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, WriterOptions);

        writer.WriteStartObject();
        writer.WriteString("type", "MdSyntaxTree");
        writer.WriteStartArray("children");

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            DeserializeNode(child, writer);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();

        stream.Position = 0;
        using JsonDocument document = JsonDocument.Parse(stream);
        return document.RootElement.Clone();
    }

    public async Task DeserializeToJsonStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(tree);

        JsonElement rootElement = DeserializeToJsonElement(tree);
        await JsonSerializer.SerializeAsync(stream, rootElement, SerializerOptions, ct);
    }

    public async Task DeserializeToJsonFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        ArgumentNullException.ThrowIfNull(tree);

        await using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await DeserializeToJsonStreamAsync(fileStream, tree, ct);
    }

    private void DeserializeNode(IMdSyntaxNode node, Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString("type", node.GetType().Name);

        if (_visitors.TryGetValue(node.GetType(), out IJsonSyntaxNodeVisitor? visitor)) {
            visitor.DeserializeToJson(node, writer);
        }

        List<IMdSyntaxNode> children = node.GetChildren().ToList();
        if (children.Count > 0) {
            writer.WriteStartArray("children");
            foreach (IMdSyntaxNode child in children) {
                DeserializeNode(child, writer);
            }
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
    #endregion

    #region Serialize
    
    public IMdSyntaxTree SerializeToSyntaxTree(string input) {
        JsonElement element = JsonDocument.Parse(input).RootElement;
        return SerializeToSyntaxTree(element);
    }
    
    public IMdSyntaxTree SerializeToSyntaxTree(JsonElement element) {
        if (!element.TryGetProperty("type", out JsonElement typeProperty) || typeProperty.GetString() != "MdSyntaxTree") {
            throw new InvalidOperationException("Invalid JSON root element");
        }

        MdSyntaxTree tree = new();

        if (element.TryGetProperty("children", out JsonElement childrenProperty) && childrenProperty.ValueKind == JsonValueKind.Array) {
            foreach (JsonElement child in childrenProperty.EnumerateArray()) {
                SerializeNode(child, tree.RootNode);
            }
        }

        return tree;
    }

    public async Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);

        using JsonDocument document = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        return SerializeToSyntaxTree(document.RootElement);
    }

    public async Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(string filePath, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        await using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return await SerializeToSyntaxTreeAsync(fileStream, ct);
    }

    private void SerializeNode(JsonElement element, IMdSyntaxNode parentNode) {
        if (element.TryGetProperty("type", out JsonElement typeProperty)) {
            string? typeName = typeProperty.GetString();
            if (!string.IsNullOrWhiteSpace(typeName)
                && _nodeTypes.TryGetValue(typeName, out Type? nodeType)
                && _visitors.TryGetValue(nodeType, out IJsonSyntaxNodeVisitor? visitor)) {

                IMdSyntaxNode newNode = visitor.SerializeToNode(element, parentNode);

                if (element.TryGetProperty("children", out JsonElement childrenProperty) && childrenProperty.ValueKind == JsonValueKind.Array) {
                    foreach (JsonElement child in childrenProperty.EnumerateArray()) {
                        SerializeNode(child, newNode);
                    }
                }
            }
        }
    }
    #endregion
}
