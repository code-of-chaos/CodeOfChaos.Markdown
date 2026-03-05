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
public sealed class CodeBlockXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<CodeBlockMdSyntaxNode> {
    private const string Language = nameof(CodeBlockMdSyntaxNode.Language);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(CodeBlockMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        WriteXmlPreserveSpace(writer);
        
        writer.WriteAttributeString(Language, node.Language);
        WriteElementContent(writer, node.Content);
    }

    protected override void SerializeDetails(XmlReader reader, CodeBlockMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);
        
        if (TryGetAttributeAsString(reader, Language, out string? language)) {
            targetNode.WithLanguage(language);
        }
    }

    protected override void SerializeContent(CodeBlockMdSyntaxNode targetNode, string content) {
        targetNode.WithContent(content);
    }
}


