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
public sealed class HeadingSimpleJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<HeadingSimpleMdSyntaxNode> {
    private static readonly string Identifier = nameof(HeadingSimpleMdSyntaxNode.Identifier).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(HeadingSimpleMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Identifier, node.Identifier);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, HeadingSimpleMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }

}
