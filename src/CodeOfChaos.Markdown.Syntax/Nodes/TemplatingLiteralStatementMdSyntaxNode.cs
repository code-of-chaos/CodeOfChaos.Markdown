// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public class TemplatingLiteralStatementMdSyntaxNode() : MdSyntaxNode<TemplatingLiteralStatementMdSyntaxNode>(initialChildCount: 1) {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Configures the current instance of the TemplatingLiteralStatementMdSyntaxNode
    /// with a child node containing a literal body defined by the given content.
    /// <param name="content">
    /// A string representing the content to initialize the literal body of the child node.
    /// </param>
    /// <returns>
    /// Returns the current instance of the TemplatingLiteralStatementMdSyntaxNode with the specified child node added.
    /// </returns>
    public TemplatingLiteralStatementMdSyntaxNode WithLiteralBody(string content) {
        TemplateExpressionMdSyntaxNode childNode = TemplateExpressionMdSyntaxNode.GetPooledLiteral();
        childNode.WithExpression(content);
        WithChild(childNode);
        
        return this;
    }
}
