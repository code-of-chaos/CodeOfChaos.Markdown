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
public sealed partial class FootnoteDescriptionMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<FootnoteDescriptionMdSyntaxNode> {
    [GeneratedRegex(@"\G^\[\^(?<id>[\d\p{L}\p{N}]+)\][\ ]?:[\ ]?(?<body>.+(?:\n(?!\[)(?:.+))*)", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['['];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int FootnoteIdentifierId = RegexRule.GroupNumberFromName("id");
    private static readonly int FootnoteBodyId = RegexRule.GroupNumberFromName("body");
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string footnoteId = match.Groups[FootnoteIdentifierId].Value;
        string body = match.Groups[FootnoteBodyId].Value;

        FootnoteDescriptionMdSyntaxNode node = MdSyntaxNodePool<FootnoteDescriptionMdSyntaxNode>.Shared.Get();
        node.WithIdentifier(footnoteId);
        parentNode.AddChildNode(node);

        stack.PushMultiLineMatchesToStack(body, node);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, FootnoteDescriptionMdSyntaxNode node) {
        queue.Enqueue('[');
        queue.Enqueue('^');
        queue.Enqueue(node.Identifier);
        queue.Enqueue(']');
        queue.Enqueue(' ');
        queue.Enqueue(':');
        queue.Enqueue(' ');
        queue.EnqueueChildren(node);
    }
}
