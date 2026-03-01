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
public sealed class FootnoteReferenceXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<FootnoteReferenceMdSyntaxNode> {
    private const string Identifier = nameof(FootnoteReferenceMdSyntaxNode.Identifier);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(FootnoteReferenceMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Identifier, node.Identifier);
    }

    protected override void SerializeDetails(XElement element, FootnoteReferenceMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithIdentifier(element.Attribute(Identifier)?.Value ?? string.Empty);
    }
}
