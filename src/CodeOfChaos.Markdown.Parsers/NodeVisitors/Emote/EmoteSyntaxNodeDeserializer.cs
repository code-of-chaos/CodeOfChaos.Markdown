// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class EmoteSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<EmoteMdSyntaxNode> {
    protected override void Deserialize(EmoteMdSyntaxNode node, StringBuilder builder) {
        builder.Append(node.OriginalEmote);
    }
}
