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
public sealed class HorizontalRuleXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<HorizontalRuleMdSyntaxNode> {
    private const string Identifier = nameof(HorizontalRuleMdSyntaxNode.Identifier);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(HorizontalRuleMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(Identifier, node.Identifier);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, HorizontalRuleMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, Identifier, out string? identifier)) {
            targetNode.WithIdentifier(identifier);
        }
    }
}


