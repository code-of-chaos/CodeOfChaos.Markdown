// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ScriptingExpressionJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<ScriptingExpressionSyntaxNode> {
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

        if (TryGetPropertyAsString(element, FullStatement, out string? fullStatement)
            && TryGetPropertyAsInt32(element, ExpressionStart, out int expressionStart)
            && TryGetPropertyAsInt32(element, ExpressionLength, out int expressionLength)
        ) {
            targetNode.WithExpression(fullStatement, expressionStart, expressionLength);
        }

    }
}
