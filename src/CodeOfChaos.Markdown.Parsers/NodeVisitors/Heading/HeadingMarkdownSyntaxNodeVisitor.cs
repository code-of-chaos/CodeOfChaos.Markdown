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
public sealed partial class HeadingMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<HeadingMdSyntaxNode> {
    [GeneratedRegex(@"\G^(?<level>\#{1,6})[\ ]+(?<text>[^\n]+)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;
    
    private static readonly char[] STriggerCharacters = ['#'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;
    
    private static readonly int HLevelId = RegexRule.GroupNumberFromName("level");
    private static readonly int HTextId = RegexRule.GroupNumberFromName("text");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string headerText = match.Groups[HTextId].Value;
        int headingLevel = match.Groups[HLevelId].Length;

        HeadingMdSyntaxNode headingNode = MdSyntaxNodePool<HeadingMdSyntaxNode>.Shared.Get();
        headingNode.WithLevel(headingLevel);
        parentNode.AddChildNode(headingNode);

        stack.PushSingleLineMatchesToStack(headerText, headingNode);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, HeadingMdSyntaxNode node) {
        queue.Enqueue('#', node.Level);
        queue.Enqueue(' ');

        queue.EnqueueChildren(node);
    }
}
