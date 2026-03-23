// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Langs.Json;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public class JsonSyntaxNodeVisitor<TSyntaxNode> : IJsonSyntaxNodeVisitor<TSyntaxNode> where TSyntaxNode : MdSyntaxNode<TSyntaxNode>, new() {
    private const string Modifiers = nameof(Modifiers);
    private const string OriginalInput = nameof(OriginalInput);
    private const string Attributes = nameof(Attributes);
    private const string Start = nameof(Start);
    private const string End = nameof(End);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public void DeserializeToJson(IMdSyntaxNode node, Utf8JsonWriter writer) {
        DeserializeDetails(Unsafe.As<TSyntaxNode>(node), writer);
    }

    private static void SerializeModifiers(IMdSyntaxNodeModifier modifiers, Utf8JsonWriter writer) {
        writer.WriteStartObject(Modifiers);

        writer.WriteStartArray(Attributes);
        foreach (KeyValuePair<string, Range> attr in modifiers.Attributes) {
            writer.WriteStartObject();
            writer.WriteString("key", attr.Key);
            writer.WriteNumber(Start, attr.Value.Start.Value);
            writer.WriteNumber(End, attr.Value.End.Value);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();

        writer.WriteString(OriginalInput, modifiers.OriginalInput);
        writer.WriteEndObject();
    }

    protected virtual void DeserializeDetails(TSyntaxNode node, Utf8JsonWriter writer) {
        if (node.Modifier is {} modifier) {
            SerializeModifiers(modifier, writer);
        }
    }

    /// <inheritdoc />
    public IMdSyntaxNode SerializeToNode(JsonElement element, IMdSyntaxNode parentNode) {
        TSyntaxNode node = MdSyntaxNodePool<TSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);

        SerializeDetails(element, node);
        return node;
    }

    private static MdSyntaxNodeModifier DeserializeModifiers(JsonElement element) {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!TryGetPropertyAsString(element, OriginalInput, out string? originalInput)) return MdSyntaxNodeModifier.FromString(string.Empty);
        return MdSyntaxNodeModifier.FromString(originalInput);
    }

    protected virtual void SerializeDetails(JsonElement element, TSyntaxNode targetNode) {
        if (element.TryGetProperty(Modifiers, out JsonElement modifiersElement)) {
            targetNode.WithModifier(DeserializeModifiers(modifiersElement));
        }
    }
    
    protected static bool TryGetPropertyAsInt32(JsonElement element, string propertyName, out int value) {
        value = 0;
        return element.TryGetProperty(propertyName, out JsonElement property) 
            && property.TryGetInt32(out value);
    }
    
    protected static bool TryGetPropertyAsString(JsonElement element, string propertyName, [NotNullWhen(true)] out string? value) {
        value = string.Empty;
        if (!element.TryGetProperty(propertyName, out JsonElement property)) return false;
        if (property.GetString() is not {} stringValue) return false;
        
        value = stringValue;
        return true;
    }
    
    protected static bool TryGetPropertyAsEnum<TEnumType>(JsonElement element, string propertyName, out TEnumType value) where TEnumType : struct {
        value = default;
        if (!element.TryGetProperty(propertyName, out JsonElement property)) return false;
        return property.GetString() is {} stringValue && Enum.TryParse(stringValue, out value);
    }
}
