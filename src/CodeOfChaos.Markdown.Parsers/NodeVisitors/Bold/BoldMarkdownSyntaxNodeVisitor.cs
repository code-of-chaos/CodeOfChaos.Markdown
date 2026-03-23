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
public sealed partial class BoldMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<BoldMdSyntaxNode> {
    [GeneratedRegex(
        """
            \G\*\*(?<b>
                (?:\\.
                | [^\\\*\n]
                | \*(?!\*) 
                | \*(?=\*\*)
                )+
            )\*\*
        """, DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['*'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int BoldContentId = RegexRule.GroupNumberFromName("b");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string boldValue = match.Groups[BoldContentId].Value;

        BoldMdSyntaxNode node = MdSyntaxNodePool<BoldMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(boldValue, node);
    }
    
    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, BoldMdSyntaxNode node) {
        queue.Enqueue("**");
        queue.EnqueueChildren(node);
        queue.Enqueue("**");
    }
}
