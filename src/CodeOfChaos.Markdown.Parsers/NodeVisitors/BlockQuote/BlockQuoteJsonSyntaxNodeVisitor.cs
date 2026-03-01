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
public sealed class BlockQuoteJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<BlockQuoteMdSyntaxNode> {
    private static readonly string LeadingSpaces = nameof(BlockQuoteMdSyntaxNode.LeadingSpaces).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(BlockQuoteMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);
        
        writer.WriteNumber(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(JsonElement element, BlockQuoteMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
    }

}
