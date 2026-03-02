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
public sealed class CodeInlineXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CodeInlineMdSyntaxNode> {
    private const string BackTickCount = nameof(CodeInlineMdSyntaxNode.BackTickCount);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CodeInlineMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        AddXmlPreserveSpace(targetElement);
        targetElement.SetAttributeValue(BackTickCount, node.BackTickCount);
        targetElement.Value = node.Content;
    }

    protected override void SerializeDetails(XElement element, CodeInlineMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetAttributeAsInt32(element, BackTickCount, out int backTickCount)) {
            targetNode.WithBackTickCount(backTickCount);
        }

        targetNode.WithContent(element.Value);
    }
}
