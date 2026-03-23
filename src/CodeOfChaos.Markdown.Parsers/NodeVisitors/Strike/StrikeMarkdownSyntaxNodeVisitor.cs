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
/// <inheritdoc />
public sealed partial class StrikeMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<StrikeMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ~~(?<s>
            (?:\\.
            | [^\\\n~]
            | ~(?!~) 
            | ~(?=~~)
            )+
        )~~
        """, DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['~'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int StrikeContentId = RegexRule.GroupNumberFromName("s");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string strikeValue = match.Groups[StrikeContentId].Value;

        StrikeMdSyntaxNode node = MdSyntaxNodePool<StrikeMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);

        stack.PushSingleLineMatchesToStack(strikeValue, node);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, StrikeMdSyntaxNode node) {
        queue.Enqueue('~');
        queue.Enqueue('~');
        queue.EnqueueChildren(node);
        queue.Enqueue('~');
        queue.Enqueue('~');
    }
}
