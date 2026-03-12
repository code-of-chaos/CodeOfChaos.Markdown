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
public sealed class CodeInlineXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CodeInlineMdSyntaxNode> {
    private const string BackTickCount = nameof(CodeInlineMdSyntaxNode.BackTickCount);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(CodeInlineMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        writer.WriteAttributeString(BackTickCount, node.BackTickCount.ToString());
        WriteElementContent(writer, node.Content);
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, CodeInlineMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsInt32(reader, BackTickCount, out int backTickCount)) {
            targetNode.WithBackTickCount(backTickCount);
        }
    }

    /// <inheritdoc />
    protected override void SerializeContent(CodeInlineMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


