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
public sealed partial class HeadingSimpleMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<HeadingSimpleMdSyntaxNode> {
    [GeneratedRegex(@"\G^(?<text>.+?)\n(?<id>[\ ]*(?:={3,}?|-{3,}?)[\ ]*$)", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int HsTextId = RegexRule.GroupNumberFromName("text");
    private static readonly int HsIdentifierId = RegexRule.GroupNumberFromName("id");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string headerSimpleText = match.Groups[HsTextId].Value;
        string headerIdentifierText = match.Groups[HsIdentifierId].Value;

        HeadingSimpleMdSyntaxNode headingNode = MdSyntaxNodePool<HeadingSimpleMdSyntaxNode>.Shared.Get();
        headingNode.WithIdentifier(headerIdentifierText);

        parentNode.AddChildNode(headingNode);

        stack.PushSingleLineMatchesToStack(headerSimpleText, headingNode);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, HeadingSimpleMdSyntaxNode node) {
        queue.EnqueueChildren(node);
        queue.Enqueue('\n');
        queue.Enqueue(node.Identifier);
    }
}
