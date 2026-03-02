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
public sealed class ImageXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ImageMdSyntaxNode> {
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

        if (TryGetAttributeAsString(element, AltText, out string? altText)) {
            targetNode.WithAltText(altText);
        }
        
        if (TryGetAttributeAsString(element, Href, out string? href)) {
            targetNode.WithHref(href);
        }
        
        if (TryGetAttributeAsString(element, Title, out string? title)) {
            targetNode.WithTitle(title);
        }
    }
}
