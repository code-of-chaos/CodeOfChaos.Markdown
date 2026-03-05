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
public sealed class ImageXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ImageMdSyntaxNode> {
    private const string Href = nameof(ImageMdSyntaxNode.Href);
    private const string Title = nameof(ImageMdSyntaxNode.Title);
    private const string AltText = nameof(AltText);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ImageMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(Href, node.Href);
        writer.WriteAttributeString(AltText, node.OriginalAltText);
        if (node.Title.IsNotNullOrEmpty()) writer.WriteAttributeString(Title, node.Title);
    }

    protected override void SerializeDetails(XmlReader reader, ImageMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, AltText, out string? altText)) {
            targetNode.WithAltText(altText);
        }
        
        if (TryGetAttributeAsString(reader, Href, out string? href)) {
            targetNode.WithHref(href);
        }
        
        if (TryGetAttributeAsString(reader, Title, out string? title)) {
            targetNode.WithTitle(title);
        }
    }
}


