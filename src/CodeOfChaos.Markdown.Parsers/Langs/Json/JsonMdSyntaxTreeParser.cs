// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Syntax;
using System.Collections.Frozen;
using System.Text;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Langs.Json;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IJsonMdSyntaxTreeParser>]
public class JsonMdSyntaxTreeParser(IMarkdownConfig config) : IJsonMdSyntaxTreeParser {
    private readonly FrozenDictionary<Type, IJsonSyntaxNodeVisitor> _visitors = config.JsonNodeVisitors;
    private readonly FrozenDictionary<string, Type> _nodeTypes = config.JsonNodeVisitors.ToFrozenDictionary(
        pair => pair.Key.Name,
        pair => pair.Key
    );
    private readonly FrozenDictionary<Type, string> _nodeTypeNames = config.JsonNodeVisitors.ToFrozenDictionary(
        pair => pair.Key,
        pair => pair.Key.Name   
    );

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

        await using var writer = new Utf8JsonWriter(stream, WriterOptions);

        writer.WriteStartObject();
        writer.WriteString("type", "MdSyntaxTree");
        writer.WriteStartArray("children");

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            DeserializeNode(child, writer);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
        await writer.FlushAsync(ct);
    }

    public async Task DeserializeToJsonFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default) {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

        ArgumentNullException.ThrowIfNull(tree);

        await using FileStream fileStream = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await DeserializeToJsonStreamAsync(fileStream, tree, ct);
    }

    private void DeserializeNode(IMdSyntaxNode node, Utf8JsonWriter writer) {
        Type nodeType = node.GetType();
        writer.WriteStartObject();

        if (_nodeTypeNames.TryGetValue(nodeType, out string? typeName)) {
            writer.WriteString("type", typeName);
        } else {
            writer.WriteString("type", nodeType.Name);
        }

        if (_visitors.TryGetValue(nodeType, out IJsonSyntaxNodeVisitor? visitor)) {
            visitor.DeserializeToJson(node, writer);
        }

        IEnumerable<IMdSyntaxNode> children = node.GetChildren();
        bool hasChildren = false;
        foreach (IMdSyntaxNode child in children) {
            if (!hasChildren) {
                writer.WriteStartArray("children");
                hasChildren = true;
            }
            DeserializeNode(child, writer);
        }
        if (hasChildren) {
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
