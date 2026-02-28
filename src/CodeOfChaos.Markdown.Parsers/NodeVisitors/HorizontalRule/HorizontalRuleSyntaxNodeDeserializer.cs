// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class HorizontalRuleSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<HorizontalRuleMdSyntaxNode> {
    protected override void Deserialize(HorizontalRuleMdSyntaxNode node, StringBuilder builder) {
        builder.Append(node.Identifier);
    }
}
