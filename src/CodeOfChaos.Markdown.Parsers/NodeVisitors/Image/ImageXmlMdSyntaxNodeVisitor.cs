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
public sealed class ImageXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ImageMdSyntaxNode> {
    private const string Href = nameof(ImageMdSyntaxNode.Href);
    private const string Title = nameof(ImageMdSyntaxNode.Title);
    private const string AltText = nameof(AltText);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ImageMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Href, node.Href);
        targetElement.SetAttributeValue(AltText, node.OriginalAltText);
        if (node.Title.IsNotNullOrEmpty()) targetElement.SetAttributeValue(Title, node.Title);
    }

    protected override void SerializeDetails(XElement element, ImageMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithAltText(element.Attribute(AltText)?.Value ?? string.Empty);
        targetNode.WithHref(element.Attribute(Href)?.Value ?? string.Empty);
    }
}
