// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json.NodeVisitors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ScriptingExpressionJsonSyntaxNodeVisitor : BaseJsonSyntaxNodeVisitor<ScriptingExpressionSyntaxNode> {
    private static readonly string FullStatement = nameof(ScriptingExpressionSyntaxNode.FullStatement).ToCamelCase();
    private static readonly string ExpressionStart = nameof(ScriptingExpressionSyntaxNode.ExpressionStart).ToCamelCase();
    private static readonly string ExpressionLength = nameof(ScriptingExpressionSyntaxNode.ExpressionLength).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingExpressionSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);
        
        writer.WriteString(FullStatement, node.FullStatement);
        writer.WriteNumber(ExpressionStart, node.ExpressionStart);
        writer.WriteNumber(ExpressionLength, node.ExpressionLength);
    }

    protected override void SerializeDetails(JsonElement element, ScriptingExpressionSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        
        element.TryGetProperty(FullStatement, out JsonElement fullStatementProperty);
        element.TryGetProperty(ExpressionStart, out JsonElement expressionStartProperty);
        element.TryGetProperty(ExpressionLength, out JsonElement expressionLengthProperty);

        targetNode.WithExpression(fullStatementProperty.GetString() ?? string.Empty, expressionStartProperty.GetInt32(), expressionLengthProperty.GetInt32());
    }
}
