// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class TextSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<TextMdSyntaxNode> {
    protected override void Deserialize(TextMdSyntaxNode node, StringBuilder builder) {
        builder.Append(node.Content);
    }
}
