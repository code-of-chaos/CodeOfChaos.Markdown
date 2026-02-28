// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class NewLineSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<NewLineMdSyntaxNode> {

    protected override void Deserialize(NewLineMdSyntaxNode node, StringBuilder builder) {
        builder.Append('\n');
    }
}
