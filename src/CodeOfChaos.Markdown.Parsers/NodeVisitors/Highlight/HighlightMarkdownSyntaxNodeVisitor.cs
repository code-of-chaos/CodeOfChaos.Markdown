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
public sealed partial class HighlightMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<HighlightMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ==(?<h>
            (?:\\.
            | [^\\\n=]
            | =(?!=) 
            | =(?===)
            )+
        )==
        """, DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['='];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int HId = RegexRule.GroupNumberFromName("h");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string highlightValue = match.Groups[HId].Value;

        HighlightMdSyntaxNode node = MdSyntaxNodePool<HighlightMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(highlightValue, node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, HighlightMdSyntaxNode node) {
        queue.Enqueue("==");
        queue.EnqueueChildren(node);
        queue.Enqueue("==");
    }
}
