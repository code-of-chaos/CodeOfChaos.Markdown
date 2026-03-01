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
public sealed class LinkXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<LinkMdSyntaxNode> {
    private const string Href = nameof(ImageMdSyntaxNode.Href);
    private const string Title = nameof(ImageMdSyntaxNode.Title);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(LinkMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Href, node.Href);
        if (node.Title.IsNotNullOrEmpty()) targetElement.SetAttributeValue(Title, node.Title);
    }

    protected override void SerializeDetails(XElement element, LinkMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithHref(element.Attribute(Href)?.Value ?? string.Empty);
        targetNode.WithTitle(element.Attribute(Title)?.Value ?? string.Empty);
    }
}
