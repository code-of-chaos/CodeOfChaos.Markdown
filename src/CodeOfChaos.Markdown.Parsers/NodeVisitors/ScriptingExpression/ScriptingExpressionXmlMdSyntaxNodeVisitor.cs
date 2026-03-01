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
public sealed class ScriptingExpressionXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<ScriptingExpressionSyntaxNode> {
    private const string FullStatement = nameof(ScriptingExpressionSyntaxNode.FullStatement);
    private const string ExpressionStart = nameof(ScriptingExpressionSyntaxNode.ExpressionStart);
    private const string ExpressionLength = nameof(ScriptingExpressionSyntaxNode.ExpressionLength);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingExpressionSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(FullStatement, node.FullStatement);
        targetElement.SetAttributeValue(ExpressionStart, node.ExpressionStart);
        targetElement.SetAttributeValue(ExpressionLength, node.ExpressionLength);
    }

    protected override void SerializeDetails(XElement element, ScriptingExpressionSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        
        string fullStatement = element.Attribute(FullStatement)?.Value ?? string.Empty;
        int expressionStart = int.Parse(element.Attribute(ExpressionStart)?.Value ?? "0");
        int expressionLength = int.Parse(element.Attribute(ExpressionLength)?.Value ?? "0");
        
        targetNode.WithExpression(fullStatement, expressionStart, expressionLength);
    }
}
