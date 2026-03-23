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
public sealed partial class ParagraphMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ParagraphMdSyntaxNode> {

    [GeneratedRegex(@"\G^(?<p>.+?)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int PId = RegexRule.GroupNumberFromName("p");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string paragraph = match.Groups[PId].Value;

        if (parentNode is HtmlSpanMdSyntaxNode) {
            stack.PushSingleLineMatchesToStack(paragraph, parentNode);
            return;
        }

        ParagraphMdSyntaxNode node = MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get();
        parentNode = parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(paragraph, parentNode);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ParagraphMdSyntaxNode node) {
        queue.EnqueueChildren(node);
    }
}
