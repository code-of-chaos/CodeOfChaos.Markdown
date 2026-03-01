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
public sealed class HtmlSpanXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HtmlSpanMdSyntaxNode> {
    private const string Attributes = nameof(HtmlSpanMdSyntaxNode.Attributes);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HtmlSpanMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Attributes, node.Attributes);
    }

    protected override void SerializeDetails(XElement element, HtmlSpanMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithAttributes(element.Attribute(Attributes)?.Value ?? string.Empty);
    }
}
