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
public sealed class LinkXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<LinkMdSyntaxNode> {
    private const string Href = nameof(ImageMdSyntaxNode.Href);
    private const string Title = nameof(ImageMdSyntaxNode.Title);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(LinkMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(Href, node.Href);
        if (node.Title.IsNotNullOrEmpty()) writer.WriteAttributeString(Title, node.Title);
    }

    protected override void SerializeDetails(XmlReader reader, LinkMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);
        
        if (TryGetAttributeAsString(reader, Href, out string? href)) {
            targetNode.WithHref(href);
        }
        if (TryGetAttributeAsString(reader, Title, out string? title)) {
            targetNode.WithTitle(title);
        }
    }
}


