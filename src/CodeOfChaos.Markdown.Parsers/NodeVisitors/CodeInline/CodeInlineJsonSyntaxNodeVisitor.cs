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
/// <inheritdoc />
public sealed class CodeInlineJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<CodeInlineMdSyntaxNode> {
    private static readonly string BackTickCount = nameof(CodeInlineMdSyntaxNode.BackTickCount).ToCamelCase();
    private static readonly string Content = nameof(CodeInlineMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(CodeInlineMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(BackTickCount, node.BackTickCount);
        writer.WriteString(Content, node.Content);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, CodeInlineMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsInt32(element, BackTickCount, out int backTickCount)) {
            targetNode.WithBackTickCount(backTickCount);
        }
        
        if (TryGetPropertyAsString(element, Content, out string? content)) {
            targetNode.WithContent(content);
        }
    }

}
