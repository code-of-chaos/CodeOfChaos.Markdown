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
public sealed class ScriptingIfStatementXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingIfStatementMdSyntaxNode> {
    private const string ElseConditionIndex = nameof(ScriptingIfStatementMdSyntaxNode.ElseConditionIndex);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingIfStatementMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(ElseConditionIndex, node.ElseConditionIndex.ToString());
    }

    protected override void SerializeDetails(XmlReader reader, ScriptingIfStatementMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsInt32(reader, ElseConditionIndex, out int elseConditionIndex)) {
            targetNode.WithElseConditionIndex(elseConditionIndex);
        }
    }
}


