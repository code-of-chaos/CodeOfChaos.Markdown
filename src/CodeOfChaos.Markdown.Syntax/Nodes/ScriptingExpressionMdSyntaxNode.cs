// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ScriptingExpressionMdSyntaxNode : MdSyntaxNode<ScriptingExpressionMdSyntaxNode> {
    public string FullStatement { get; private set; } = string.Empty;
    public int ExpressionStart { get; private set; }
    public int ExpressionLength { get; private set; }
    public ReadOnlySpan<char> ExpressionValue => FullStatement.AsSpan(ExpressionStart, ExpressionLength);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ScriptingExpressionMdSyntaxNode WithExpression(string fullStatement, int expressionStart, int expressionLength) {
        FullStatement = fullStatement;
        ExpressionStart = expressionStart;
        ExpressionLength = expressionLength;
        return this;
    }
    
    public override bool TryReset() {
        FullStatement = string.Empty;
        ExpressionStart = 0;
        ExpressionLength = 0;
        return base.TryReset();
    }

    protected override bool Equals(ScriptingExpressionMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(FullStatement, other.FullStatement)
            && ExpressionStart == other.ExpressionStart
            && ExpressionLength == other.ExpressionLength;

    public override string ToDebugString() 
        => $"{base.ToDebugString()}:'{ExpressionValue}'";
}
