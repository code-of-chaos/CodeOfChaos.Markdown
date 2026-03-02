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
public sealed class LinkXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<LinkMdSyntaxNode> {
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
        
        if (TryGetAttributeAsString(element, Href, out string? href)) {
            targetNode.WithHref(href);
        }
        if (TryGetAttributeAsString(element, Title, out string? title)) {
            targetNode.WithTitle(title);
        }
    }
}
