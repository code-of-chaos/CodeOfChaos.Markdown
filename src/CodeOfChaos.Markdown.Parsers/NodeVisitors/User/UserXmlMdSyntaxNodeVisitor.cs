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
public sealed class UserXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<UserMdSyntaxNode> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(UserMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        AddXmlPreserveSpace(targetElement);
        targetElement.Value = node.Content;
    }

    protected override void SerializeDetails(XElement element, UserMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithContent(element.Value);
    }
}
