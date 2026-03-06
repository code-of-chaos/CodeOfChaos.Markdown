// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TemplatingLiteralStatementMdSyntaxNode() : MdSyntaxNode<TemplatingLiteralStatementMdSyntaxNode>(initialChildCount: 1) {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TemplatingLiteralStatementMdSyntaxNode WithLiteralBody(string content) {
        TemplateExpressionMdSyntaxNode childNode = TemplateExpressionMdSyntaxNode.GetPooledLiteral();
        childNode.WithExpression(content);
        WithChild(childNode);
        
        return this;
    }
}
