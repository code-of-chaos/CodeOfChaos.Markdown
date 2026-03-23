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
public sealed class HeadingSimpleXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HeadingSimpleMdSyntaxNode> {
    private const string Identifier = nameof(HeadingSimpleMdSyntaxNode.Identifier);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(HeadingSimpleMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(Identifier, node.Identifier);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, HeadingSimpleMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }
}


