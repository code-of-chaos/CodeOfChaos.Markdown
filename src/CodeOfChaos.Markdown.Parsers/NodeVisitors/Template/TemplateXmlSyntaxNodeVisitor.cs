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
public sealed class TemplateXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<TemplateMdSyntaxNode> {
    private const string BracesCount = nameof(TemplateMdSyntaxNode.BracesCount);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TemplateMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(BracesCount, node.BracesCount);
        targetElement.Value = node.Content;
    }

    protected override void SerializeDetails(XElement element, TemplateMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        
        targetNode.WithContent(element.Value);

        if (TryGetAttributeAsInt32(element, BracesCount, out int bracesCount)) {
            targetNode.WithBracesCount(bracesCount);
        }
    }
}
