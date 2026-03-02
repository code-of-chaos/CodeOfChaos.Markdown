// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ScriptingIfStatementMdSyntaxNode : MdSyntaxNode<ScriptingIfStatementMdSyntaxNode> {
    public int ElseConditionIndex { get; private set; } = -1;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ScriptingIfStatementMdSyntaxNode WithIfCondition(ScriptingExpressionMdSyntaxNode expressionNode) {
        AddChildNode(expressionNode);
        return this;
    }
    
    public ScriptingIfStatementMdSyntaxNode WithElseCondition(ScriptingExpressionMdSyntaxNode expressionNode) {
        if (ElseConditionIndex != -1) {
            throw new InvalidOperationException("Else child already set.");
        }

        AddChildNode(expressionNode);
        ElseConditionIndex = ChildCount - 1;
        return this;
    }
    
    internal void WithElseConditionIndex(int parse) {
        ElseConditionIndex = parse;
    }
    
    public override bool TryReset() {
        ElseConditionIndex = -1;
        return base.TryReset();
    }

    protected override bool Equals(ScriptingIfStatementMdSyntaxNode? other)
        => base.Equals(other)
            && ElseConditionIndex == other.ElseConditionIndex;
}
