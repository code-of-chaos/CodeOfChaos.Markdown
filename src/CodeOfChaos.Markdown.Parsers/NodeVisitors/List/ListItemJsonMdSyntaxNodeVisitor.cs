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
public sealed class ListItemJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<ListItemMdSyntaxNode> {
    private static readonly string LeadingSpaces = nameof(ListItemMdSyntaxNode.LeadingSpaces).ToCamelCase();
    private static readonly string CheckLeadingSpaces = nameof(ListItemMdSyntaxNode.CheckLeadingSpaces).ToCamelCase();
    private static readonly string Index = nameof(ListItemMdSyntaxNode.Index).ToCamelCase();
    private static readonly string CheckMarker = nameof(CheckMarker).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ListItemMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(CheckMarker, node.OriginalCheckMarker);
        writer.WriteString(Index, node.Index);
        writer.WriteNumber(LeadingSpaces, node.LeadingSpaces);
        writer.WriteNumber(CheckLeadingSpaces, node.CheckLeadingSpaces);
    }

    protected override void SerializeDetails(JsonElement element, ListItemMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, CheckMarker, out string? checkMarker)) {
            targetNode.WithCheckMarker(checkMarker);
        }

        if (TryGetPropertyAsString(element, Index, out string? index)) {
            targetNode.WithIndex(index);
        }

        if (TryGetPropertyAsInt32(element, LeadingSpaces, out int  leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
        if (TryGetPropertyAsInt32(element, CheckLeadingSpaces, out int checkLeadingSpaces)) {
            targetNode.WithCheckLeadingSpaces(checkLeadingSpaces);
        }
    }

}
