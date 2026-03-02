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
public sealed class HeadingSimpleXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HeadingSimpleMdSyntaxNode> {
    private const string Identifier = nameof(HeadingSimpleMdSyntaxNode.Identifier);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HeadingSimpleMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(Identifier, node.Identifier);
    }

    protected override void SerializeDetails(XElement element, HeadingSimpleMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsString(element, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }
}
