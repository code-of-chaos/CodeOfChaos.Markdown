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
public sealed class TemplateXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<TemplateMdSyntaxNode> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TemplateMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteElementContent(writer, node.Content);
    }

    protected override void SerializeContent(TemplateMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


