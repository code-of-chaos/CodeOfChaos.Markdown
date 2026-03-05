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
    private const string BracesCount = nameof(TemplateMdSyntaxNode.BracesCount);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TemplateMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(BracesCount, node.BracesCount.ToString());
        WriteElementContent(writer, node.Content);
    }

    protected override void SerializeDetails(XmlReader reader, TemplateMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsInt32(reader, BracesCount, out int bracesCount)) {
            targetNode.WithBracesCount(bracesCount);
        }
    }

    protected override void SerializeContent(TemplateMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


