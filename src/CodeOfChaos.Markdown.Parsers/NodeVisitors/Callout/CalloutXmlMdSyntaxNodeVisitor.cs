// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class CalloutXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CalloutMdSyntaxNode> {
    private const string CalloutType = nameof(CalloutMdSyntaxNode.CalloutType);
    private const string CollapsedState = nameof(CalloutMdSyntaxNode.CollapsedState);
    private const string LeadingSpaces = nameof(CalloutMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CalloutMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);

        targetElement.SetAttributeValue(CalloutType, node.CalloutType);
        targetElement.SetAttributeValue(CollapsedState, node.CollapsedState);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
    }

    protected override void SerializeDetails(XElement element, CalloutMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }

        if (TryGetAttributeAsString(element, CalloutType, out string? calloutType)) {
            targetNode.WithCalloutType(calloutType);
        }
        
        if (TryGetAttributeAsEnum(element, CollapsedState, out CalloutMdSyntaxNode.CollapseStateOptions collapsedState)) {
            targetNode.WithCollapseState(collapsedState);
        }
    }
}
