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
public sealed class WikiLinkXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<WikiLinkMdSyntaxNode> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(WikiLinkMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteElementContent(writer, node.Content);
    }

    protected override void SerializeContent(WikiLinkMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


