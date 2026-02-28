// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Pooling;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class BlockQuoteMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<BlockQuoteMdSyntaxNode> {
    [GeneratedRegex(@"\G^>[\ ]*(?:.+(?:\n>[^\n]*)*)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['>'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(IMdSyntaxFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> blockQuoteBody = match.ValueSpan;
        string adjustedBlockquote = LineNormalization.NormalizeBlockQuote(blockQuoteBody, out int leadingSpaces);

        BlockQuoteMdSyntaxNode blockQuoteNode = MdSyntaxNodePool<BlockQuoteMdSyntaxNode>.Shared.Get();
        blockQuoteNode.WithLeadingSpaces(leadingSpaces);

        parentNode.AddChildNode(blockQuoteNode);
        stack.PushMultiLineMatchesToStack(adjustedBlockquote, blockQuoteNode);
    }

    protected override void Deserialize(IMdStringDeserializerQueue _, BlockQuoteMdSyntaxNode node, StringBuilder builder) {
        StringBuilder contentBuilder = GlobalPools.StringBuilder.Get();
        MdStringDeserializerQueue queue = MdStringDeserializerQueuePool.Shared.Get();

        try {
            if (node.ChildCount == 0) {
                builder.Append('>');
                builder.Append(' ');
                return;
            }

            // First, deserialize all children to get the raw content
            queue.EnqueueChildren(node, contentBuilder);

            if (contentBuilder.Length == 0) return;

            // Process content line by line without creating an array
            ReadOnlySpan<char> content = contentBuilder.ToString().AsSpan();
            int lineStart = 0;
            bool isFirstLine = true;
            string leadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(node.LeadingSpaces, 0), static i => new string(' ', i));

            for (int i = 0; i <= content.Length; i++) {
                if (i != content.Length && content[i] != '\n') continue;

                ReadOnlySpan<char> line = content.Slice(lineStart, i - lineStart);
                if (!isFirstLine) builder.Append('\n');
                builder.Append('>');
                builder.Append(leadingSpaces);

                if (line.IsEmpty) builder.Append(' ');
                else builder.Append(line);

                // Move to the next line
                lineStart = i + 1;
                isFirstLine = false;
            }
        }
        finally {
            GlobalPools.StringBuilder.Return(contentBuilder);
            MdStringDeserializerQueuePool.Shared.Return(queue);
        }
    }
}
