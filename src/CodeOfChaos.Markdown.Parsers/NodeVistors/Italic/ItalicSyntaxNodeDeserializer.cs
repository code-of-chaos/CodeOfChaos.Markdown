// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ItalicSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<ItalicMdSyntaxNode> {

    protected override void Deserialize(ItalicMdSyntaxNode node, StringBuilder builder) {
        builder.Append('*');
        DeserializeChildren(node, builder);
        builder.Append('*');
    }
}
