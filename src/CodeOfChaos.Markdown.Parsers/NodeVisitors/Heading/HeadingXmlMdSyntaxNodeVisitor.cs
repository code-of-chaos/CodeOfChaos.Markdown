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
public sealed class HeadingXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HeadingMdSyntaxNode> {
    private const string Level = nameof(HeadingMdSyntaxNode.Level);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HeadingMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Level, node.Level);
    }

    protected override void SerializeDetails(XElement element, HeadingMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithLevel(int.Parse(element.Attribute(Level)?.Value ?? "0"));
    }
}
