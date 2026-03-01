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
public sealed partial class SuperScriptMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<SuperScriptMdSyntaxNode> {
    [GeneratedRegex(@"\G\^(?<sp>(?>[^\\\^\n]+|\\\^|\^\^|(?<open>\^)|(?<-open>\^))+)(?(open)(?!))\^", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['^'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int SpId = RegexRule.GroupNumberFromName("sp");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string superValue = match.Groups[SpId].Value;

        SuperScriptMdSyntaxNode node = MdSyntaxNodePool<SuperScriptMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(superValue, node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, SuperScriptMdSyntaxNode node) {
        queue.Enqueue('^');
        queue.EnqueueChildren(node);
        queue.Enqueue('^');
    }
}
