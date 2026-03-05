// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class CalloutXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CalloutMdSyntaxNode> {
    private const string CalloutType = nameof(CalloutMdSyntaxNode.CalloutType);
    private const string CollapsedState = nameof(CalloutMdSyntaxNode.CollapsedState);
    private const string LeadingSpaces = nameof(CalloutMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CalloutMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteAttributeString(CalloutType, node.CalloutType);
        writer.WriteAttributeString(CollapsedState, node.CollapsedState.ToString());
        writer.WriteAttributeString(LeadingSpaces, node.LeadingSpaces.ToString());
    }

    protected override void SerializeDetails(XmlReader reader, CalloutMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsInt32(reader, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }

        if (TryGetAttributeAsString(reader, CalloutType, out string? calloutType)) {
            targetNode.WithCalloutType(calloutType);
        }
        
        if (TryGetAttributeAsEnum(reader, CollapsedState, out CalloutMdSyntaxNode.CollapseStateOptions collapsedState)) {
            targetNode.WithCollapseState(collapsedState);
        }
    }
}


