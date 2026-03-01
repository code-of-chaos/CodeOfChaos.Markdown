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
public sealed partial class ImageMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ImageMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        !
        \[(?<text> (?:\ *!?\[.+?\]\(.+?\)\ *)|(?:[^\\\]]|\\\]|\\[^\]])*?)\]
        \(
          (?<href>\ *https?[^\ |]+)
          (?:\ ?\"(?<title>.+)\")?
          (?<mods>\|.*)?
        \)
        """, DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['!'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int ImgTextId = RegexRule.GroupNumberFromName("text");
    private static readonly int ImgHrefId = RegexRule.GroupNumberFromName("href");
    private static readonly int ImgTitleId = RegexRule.GroupNumberFromName("title");
    private static readonly int ImgModsId = RegexRule.GroupNumberFromName("mods");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> altText = match.Groups[ImgTextId].ValueSpan;
        ReadOnlySpan<char> href = match.Groups[ImgHrefId].ValueSpan;
        ReadOnlySpan<char> mods = match.Groups[ImgModsId].ValueSpan;
        ReadOnlySpan<char> title = match.Groups[ImgTitleId].ValueSpan;

        ImageMdSyntaxNode imgNode = MdSyntaxNodePool<ImageMdSyntaxNode>.Shared.Get();
        imgNode.WithAltText(altText.ToString());
        imgNode.WithHref(href.ToString());

        if (!mods.IsWhiteSpace()) imgNode.WithModifier(MdSyntaxNodeModifier.FromString(mods.ToString()));
        if (!title.IsEmpty) imgNode.WithTitle(title.ToString());

        parentNode.AddChildNode(imgNode);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ImageMdSyntaxNode node) {
        queue.Enqueue('!');
        queue.Enqueue('[');
        queue.Enqueue(node.OriginalAltText);
        queue.Enqueue(']');
        queue.Enqueue('(');
        queue.Enqueue(node.Href);
        if (node.Modifier is { OriginalInputSpan: var inputSpan }) queue.Enqueue(inputSpan);
        queue.Enqueue(')');
    }
}
