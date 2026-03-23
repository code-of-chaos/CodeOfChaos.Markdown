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
public sealed class UserXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<UserMdSyntaxNode> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(UserMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        WriteElementContent(writer, node.Content);
    }

    /// <inheritdoc />
    protected override void SerializeContent(UserMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


