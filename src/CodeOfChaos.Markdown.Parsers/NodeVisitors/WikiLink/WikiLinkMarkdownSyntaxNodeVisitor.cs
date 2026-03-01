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
public sealed partial class WikiLinkMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<WikiLinkMdSyntaxNode> {
    [GeneratedRegex(@"\G\[\[(?<href>[^\]\[\ ]+)\]\]", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int WikiLinkHrefId = RegexRule.GroupNumberFromName("href");

    private static readonly char[] STriggerCharacters = ['['];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string href = match.Groups[WikiLinkHrefId].Value;

        WikiLinkMdSyntaxNode node = MdSyntaxNodePool<WikiLinkMdSyntaxNode>.Shared.Get();
        node.WithContent(href);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, WikiLinkMdSyntaxNode node) {
        queue.Enqueue('[');
        queue.Enqueue('[');
        queue.Enqueue(node.Content);
        queue.Enqueue(']');
        queue.Enqueue(']');
    }
}
