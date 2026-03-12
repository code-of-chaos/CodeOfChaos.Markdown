// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Buffers;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed partial class CodeBlockMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<CodeBlockMdSyntaxNode> {
    [GeneratedRegex(@"\G^(?<open>`{3,})[\ ]*(?<lang>.*?)?\n(?<body>(?>[\s\S]|(?!\k<open>))*?)\k<open>(?<tail>[^\n]+)?$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['`'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int CBodyId = RegexRule.GroupNumberFromName("body");
    private static readonly int CLangId = RegexRule.GroupNumberFromName("lang");
    private static readonly int CTrailId = RegexRule.GroupNumberFromName("tail");
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> codeBlockBody = match.Groups[CBodyId].ValueSpan;
        CodeBlockMdSyntaxNode codeNode = MdSyntaxNodePool<CodeBlockMdSyntaxNode>.Shared.Get();

        string langNameValue = match.Groups[CLangId].Value;
        if (!langNameValue.IsEmpty()) codeNode.WithLanguage(langNameValue);

        string content = ProcessCodeBlockContent(ref codeBlockBody);
        codeNode.WithContent(content);
        parentNode.AddChildNode(codeNode);

        // Add trailing text as a paragraph node
        if (!match.Groups[CTrailId].TryGetValue(out string? trailing)) return;

        ParagraphMdSyntaxNode paragraphNode = MdSyntaxNodePool<ParagraphMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(paragraphNode);
        stack.PushSingleLineMatchesToStack(trailing, paragraphNode);
    }

    private static string ProcessCodeBlockContent(ref ReadOnlySpan<char> content) {
        if (!content.Contains('\r')) return content.ToString();

        const int stackAllocThreshold = 1024;

        if (content.Length <= stackAllocThreshold) {
            // Use stack allocation for small strings
            Span<char> result = stackalloc char[content.Length];
            int length = ProcessContent(content, result);
            return new string(result[..length]);
        }

        // Use array pool for larger strings
        char[] rentedArray = ArrayPool<char>.Shared.Rent(content.Length);
        try {
            Span<char> asSpan = rentedArray.AsSpan();
            int length = ProcessContent(content, asSpan);
            return new string(rentedArray.AsSpan(0, length));
        }
        finally {
            ArrayPool<char>.Shared.Return(rentedArray);
        }
    }

    private static int ProcessContent(
        ReadOnlySpan<char> content,
        Span<char> result
    ) {
        int destinationIndex = 0;

        for (int i = 0; i < content.Length; i++) {
            switch (content[i]) {
                case '\r' when i + 1 < content.Length && content[i + 1] == '\n': {
                    result[destinationIndex++] = '\n';
                    i++;// Skip the \n
                    break;
                }

                default:
                    result[destinationIndex++] = content[i];
                    break;
            }
        }

        return destinationIndex;
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, CodeBlockMdSyntaxNode node) {
        queue.Enqueue("```");
        queue.Enqueue(node.Language);
        queue.Enqueue('\n');
        queue.Enqueue(node.Content);
        queue.Enqueue("```");
    }
}
