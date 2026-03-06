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
public sealed class TemplateExpressionJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<TemplateExpressionMdSyntaxNode> {
    private static readonly string ExpressionType = nameof(TemplateExpressionMdSyntaxNode.ExpressionType).ToCamelCase();
    private static readonly string Expression = nameof(TemplateExpressionMdSyntaxNode.Expression).ToCamelCase();
    private static readonly string BodyLeadingSpaces = nameof(TemplateExpressionMdSyntaxNode.BodyLeadingSpaces).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TemplateExpressionMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(BodyLeadingSpaces, node.BodyLeadingSpaces);
        writer.WriteString(ExpressionType, Enum.GetName(node.ExpressionType));
        writer.WriteString(Expression, node.Expression);
    }

    protected override void SerializeDetails(JsonElement element, TemplateExpressionMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Expression, out string? content)) {
            targetNode.WithExpression(content);   
        }

        if (TryGetPropertyAsEnum(element, ExpressionType, out TemplateExpressionType expressionType)) {
            targetNode.WithExpressionType(expressionType);  
        }

        if (TryGetPropertyAsInt32(element, BodyLeadingSpaces, out int leadingSpaces)) {
            targetNode.WithBodyLeadingSpaces(leadingSpaces);
        }
    }

}
