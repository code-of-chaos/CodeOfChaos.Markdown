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
/// <inheritdoc />
public sealed class FrontMatterXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<FrontMatterMdSyntaxNode> {
    private const string Language = nameof(FrontMatterMdSyntaxNode.Language);
    private const string DashesCount = nameof(FrontMatterMdSyntaxNode.DashesCount);
    private const string LeadingSpaces = nameof(FrontMatterMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(FrontMatterMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        writer.WriteAttributeString(Language, node.Language);
        writer.WriteAttributeString(DashesCount, node.DashesCount.ToString());
        writer.WriteAttributeString(LeadingSpaces, node.LeadingSpaces.ToString());
        WriteElementContent(writer, node.Content);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, FrontMatterMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, Language, out string? language)) {
            targetNode.WithLanguage(language);      
        }

        if (TryGetAttributeAsInt32(reader, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);     
        }
        
        if (TryGetAttributeAsInt32(reader, DashesCount, out int dashesCount)) {
            targetNode.WithDashesCount(dashesCount);      
        }
    }

    /// <inheritdoc />
    protected override void SerializeContent(FrontMatterMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


