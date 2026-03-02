// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ListItemXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ListItemMdSyntaxNode> {
    private const string LeadingSpaces = nameof(ListItemMdSyntaxNode.LeadingSpaces);
    private const string CheckLeadingSpaces = nameof(ListItemMdSyntaxNode.CheckLeadingSpaces);
    private const string Index = nameof(ListItemMdSyntaxNode.Index);
    private const string CheckMarker = nameof(CheckMarker);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ListItemMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(CheckMarker, node.OriginalCheckMarker);
        targetElement.SetAttributeValue(Index, node.Index);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
        targetElement.SetAttributeValue(CheckLeadingSpaces, node.CheckLeadingSpaces);
    }

    protected override void SerializeDetails(XElement element, ListItemMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsString(element, CheckMarker, out string? checkMarker)) {
            targetNode.WithCheckMarker(checkMarker);
        }

        if (TryGetAttributeAsString(element, Index, out string? index)) {
            targetNode.WithIndex(index);
        }

        if (TryGetAttributeAsInt32(element, LeadingSpaces, out int  leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
        if (TryGetAttributeAsInt32(element, CheckLeadingSpaces, out int checkLeadingSpaces)) {
            targetNode.WithCheckLeadingSpaces(checkLeadingSpaces);
        }
    }
}
