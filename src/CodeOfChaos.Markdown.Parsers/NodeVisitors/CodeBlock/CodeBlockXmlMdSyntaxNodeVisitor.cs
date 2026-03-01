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
public sealed class CodeBlockXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CodeBlockMdSyntaxNode> {
    private const string Language = nameof(CodeBlockMdSyntaxNode.Language);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CodeBlockMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        AddXmlPreserveSpace(targetElement);
        
        targetElement.SetAttributeValue(Language, node.Language);
        targetElement.Value = node.Content;
    }

    protected override void SerializeDetails(XElement element, CodeBlockMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);
        
        if (TryGetPropertyAsString(element, Language, out string? language)) {
            targetNode.WithLanguage(language);
        }
        
        targetNode.WithContent(element.Value);
    }
}
