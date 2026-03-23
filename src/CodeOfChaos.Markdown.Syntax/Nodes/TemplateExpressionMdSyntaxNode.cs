// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public class TemplateExpressionMdSyntaxNode : MdSyntaxNode<TemplateExpressionMdSyntaxNode> {
    /// <summary>
    /// Represents the type of a template expression within a Markdown syntax node.
    /// </summary>
    /// <remarks>
    /// The <c>ExpressionType</c> property specifies the category or purpose of the template expression.
    /// Examples include literal values, conditional branches like "if," "else if," and "else."
    /// </remarks>
    public TemplateExpressionType ExpressionType { get; private set; }
    /// <summary>
    /// Gets the expression text associated with the template expression node.
    /// </summary>
    /// <remarks>
    /// This property is used to retrieve or manipulate the expression content
    /// within a template expression syntax node. It is initialized to an empty
    /// string by default and is updated via the <c>WithExpression</c> method.
    /// </remarks>
    /// <value>
    /// A string representing the expression text.
    /// </value>
    public string Expression { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the number of leading spaces associated with the template expression.
    /// </summary>
    /// <remarks>
    /// This property represents the count of whitespace characters located at the
    /// beginning of the template expression. Leading spaces can be used to
    /// determine the indentation level for markdown syntax nodes or to adjust
    /// formatting based on the structure of the expression.
    /// </remarks>
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// Retrieves a pooled instance of a `TemplateExpressionMdSyntaxNode` configured with `TemplateExpressionType.Literal`.
    /// <returns>A pooled instance of `TemplateExpressionMdSyntaxNode` with the expression type set to `Literal`.</returns>
    public static TemplateExpressionMdSyntaxNode GetPooledLiteral() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.Literal);

    /// Retrieves a pooled instance of the TemplateExpressionMdSyntaxNode,
    /// initialized with the expression type set to "If".
    /// <returns>A pooled instance of TemplateExpressionMdSyntaxNode with the "If" expression type.</returns>
    public static TemplateExpressionMdSyntaxNode GetPooledIf() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.If);

    /// Retrieves a pooled instance of a `TemplateExpressionMdSyntaxNode` configured with the `ElseIf` expression type.
    /// <return>A pooled instance of `TemplateExpressionMdSyntaxNode` with `ExpressionType` set to `ElseIf`.</return>
    public static TemplateExpressionMdSyntaxNode GetPooledElseIf() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.ElseIf);

    /// Retrieves a pooled instance of the TemplateExpressionMdSyntaxNode with its expression type set to 'Else'.
    /// <return>Pooled instance of TemplateExpressionMdSyntaxNode configured with the 'Else' expression type.</return>
    public static TemplateExpressionMdSyntaxNode GetPooledElse() => MdSyntaxNodePool<TemplateExpressionMdSyntaxNode>.Shared.Get()
        .WithExpressionType(TemplateExpressionType.Else);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the type of the template expression.
    /// <param name="type">The <see cref="TemplateExpressionType"/> value to be assigned to the expression.</param>
    /// <return>Returns the current <see cref="TemplateExpressionMdSyntaxNode"/> with the updated expression type.</return>
    public TemplateExpressionMdSyntaxNode WithExpressionType(TemplateExpressionType type) {
        ExpressionType = type;
        return this;
    }

    /// Updates the expression associated with the current syntax node.
    /// <param name="expression">The new value for the expression to be set.</param>
    /// <returns>The updated instance of <see cref="TemplateExpressionMdSyntaxNode"/>.</returns>
    public TemplateExpressionMdSyntaxNode WithExpression(string expression) {
        Expression = expression;
        return this;
    }

    /// <summary>
    /// Sets the number of leading spaces for the current Markdown syntax node.
    /// </summary>
    /// <param name="leadingSpaces">The number of leading spaces to be assigned to the node.</param>
    /// <returns>The current instance with the updated leading spaces.</returns>
    public TemplateExpressionMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = leadingSpaces;
        return this;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        ExpressionType = TemplateExpressionType.Unknown;
        Expression = string.Empty;
        LeadingSpaces = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals([NotNullWhen(true)] TemplateExpressionMdSyntaxNode? other)
        => base.Equals(other)
            && ExpressionType == other.ExpressionType
            && LeadingSpaces == other.LeadingSpaces
            && StringComparer.Ordinal.Equals(Expression, other.Expression);

    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{ExpressionTypeToString(ExpressionType)}' '{Expression}' LS={LeadingSpaces}";

    private static string ExpressionTypeToString(TemplateExpressionType type) => type switch {
        TemplateExpressionType.Literal => "Literal",
        TemplateExpressionType.If => "If",
        TemplateExpressionType.ElseIf => "Else If",
        TemplateExpressionType.Else => "Else",

        TemplateExpressionType.Unknown => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
}

/// Represents the type of a template expression in the Markdown syntax tree.
/// Used to define different kinds of template expressions that can be processed
/// within a Markdown document.
/// The types include:
/// - Literal: Represents a literal expression.
/// - If: Represents a conditional "if" expression.
/// - ElseIf: Represents a conditional "else-if" expression.
/// - Else: Represents a conditional "else" expression.
/// - Unknown: Represents an undefined or invalid expression type.
public enum TemplateExpressionType {
    /// Represents an unspecified or uninitialized state for a template expression type.
    Unknown = 0,
    /// Represents a literal expression type in a template.
    /// Used when the expression is a fixed or unchanging value within the template context.
    Literal,
    /// Represents a conditional "If" expression type within a template expression.
    /// Used to denote a conditional branching point in the syntax tree structure.
    If,
    /// Represents a conditional expression type that evaluates only when the preceding "If" or "ElseIf" condition is not met, and an additional condition is provided.
    ElseIf,
    /// Represents the "Else" conditional branch in a template expression.
    /// This member is used to denote an alternative execution path
    /// when the preceding "If" or "ElseIf" conditions are not met.
    Else,
}
