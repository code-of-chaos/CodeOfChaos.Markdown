// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ScriptingExpressionMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ScriptingExpressionMdSyntaxNode> {
    // ScriptingExpression nodes don't have a regex pattern - they're created programmatically
    protected override Regex Syntax => throw new NotSupportedException("ScriptingExpression nodes are created programmatically, not from regex matches.");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // ScriptingExpression nodes are not serialized from regex matches
    public override bool TryGetSerializationMatch(string input, [NotNullWhen(true)] out Match? match, int startPosition = 0) {
        match = null;
        return false;
    }
    
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) 
        => throw new NotSupportedException("ScriptingExpression nodes are created programmatically, not from regex matches.");

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ScriptingExpressionMdSyntaxNode node) {
        queue.Enqueue(node.FullStatement);
        queue.Enqueue('\n');
        queue.EnqueueChildren(node);
    }
}
