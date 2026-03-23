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
public sealed partial class BlockQuoteMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<BlockQuoteMdSyntaxNode> {
    [GeneratedRegex(@"\G^>[\ ]*(?:.+(?:\n>[^\n]*)*)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['>'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> blockQuoteBody = match.ValueSpan;
        string adjustedBlockquote = LineNormalization.NormalizeBlockQuote(blockQuoteBody, out int leadingSpaces);

        BlockQuoteMdSyntaxNode blockQuoteNode = MdSyntaxNodePool<BlockQuoteMdSyntaxNode>.Shared.Get();
        blockQuoteNode.WithLeadingSpaces(leadingSpaces);

        parentNode.AddChildNode(blockQuoteNode);
        stack.PushMultiLineMatchesToStack(adjustedBlockquote, blockQuoteNode);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, BlockQuoteMdSyntaxNode node) {
        if (node.ChildCount == 0) {
            queue.Enqueue('>');
            queue.Enqueue(' ');
            return;
        }

        // Process content line by line without creating an array
        string content = queue.ProcessAsStandaloneContent(node);
        ReadOnlySpan<char> contentValue = content.AsSpan();
        int lineStart = 0;
        bool isFirstLine = true;
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(node.LeadingSpaces, 0), static i => new string(' ', i));

        for (int i = 0; i <= contentValue.Length; i++) {
            if (i != contentValue.Length && contentValue[i] != '\n') continue;

            ReadOnlySpan<char> line = contentValue.Slice(lineStart, i - lineStart);
            if (!isFirstLine) queue.Enqueue('\n');
            queue.Enqueue('>');
            queue.Enqueue(leadingSpaces);

            if (line.IsEmpty) queue.Enqueue(' ');
            else queue.Enqueue(line);

            // Move to the next line
            lineStart = i + 1;
            isFirstLine = false;
        }
    }
}
