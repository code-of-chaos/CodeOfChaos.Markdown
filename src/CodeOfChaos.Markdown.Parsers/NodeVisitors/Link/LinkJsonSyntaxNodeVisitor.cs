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
public sealed class LinkJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<LinkMdSyntaxNode> {
    private static readonly string Href = nameof(LinkMdSyntaxNode.Href).ToCamelCase();
    private static readonly string Title = nameof(LinkMdSyntaxNode.Title).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(LinkMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        if (node.Href.IsNotNullOrEmpty()) writer.WriteString(Href, node.Href);
        if (node.Title.IsNotNullOrEmpty()) writer.WriteString(Title, node.Title);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, LinkMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Href, out string? href)) {
            targetNode.WithHref(href);
        }
        
        if (TryGetPropertyAsString(element, Title, out string? title)) {
            targetNode.WithTitle(title);
        }
    }
}
