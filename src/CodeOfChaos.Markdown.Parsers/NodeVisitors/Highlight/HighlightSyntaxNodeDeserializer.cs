// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class HighlightSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<HighlightMdSyntaxNode> {

    protected override void Deserialize(HighlightMdSyntaxNode node, StringBuilder builder) {
        builder.Append("==");
        DeserializeChildren(node, builder);
        builder.Append("==");
    }
}
