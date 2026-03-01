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
public sealed class EscapedCharacterXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<EscapedCharacterMdSyntaxNode> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(EscapedCharacterMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        AddXmlPreserveSpace(targetElement);
        targetElement.Value = node.Content.ToString();
    }

    protected override void SerializeDetails(XElement element, EscapedCharacterMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithContent(element.Value.ElementAtOrDefault(0));
    }
}
