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
        (?<body>
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
            (</span>)
        )
        (?<post>.+)?
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;
    
    private static readonly int HtmlPreId = RegexRule.GroupNumberFromName("pre");
    private static readonly int HtmlBodyId = RegexRule.GroupNumberFromName("body");
    private static readonly int HtmlPostId = RegexRule.GroupNumberFromName("post");
    private static readonly int SpanTagAttrsId = RegexRule.GroupNumberFromName("attr");
    private static readonly int SpanBodyId = RegexRule.GroupNumberFromName("body");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {// Only add a paragraph wrapper if there's trailing content (pre or post)
        bool hasTrailingContent = match.Groups[HtmlPreId].Success || match.Groups[HtmlPostId].Success;

        Match? spanMatch = null;
        bool hasHtmlBody = match.Groups[HtmlBodyId].TryGetValue(out string? htmlBody);
        string? spanBody = null;
        if (hasHtmlBody && htmlBody is not null) {
            spanMatch = SpanRegexRule.Match(htmlBody);
            if (spanMatch.Groups[SpanBodyId].TryGetValue(out spanBody)) {
                hasTrailingContent = true;
            }
        }

        if (hasTrailingContent && parentNode is not (ParagraphMdSyntaxNode or HtmlSpanMdSyntaxNode)) {
            parentNode = parentNode.AddChildNode(MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get());
        }

        if (match.Groups[HtmlPostId].TryGetValue(out string? post)) {
            stack.PushSingleLineMatchesToStack(post, parentNode);
        }

        if (hasHtmlBody && htmlBody is not null) {
            // Span should be the only special case allowed that allows for Markdown parsing within it
            if (spanMatch is not null && spanBody is not null) {
                HtmlSpanMdSyntaxNode spanNode = MdSyntaxNodePool<HtmlSpanMdSyntaxNode>.Shared.Get();

                string spanTagAttrs = spanMatch.Groups[SpanTagAttrsId].Value;
                spanNode.WithAttributes(spanTagAttrs);

                stack.PushMultiLineMatchesToStack(spanBody, spanNode);
                stack.PushProcessedNodeToStack(parentNode, spanNode);
            }
            else {
                HtmlMdSyntaxNode htmlNode = MdSyntaxNodePool<HtmlMdSyntaxNode>.Shared.Get();
                htmlNode.WithContent(htmlBody);
                stack.PushProcessedNodeToStack(parentNode, htmlNode);
            }
        }

        if (match.Groups[HtmlPreId].TryGetValue(out string? pre)) {
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
