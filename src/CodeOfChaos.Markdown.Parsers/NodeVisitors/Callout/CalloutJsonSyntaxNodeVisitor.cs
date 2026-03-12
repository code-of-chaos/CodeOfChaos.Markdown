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
public sealed class CalloutJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<CalloutMdSyntaxNode> {
    private static readonly string CalloutType = nameof(CalloutMdSyntaxNode.CalloutType).ToCamelCase();
    private static readonly string CollapsedState = nameof(CalloutMdSyntaxNode.CollapsedState).ToCamelCase();
    private static readonly string LeadingSpaces = nameof(CalloutMdSyntaxNode.LeadingSpaces).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(CalloutMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteString(CalloutType, node.CalloutType);
        writer.WriteString(CollapsedState, node.CollapsedState.ToString());
        writer.WriteNumber(LeadingSpaces, node.LeadingSpaces);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(JsonElement element, CalloutMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }

        if (TryGetPropertyAsString(element, CalloutType, out string? calloutType)) {
            targetNode.WithCalloutType(calloutType);
        }
        
        if (TryGetPropertyAsEnum(element, CollapsedState, out CalloutCollapseStateOptions collapsedState)) {
            targetNode.WithCollapseState(collapsedState);
        }
    }

}
