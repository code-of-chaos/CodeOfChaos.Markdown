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
public sealed class FrontMatterJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<FrontMatterMdSyntaxNode> {
    private static readonly string Language = nameof(FrontMatterMdSyntaxNode.Language).ToCamelCase();
    private static readonly string Content = nameof(FrontMatterMdSyntaxNode.Content).ToCamelCase();
    private static readonly string LeadingSpaces = nameof(FrontMatterMdSyntaxNode.LeadingSpaces).ToCamelCase();
    private static readonly string DashesCount = nameof(FrontMatterMdSyntaxNode.DashesCount).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(FrontMatterMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(Language, node.Language);
        writer.WriteString(Content, node.Content);
        writer.WriteString(LeadingSpaces, node.LeadingSpaces.ToString());
        writer.WriteString(DashesCount, node.DashesCount.ToString());
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, FrontMatterMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsString(element, Language, out string? language)) {
            targetNode.WithLanguage(language);       
        }

        if (TryGetPropertyAsString(element, Content, out string? content)) {
            targetNode.WithContent(content);      
        }

        if (TryGetPropertyAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);      
        }

        if (TryGetPropertyAsInt32(element, DashesCount, out int dashesCount)) {
            targetNode.WithDashesCount(dashesCount);     
        }
    }
}
