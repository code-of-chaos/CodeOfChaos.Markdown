// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class EscapedCharacterSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<EscapedCharacterMdSyntaxNode> {

    protected override void Deserialize(EscapedCharacterMdSyntaxNode node, StringBuilder builder) {
        builder.Append('\\');
        builder.Append(node.Content);
    }
}
