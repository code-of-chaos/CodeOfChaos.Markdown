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
public sealed class FootnoteDescriptionJsonMdSyntaxNodeVisitor : JsonSyntaxNodeVisitor<FootnoteDescriptionMdSyntaxNode> {
    private static readonly string Identifier = nameof(FootnoteDescriptionMdSyntaxNode.Identifier).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(FootnoteDescriptionMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Identifier, node.Identifier);
    }

    protected override void SerializeDetails(JsonElement element, FootnoteDescriptionMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }
}
