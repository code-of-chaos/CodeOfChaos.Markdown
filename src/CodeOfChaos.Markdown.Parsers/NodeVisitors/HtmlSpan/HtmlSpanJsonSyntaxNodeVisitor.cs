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
public sealed class HtmlSpanJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<HtmlSpanMdSyntaxNode> {
    private static readonly string Attributes = nameof(HtmlSpanMdSyntaxNode.Attributes).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(HtmlSpanMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Attributes, node.Attributes);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, HtmlSpanMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Attributes, out string? attributes)) {
            targetNode.WithAttributes(attributes);
        }
    }

}
