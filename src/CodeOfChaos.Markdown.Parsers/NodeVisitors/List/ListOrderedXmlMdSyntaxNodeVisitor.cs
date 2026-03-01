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
public sealed class ListOrderedXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ListOrderedMdSyntaxNode> {
    private const string LeadingSpaces = nameof(ListOrderedMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ListOrderedMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(XElement element, ListOrderedMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithLeadingSpaces(int.Parse(element.Attribute(LeadingSpaces)?.Value ?? "0"));
    }
}
