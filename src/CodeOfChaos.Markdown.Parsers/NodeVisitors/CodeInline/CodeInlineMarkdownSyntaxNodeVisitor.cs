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
public sealed partial class CodeInlineMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<CodeInlineMdSyntaxNode> {
    [GeneratedRegex(@"\G(?<open>`+)(?<c>(?>[^`\\]+|\\.|`(?!\k<open>))+?)\k<open>", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['`'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int CodeContentId = RegexRule.GroupNumberFromName("c");
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string codeValue = match.Groups[CodeContentId].Value;
        ReadOnlySpan<char> fullOriginalString = match.ValueSpan;

        CodeInlineMdSyntaxNode node = MdSyntaxNodePool<CodeInlineMdSyntaxNode>.Shared.Get();
        node.WithContent(codeValue);

        // Calculate backtick count by comparing full string length to content length
        int totalLength = fullOriginalString.Length;
        int contentLength = codeValue.Length;
        int totalBackticks = totalLength - contentLength;
        int backtickCount = totalBackticks / 2;// Backticks on one side

        node.WithBackTickCount(backtickCount);
        parentNode.AddChildNode(node);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, CodeInlineMdSyntaxNode node) {
        queue.Enqueue('`', Math.Max(node.BackTickCount, 1));
        queue.Enqueue(node.Content);
        queue.Enqueue('`', Math.Max(node.BackTickCount, 1));
    }
}
