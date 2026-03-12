// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Syntax;
using System.Buffers;
using System.Collections.Frozen;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Langs.Json;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
[InjectableSingleton<IJsonMdSyntaxTreeParser>]
public class JsonMdSyntaxTreeParser(IMarkdownConfig config) : IJsonMdSyntaxTreeParser {
    private readonly FrozenDictionary<Type, IJsonSyntaxNodeVisitor> _visitors = config.JsonSyntaxNodeVisitors;
    private readonly FrozenDictionary<string, Type> _nodeTypes = config.JsonSyntaxNodeVisitors.ToFrozenDictionary(
        pair => pair.Key.Name,
        pair => pair.Key
    );
    private readonly FrozenDictionary<Type, string> _nodeTypeNames = config.JsonSyntaxNodeVisitors.ToFrozenDictionary(
        pair => pair.Key,
        pair => pair.Key.Name
    );

    // Cached property names to reduce allocations
    private static readonly JsonEncodedText TypePropertyNameEncoded = JsonEncodedText.Encode("type");
    private static readonly JsonEncodedText ChildrenPropertyNameEncoded = JsonEncodedText.Encode("children");
    private static readonly JsonEncodedText MdSyntaxTreeEncoded = JsonEncodedText.Encode("MdSyntaxTree");

    private static readonly JsonWriterOptions WriterOptions = new() {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Deserialize
    /// <inheritdoc />
    public string DeserializeToString(IMdSyntaxTree input) {
        ArgumentNullException.ThrowIfNull(input);

        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(bufferWriter, WriterOptions);

        WriteTree(input, writer);
        writer.Flush();

        return Encoding.UTF8.GetString(bufferWriter.WrittenSpan);
    }
    
    /// <inheritdoc />
    public async Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(tree);

        var bufferWriter = new ArrayBufferWriter<byte>();
        await using var writer = new Utf8JsonWriter(bufferWriter, WriterOptions);
        WriteTree(tree, writer);
        await writer.FlushAsync(ct);

        return Encoding.UTF8.GetString(bufferWriter.WrittenSpan);
    }

    /// <inheritdoc />
    public JsonElement DeserializeToJsonElement(IMdSyntaxTree tree) {
        ArgumentNullException.ThrowIfNull(tree);

        var bufferWriter = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(bufferWriter, WriterOptions);
        WriteTree(tree, writer);
        writer.Flush();

        using JsonDocument document = JsonDocument.Parse(bufferWriter.WrittenMemory);
        return document.RootElement.Clone();
    }

    /// <inheritdoc />
    public async Task DeserializeToJsonStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(tree);

        await using var writer = new Utf8JsonWriter(stream, WriterOptions);
        WriteTree(tree, writer);
        await writer.FlushAsync(ct);
    }

    /// <inheritdoc />
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
            writer.WriteString(TypePropertyNameEncoded, typeName);
        } else {
            writer.WriteString(TypePropertyNameEncoded, nodeType.Name);
        }

        if (_visitors.TryGetValue(nodeType, out IJsonSyntaxNodeVisitor? visitor)) {
            visitor.DeserializeToJson(node, writer);
        }

        IEnumerable<IMdSyntaxNode> children = node.GetChildren();
        bool hasChildren = false;
        foreach (IMdSyntaxNode child in children) {
            if (!hasChildren) {
                writer.WriteStartArray(ChildrenPropertyNameEncoded);
                hasChildren = true;
            }
            DeserializeNode(child, writer);
        }
        if (hasChildren) {
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }

    private void WriteTree(IMdSyntaxTree tree, Utf8JsonWriter writer) {
        writer.WriteStartObject();
        writer.WriteString(TypePropertyNameEncoded, MdSyntaxTreeEncoded);
        writer.WriteStartArray(ChildrenPropertyNameEncoded);

        foreach (IMdSyntaxNode child in tree.RootNode.GetChildren()) {
            DeserializeNode(child, writer);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
    #endregion

    #region Serialize
    
    /// <inheritdoc />
    public IMdSyntaxTree SerializeToSyntaxTree(string input) {
        JsonElement element = JsonDocument.Parse(input).RootElement;
        return SerializeToSyntaxTree(element);
    }
    
    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default) {
        ArgumentNullException.ThrowIfNull(stream);

        using JsonDocument document = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        return SerializeToSyntaxTree(document.RootElement);
    }

    /// <inheritdoc />
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
