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
public sealed class CodeBlockJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<CodeBlockMdSyntaxNode> {
    private static readonly string Language = nameof(CodeBlockMdSyntaxNode.Language).ToCamelCase();
    private static readonly string Content = nameof(CodeBlockMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CodeBlockMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Language, node.Language);
        writer.WriteString(Content, node.Content);
    }

    protected override void SerializeDetails(JsonElement element, CodeBlockMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Language, out string? language)) {
            targetNode.WithLanguage(language);
        }

        if (TryGetPropertyAsString(element, Content, out string? content)) {
            targetNode.WithContent(content);
        }
    }
}
