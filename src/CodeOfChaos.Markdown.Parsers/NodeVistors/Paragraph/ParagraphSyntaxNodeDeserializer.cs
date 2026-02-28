// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ParagraphSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<ParagraphMdSyntaxNode> {
    protected override void Deserialize(ParagraphMdSyntaxNode node, StringBuilder builder) {
        DeserializeChildren(node, builder);
    }
}
