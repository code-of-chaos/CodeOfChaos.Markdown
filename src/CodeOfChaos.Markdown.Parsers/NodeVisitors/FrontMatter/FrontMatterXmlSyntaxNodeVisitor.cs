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
public sealed class FrontMatterXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<FrontMatterMdSyntaxNode> {
    private const string Language = nameof(FrontMatterMdSyntaxNode.Language);
    private const string DashesCount = nameof(FrontMatterMdSyntaxNode.DashesCount);
    private const string LeadingSpaces = nameof(FrontMatterMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(FrontMatterMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        AddXmlPreserveSpace(targetElement);
        targetElement.SetAttributeValue(Language, node.Language);
        targetElement.SetAttributeValue(DashesCount, node.DashesCount);
        targetElement.SetAttributeValue(LeadingSpaces, node.LeadingSpaces);
        targetElement.Value = node.Content;
    }

    protected override void SerializeDetails(XElement element, FrontMatterMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsString(element, Language, out string? language)) {
            targetNode.WithLanguage(language);      
        }

        if (TryGetAttributeAsInt32(element, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);     
        }
        
        if (TryGetAttributeAsInt32(element, DashesCount, out int dashesCount)) {
            targetNode.WithDashesCount(dashesCount);      
        }
        
        targetNode.WithContent(element.Value);
    }
}
