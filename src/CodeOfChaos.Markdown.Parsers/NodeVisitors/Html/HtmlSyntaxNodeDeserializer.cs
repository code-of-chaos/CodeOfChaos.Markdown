// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class HtmlSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<HtmlMdSyntaxNode> {
    protected override void Deserialize(HtmlMdSyntaxNode node, StringBuilder builder) {
        builder.Append(node.Content);
    }
}
