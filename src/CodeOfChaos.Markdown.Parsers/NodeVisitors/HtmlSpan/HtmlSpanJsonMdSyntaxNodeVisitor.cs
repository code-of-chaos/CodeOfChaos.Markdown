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
public sealed class HtmlSpanJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<HtmlSpanMdSyntaxNode> {
    private static readonly string Attributes = nameof(HtmlSpanMdSyntaxNode.Attributes).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HtmlSpanMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Attributes, node.Attributes);
    }

    protected override void SerializeDetails(JsonElement element, HtmlSpanMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (element.TryGetProperty(Attributes, out JsonElement attributesProperty)) {
            targetNode.WithAttributes(attributesProperty.GetString() ?? string.Empty);
        }
    }

}
