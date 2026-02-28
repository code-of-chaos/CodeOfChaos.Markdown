// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class SubScriptSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<SubScriptMdSyntaxNode> {
    protected override void Deserialize(SubScriptMdSyntaxNode node, StringBuilder builder) {
        builder.Append('~');
        DeserializeChildren(node, builder);
        builder.Append('~');
    }
}
