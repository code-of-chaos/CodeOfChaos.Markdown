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
public sealed class TemplateJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<TemplateMdSyntaxNode> {
    private static readonly string BracesCount = nameof(TemplateMdSyntaxNode.BracesCount).ToCamelCase();
    private static readonly string Content = nameof(TemplateMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TemplateMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(BracesCount, node.BracesCount);
        writer.WriteString(Content, node.Content);
    }

    protected override void SerializeDetails(JsonElement element, TemplateMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Content, out string? content)) {
            targetNode.WithContent(content);   
        }

        if (TryGetPropertyAsInt32(element, BracesCount, out int bracesCount)) {
            targetNode.WithBracesCount(bracesCount);
        }
    }

}
