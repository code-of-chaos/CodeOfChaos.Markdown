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
public sealed class BlockQuoteXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<BlockQuoteMdSyntaxNode> {
    private const string LeadingSpaces = nameof(BlockQuoteMdSyntaxNode.LeadingSpaces);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(BlockQuoteMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        
        writer.WriteAttributeString(LeadingSpaces, node.LeadingSpaces.ToString());
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, BlockQuoteMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);
        
        if (TryGetAttributeAsInt32(reader, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
    }
}


