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
public sealed class ListUnorderedXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ListUnorderedMdSyntaxNode> {
    private const string LeadingSpaces = nameof(ListUnorderedMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ListUnorderedMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(XElement element, ListUnorderedMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
    }
}
