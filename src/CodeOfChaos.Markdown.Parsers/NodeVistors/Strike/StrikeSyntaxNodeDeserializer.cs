// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class StrikeSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<StrikeMdSyntaxNode> {
    protected override void Deserialize(StrikeMdSyntaxNode node, StringBuilder builder) {
        builder.Append('~');
        builder.Append('~');
        DeserializeChildren(node, builder);
        builder.Append('~');
        builder.Append('~');
    }
}
