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
public sealed class HtmlXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HtmlMdSyntaxNode> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HtmlMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        WriteElementContent(writer, node.Content);
    }

    protected override void SerializeContent(HtmlMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


