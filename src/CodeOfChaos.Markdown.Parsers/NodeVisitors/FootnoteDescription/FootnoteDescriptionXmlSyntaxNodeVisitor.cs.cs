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
public sealed class FootnoteDescriptionXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<FootnoteDescriptionMdSyntaxNode> {
    private const string Identifier = nameof(FootnoteDescriptionMdSyntaxNode.Identifier);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(FootnoteDescriptionMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(Identifier, node.Identifier);
    }

    protected override void SerializeDetails(XmlReader reader, FootnoteDescriptionMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }
}


