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
public sealed class ListOrderedJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<ListOrderedMdSyntaxNode> {
    private static readonly string LeadingSpaces = nameof(ListOrderedMdSyntaxNode.LeadingSpaces).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ListOrderedMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(JsonElement element, ListOrderedMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (element.TryGetProperty(LeadingSpaces, out JsonElement leadingSpacesProperty)) {
            targetNode.WithLeadingSpaces(leadingSpacesProperty.GetInt32());
        }
    }
}
