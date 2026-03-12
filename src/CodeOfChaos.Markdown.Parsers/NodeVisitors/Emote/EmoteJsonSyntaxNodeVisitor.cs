// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class EmoteJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<EmoteMdSyntaxNode> {
    private static readonly string EmoteKey = nameof(EmoteMdSyntaxNode.EmoteKey).ToCamelCase();
    private static readonly string OriginalEmote = nameof(EmoteMdSyntaxNode.OriginalEmote).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(EmoteMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(EmoteKey, node.EmoteKey);
        writer.WriteString(OriginalEmote, node.OriginalEmote);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, EmoteMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, EmoteKey, out string? emoteKey)) {
            targetNode.WithEmoteKey(emoteKey);
        }
        
        if (TryGetPropertyAsString(element, OriginalEmote, out string? originalEmote)) {
            targetNode.WithOriginalEmote(originalEmote);
        }
    }

}
