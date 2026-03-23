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
public sealed partial class LinkMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<LinkMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        \[(?<text> (?:\ *!?\[.+?\]\(.+?\)\ *)|(?:[^\\\]]|\\\]|\\[^\]])*?)\]
        \(
          (?<href>\ *https?[^\ |]+)
          (?:\ ?\"(?<title>.+)\")?
          (?<mods>\|.*)?
        \)
        """, DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['['];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int LnTextId = RegexRule.GroupNumberFromName("text");
    private static readonly int LnHrefId = RegexRule.GroupNumberFromName("href");
    private static readonly int LnTitleId = RegexRule.GroupNumberFromName("title");
    private static readonly int LnModsId = RegexRule.GroupNumberFromName("mods");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string linkText = match.Groups[LnTextId].Value;
        string linkHref = match.Groups[LnHrefId].Value;
        string mods = match.Groups[LnModsId].Value;
        string title = match.Groups[LnTitleId].Value;

        LinkMdSyntaxNode linkNode = MdSyntaxNodePool<LinkMdSyntaxNode>.Shared.Get();
        linkNode.WithHref(linkHref);
        if (mods.IsNotNullOrWhiteSpace()) linkNode.WithModifier(MdSyntaxNodeModifier.FromString(mods));
        if (title.IsNotNullOrEmpty()) linkNode.WithTitle(title);

        parentNode.AddChildNode(linkNode);
        stack.PushSingleLineMatchesToStack(linkText, linkNode);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, LinkMdSyntaxNode node) {
        queue.Enqueue('[');
        queue.EnqueueChildren(node);
        queue.Enqueue(']');
        queue.Enqueue('(');
        queue.Enqueue(node.Href);
        if (node.Title.IsNotNullOrEmpty()) {
            queue.Enqueue(' ');
            queue.Enqueue('"');
            queue.Enqueue(node.Title);
            queue.Enqueue('"');
        }
        if (node.Modifier is { OriginalInputSpan: var inputSpan }) queue.Enqueue(inputSpan);
        queue.Enqueue(')');
    }
}
