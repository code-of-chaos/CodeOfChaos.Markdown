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
public sealed class EscapedCharacterXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<EscapedCharacterMdSyntaxNode> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(EscapedCharacterMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        WriteElementContent(writer, node.Content.ToString());
    }

    /// <inheritdoc />
    protected override void SerializeContent(EscapedCharacterMdSyntaxNode targetNode, string content) {
        if (content.IsNotNullOrEmpty()) {
            targetNode.WithContent(content[0]);
        }
    }
}


