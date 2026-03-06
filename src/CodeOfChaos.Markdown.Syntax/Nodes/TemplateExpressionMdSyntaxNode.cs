// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TemplateExpressionMdSyntaxNode : MdSyntaxNode<TemplateExpressionMdSyntaxNode> {
    public TemplateExpressionType ExpressionType { get; private set; }
    public string Expression { get; private set; } = string.Empty;
    public int BodyLeadingSpaces { get; private set; }
    
    public enum TemplateExpressionType {
        Unknown = 0,
        Literal,
        If,
        ElseIf,
        Else,
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static TemplateExpressionMdSyntaxNode GetPooledLiteral() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.Literal);
    
    public static TemplateExpressionMdSyntaxNode GetPooledIf() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.If);
    
    public static TemplateExpressionMdSyntaxNode GetPooledElseIf() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.ElseIf);
    
    public static TemplateExpressionMdSyntaxNode GetPooledElse() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.Else);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public TemplateExpressionMdSyntaxNode WithExpressionType(TemplateExpressionType type) {
        ExpressionType = type;
        return this;    
    }
    
    public TemplateExpressionMdSyntaxNode WithExpression(string expression) {
        Expression = expression;
        return this;
    }
    
    public TemplateExpressionMdSyntaxNode WithBodyLeadingSpaces(int leadingSpaces) {
        BodyLeadingSpaces = leadingSpaces;
        return this;
    }

    public override bool TryReset() {
        ExpressionType = TemplateExpressionType.Unknown;
        Expression = string.Empty;
        BodyLeadingSpaces = 0;
        return base.TryReset();   
    }

    protected override bool Equals([NotNullWhen(true)] TemplateExpressionMdSyntaxNode? other) 
        => base.Equals(other)
            && ExpressionType == other.ExpressionType
            && BodyLeadingSpaces == other.BodyLeadingSpaces
            && StringComparer.Ordinal.Equals(Expression, other.Expression);
    
}
