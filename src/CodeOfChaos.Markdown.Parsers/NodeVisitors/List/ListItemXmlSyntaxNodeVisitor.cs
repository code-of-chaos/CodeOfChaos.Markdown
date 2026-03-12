// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class ListItemXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ListItemMdSyntaxNode> {
    private const string LeadingSpaces = nameof(ListItemMdSyntaxNode.LeadingSpaces);
    private const string CheckLeadingSpaces = nameof(ListItemMdSyntaxNode.CheckLeadingSpaces);
    private const string Index = nameof(ListItemMdSyntaxNode.Index);
    private const string CheckMarker = nameof(CheckMarker);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(ListItemMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(CheckMarker, node.OriginalCheckMarker);
        writer.WriteAttributeString(Index, node.Index);
        writer.WriteAttributeString(LeadingSpaces, node.LeadingSpaces.ToString());
        writer.WriteAttributeString(CheckLeadingSpaces, node.CheckLeadingSpaces.ToString());
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, ListItemMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, CheckMarker, out string? checkMarker)) {
            targetNode.WithCheckMarker(checkMarker);
        }

        if (TryGetAttributeAsString(reader, Index, out string? index)) {
            targetNode.WithIndex(index);
        }

        if (TryGetAttributeAsInt32(reader, LeadingSpaces, out int  leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
        if (TryGetAttributeAsInt32(reader, CheckLeadingSpaces, out int checkLeadingSpaces)) {
            targetNode.WithCheckLeadingSpaces(checkLeadingSpaces);
        }
    }
}


