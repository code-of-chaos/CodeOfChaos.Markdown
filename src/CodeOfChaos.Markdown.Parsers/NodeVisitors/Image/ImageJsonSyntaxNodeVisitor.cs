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
public sealed class ImageJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<ImageMdSyntaxNode> {
    private static readonly string Href = nameof(ImageMdSyntaxNode.Href).ToCamelCase();
    private static readonly string Title = nameof(ImageMdSyntaxNode.Title).ToCamelCase();
    private static readonly string AltText = nameof(AltText).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ImageMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Href, node.Href);
        writer.WriteString(AltText, node.OriginalAltText);
        if (node.Title.IsNotNullOrEmpty()) writer.WriteString(Title, node.Title);
    }

    protected override void SerializeDetails(JsonElement element, ImageMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, AltText, out string? altText)) {
            targetNode.WithAltText(altText);
        }

        if (TryGetPropertyAsString(element, Href, out string? href)) {
            targetNode.WithHref(href);       
        }

        if (TryGetPropertyAsString(element, Title, out string? title)) {
            targetNode.WithTitle(title);     
        }
    }

}
