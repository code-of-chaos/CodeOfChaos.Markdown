// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class ScriptingExpressionMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ScriptingExpressionSyntaxNode> {
    // ScriptingExpression nodes don't have a regex pattern - they're created programmatically
    protected override Regex Syntax => null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        // ScriptingExpression nodes are not serialized from regex matches
        throw new NotSupportedException("ScriptingExpression nodes are created programmatically, not from regex matches.");
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ScriptingExpressionSyntaxNode node) {
        queue.Enqueue(node.FullStatement);
        queue.Enqueue('\n');
        queue.EnqueueChildren(node);
    }
}
