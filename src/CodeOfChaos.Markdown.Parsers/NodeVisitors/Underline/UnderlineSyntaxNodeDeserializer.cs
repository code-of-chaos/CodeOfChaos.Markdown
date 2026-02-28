// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class UnderlineSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<UnderlineMdSyntaxNode> {
    protected override void Deserialize(UnderlineMdSyntaxNode node, StringBuilder builder) {
        builder.Append('_');
        DeserializeChildren(node, builder);
        builder.Append('_');
    }
}
