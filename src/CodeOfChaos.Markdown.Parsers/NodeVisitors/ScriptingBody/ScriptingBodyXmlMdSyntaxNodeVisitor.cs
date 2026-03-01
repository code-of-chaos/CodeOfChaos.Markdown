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
public sealed class ScriptingBodyXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingBodySyntaxNode> {
    private const string LeadingSpaces = nameof(ScriptingBodySyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingBodySyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(XElement element, ScriptingBodySyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        targetNode.WithLeadingSpaces(int.Parse(element.Attribute(LeadingSpaces)?.Value ?? "0"));
    }
}
