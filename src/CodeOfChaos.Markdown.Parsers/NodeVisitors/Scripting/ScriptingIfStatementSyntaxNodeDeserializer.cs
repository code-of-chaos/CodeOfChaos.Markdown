// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ScriptingIfStatementSyntaxNodeDeserializer : MdStringMdSyntaxNodeDeserializerBase<ScriptingIfStatementSyntaxNode> {
    protected override void Deserialize(ScriptingIfStatementSyntaxNode node, StringBuilder builder) {
        DeserializeChildren(node, builder);
        builder.AppendLine("@endif");
    }
}
