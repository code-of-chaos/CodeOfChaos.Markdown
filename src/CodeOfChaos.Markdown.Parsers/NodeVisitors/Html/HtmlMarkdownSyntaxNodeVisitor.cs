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
public sealed partial class HtmlMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<HtmlMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        (?<pre>.+?)?
        (?<body>
            <(?<tag>\w+)\b[^>]*>
            (?>
                [^<]+
                | <(?<open>\k<tag>)\b[^>]*>
                | </(?<-open>\k<tag>)>
                | <(?!/?\k<tag>\b)[^>]+>
            )*
            (?(open)(?!))
            (</\k<tag>>)
        )
        (?<post>.+)?
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int HtmlPreId = RegexRule.GroupNumberFromName("pre");
    private static readonly int HtmlBodyId = RegexRule.GroupNumberFromName("body");
    private static readonly int HtmlPostId = RegexRule.GroupNumberFromName("post");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        // Only add a paragraph wrapper if there's trailing content (pre or post)
        bool hasTrailingContent = match.Groups[HtmlPreId].Success || match.Groups[HtmlPostId].Success;

        if (hasTrailingContent && parentNode is not (ParagraphMdSyntaxNode or HtmlSpanMdSyntaxNode)) {
            parentNode = parentNode.AddChildNode(MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get());
        }

        if (match.Groups[HtmlPostId].TryGetValue(out string? post)) {
            stack.PushSingleLineMatchesToStack(post, parentNode);
        }

        if (match.Groups[HtmlBodyId].TryGetValue(out string? htmlBody)) {
            HtmlMdSyntaxNode htmlNode = MdSyntaxNodePool<HtmlMdSyntaxNode>.Shared.Get();
            htmlNode.WithContent(htmlBody);
            stack.PushProcessedNodeToStack(parentNode, htmlNode);
        }

        if (match.Groups[HtmlPreId].TryGetValue(out string? pre)) {
            stack.PushSingleLineMatchesToStack(pre, parentNode);
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, HtmlMdSyntaxNode node) {
        queue.Enqueue(node.Content);
    }
}
