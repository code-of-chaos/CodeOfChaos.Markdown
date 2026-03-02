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
public sealed class TextJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<TextMdSyntaxNode> {
    private static readonly string Content = nameof(TextMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TextMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Content, node.Content);
    }

    protected override void SerializeDetails(JsonElement element, TextMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Content, out string? content)) {
            targetNode.WithContent(content);  
        }
    }

}
