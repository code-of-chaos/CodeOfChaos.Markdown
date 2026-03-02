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
public sealed class ScriptingIfStatementXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingIfStatementSyntaxNode> {
    private const string ElseConditionIndex = nameof(ScriptingIfStatementSyntaxNode.ElseConditionIndex);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingIfStatementSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(ElseConditionIndex, node.ElseConditionIndex);
    }

    protected override void SerializeDetails(XElement element, ScriptingIfStatementSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsInt32(element, ElseConditionIndex, out int elseConditionIndex)) {
            targetNode.WithElseConditionIndex(elseConditionIndex);
        }
    }
}
