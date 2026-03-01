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
public sealed partial class HtmlSpanMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<HtmlSpanMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        (?<pre>.+?)?
        <span\ ?(?<attr>\b[^>]*)>
        (?<body>
          (?>
            [^<]+
            | <(?<open>span)\b[^>]*>
            | </(?<-open>span)>
            | <(?!/?span\b)[^>]+>
          )*
        )
        (?(open)(?!))
        </span>
        (?<post>.+)?
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int SpanPreId = RegexRule.GroupNumberFromName("pre");
    private static readonly int SpanBodyId = RegexRule.GroupNumberFromName("body");
    private static readonly int SpanPostId = RegexRule.GroupNumberFromName("post");
    private static readonly int SpanAttrsId = RegexRule.GroupNumberFromName("attr");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        // Spans should always be wrapped in a paragraph if not already in one
        bool needsParagraph = parentNode is not (ParagraphMdSyntaxNode or HtmlSpanMdSyntaxNode);

        // Check if there's pre or post content that would also need the paragraph
        bool hasPreContent = match.Groups[SpanPreId].Success;
        bool hasPostContent = match.Groups[SpanPostId].Success;

        if (needsParagraph) {
            parentNode = parentNode.AddChildNode(MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get());
        }

        if (hasPostContent && match.Groups[SpanPostId].TryGetValue(out string? post)) {
            stack.PushSingleLineMatchesToStack(post, parentNode);
        }

        HtmlSpanMdSyntaxNode spanNode = MdSyntaxNodePool<HtmlSpanMdSyntaxNode>.Shared.Get();

        string spanAttrs = match.Groups[SpanAttrsId].Value;
        spanNode.WithAttributes(spanAttrs);

        if (match.Groups[SpanBodyId].TryGetValue(out string? spanBody)) {
            stack.PushMultiLineMatchesToStack(spanBody, spanNode);
        }

        stack.PushProcessedNodeToStack(parentNode, spanNode);

        if (hasPreContent && match.Groups[SpanPreId].TryGetValue(out string? pre)) {
            stack.PushSingleLineMatchesToStack(pre, parentNode);
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, HtmlSpanMdSyntaxNode node) {
        queue.Enqueue("<span");
        if (node.Attributes.IsNotNullOrEmpty()) {
            queue.Enqueue(' ');
            queue.Enqueue(node.Attributes);
        }
        queue.Enqueue('>');
        queue.EnqueueChildren(node);
        queue.Enqueue("</span>");
    }
}
