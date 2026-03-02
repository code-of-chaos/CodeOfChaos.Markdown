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
public sealed class ScriptingIfStatementXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingIfStatementMdSyntaxNode> {
    private const string ElseConditionIndex = nameof(ScriptingIfStatementMdSyntaxNode.ElseConditionIndex);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingIfStatementMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(ElseConditionIndex, node.ElseConditionIndex);
    }

    protected override void SerializeDetails(XElement element, ScriptingIfStatementMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsInt32(element, ElseConditionIndex, out int elseConditionIndex)) {
            targetNode.WithElseConditionIndex(elseConditionIndex);
        }
    }
}
