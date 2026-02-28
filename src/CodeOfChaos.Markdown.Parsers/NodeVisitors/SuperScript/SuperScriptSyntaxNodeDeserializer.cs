// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class SuperScriptSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<SuperScriptMdSyntaxNode> {
    protected override void Deserialize(SuperScriptMdSyntaxNode node, StringBuilder builder) {
        builder.Append('^');
        DeserializeChildren(node, builder);
        builder.Append('^');
    }
}
