// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class UserSyntaxNodeDeserializer : BaseMarkdownNodeSerializer<UserMdSyntaxNode> {
    protected override void Deserialize(UserMdSyntaxNode node, StringBuilder builder) {
        builder.Append('@');
        builder.Append(node.Content);
    }
}
