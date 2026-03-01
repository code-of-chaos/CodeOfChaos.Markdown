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
public sealed class FootnoteReferenceJsonMdSyntaxNodeVisitor : JsonSyntaxNodeVisitor<FootnoteReferenceMdSyntaxNode> {
    private static readonly string Identifier = nameof(FootnoteReferenceMdSyntaxNode.Identifier).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(FootnoteReferenceMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Identifier, node.Identifier);
    }

    protected override void SerializeDetails(JsonElement element, FootnoteReferenceMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        // ReSharper disable once InvertIf
        if (element.TryGetProperty(Identifier, out JsonElement contentProperty)) {
            string? contentValue = contentProperty.GetString();
            if (!string.IsNullOrEmpty(contentValue)) {
                targetNode.WithIdentifier(contentValue);
            }
        }
    }
}
