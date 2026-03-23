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
/// <inheritdoc />
public sealed class WikiLinkXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<WikiLinkMdSyntaxNode> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(WikiLinkMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteElementContent(writer, node.Content);
    }

    /// <inheritdoc />
    protected override void SerializeContent(WikiLinkMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


