// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class HeadingSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<HeadingMdSyntaxNode> {
    protected override void Deserialize(HeadingMdSyntaxNode node, StringBuilder builder) {
        builder.Append('#', node.Level);
        builder.Append(' ');

        DeserializeChildren(node, builder);
    }
}
