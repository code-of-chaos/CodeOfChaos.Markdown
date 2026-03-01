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
public sealed class UserJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<UserMdSyntaxNode> {
    private static readonly string Content = nameof(UserMdSyntaxNode.Content).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(UserMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Content, node.Content);
    }

    protected override void SerializeDetails(JsonElement element, UserMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (element.TryGetProperty(Content, out JsonElement contentProperty)) {
            targetNode.WithContent(contentProperty.GetString() ?? string.Empty);
        }
    }
}
