// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class FootnoteReferenceSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<FootnoteReferenceMdSyntaxNode> {
    protected override void Deserialize(FootnoteReferenceMdSyntaxNode node, StringBuilder builder) {
        builder.Append('[');
        builder.Append('^');
        builder.Append(node.Identifier);
        builder.Append(']');
    }
}
