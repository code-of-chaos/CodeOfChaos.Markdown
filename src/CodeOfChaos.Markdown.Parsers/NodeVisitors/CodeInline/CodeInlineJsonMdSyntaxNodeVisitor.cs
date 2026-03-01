// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class CodeInlineJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<CodeInlineMdSyntaxNode> {
    private static readonly string BackTickCount = nameof(CodeInlineMdSyntaxNode.BackTickCount).ToCamelCase();
    private static readonly string Content = nameof(CodeInlineMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CodeInlineMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(BackTickCount, node.BackTickCount);
        writer.WriteString(Content, node.Content);
    }

    protected override void SerializeDetails(JsonElement element, CodeInlineMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (element.TryGetProperty(BackTickCount, out JsonElement backTickCountProperty)) {
            targetNode.WithBackTickCount(backTickCountProperty.GetInt32());
        }

        if (element.TryGetProperty(Content, out JsonElement contentProperty)) {
            targetNode.WithContent(contentProperty.GetString() ?? string.Empty);
        }
    }

}
