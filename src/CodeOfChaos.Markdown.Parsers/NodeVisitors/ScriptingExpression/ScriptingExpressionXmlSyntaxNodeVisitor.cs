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
public sealed class ScriptingExpressionXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingExpressionMdSyntaxNode> {
    private const string FullStatement = nameof(ScriptingExpressionMdSyntaxNode.FullStatement);
    private const string ExpressionStart = nameof(ScriptingExpressionMdSyntaxNode.ExpressionStart);
    private const string ExpressionLength = nameof(ScriptingExpressionMdSyntaxNode.ExpressionLength);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingExpressionMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(FullStatement, node.FullStatement);
        writer.WriteAttributeString(ExpressionStart, node.ExpressionStart.ToString());
        writer.WriteAttributeString(ExpressionLength, node.ExpressionLength.ToString());
    }

    protected override void SerializeDetails(XmlReader reader, ScriptingExpressionMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, FullStatement, out string? fullStatement)
            && TryGetAttributeAsInt32(reader, ExpressionStart, out int expressionStart)
            && TryGetAttributeAsInt32(reader, ExpressionLength, out int expressionLength)
        ) {
            targetNode.WithExpression(fullStatement, expressionStart, expressionLength);
        }
    }
}

