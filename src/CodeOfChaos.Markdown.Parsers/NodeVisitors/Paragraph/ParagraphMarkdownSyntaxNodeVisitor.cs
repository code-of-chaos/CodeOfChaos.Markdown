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
public sealed partial class ParagraphMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ParagraphMdSyntaxNode> {

    [GeneratedRegex(@"\G^(?<p>.+?)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int PId = RegexRule.GroupNumberFromName("p");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> paragraph = match.Groups[PId].ValueSpan;

        if (parentNode is HtmlSpanMdSyntaxNode) {
            stack.PushSingleLineMatchesToStack(paragraph.ToString(), parentNode);
            return;
        }

        ParagraphMdSyntaxNode node = MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get();
        parentNode = parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(paragraph.ToString(), parentNode);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ParagraphMdSyntaxNode node) {
        queue.EnqueueChildren(node);
    }
}
