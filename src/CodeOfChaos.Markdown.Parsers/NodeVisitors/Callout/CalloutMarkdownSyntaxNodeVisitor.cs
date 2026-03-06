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
public sealed partial class CalloutMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<CalloutMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ^>(?:\[!(?<type>[^\|\n]+)(?<mod>\|[^\n]*)?\](?<option>\+|\-)?)[\ ]*(?<title>[^\n]*)$
        (?:\n(?<body>>[^\n]*(?:\n>[^\n]*)*)$)?
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['>'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int CalloutTypeId = RegexRule.GroupNumberFromName("type");
    private static readonly int CalloutModId = RegexRule.GroupNumberFromName("mod");
    private static readonly int CalloutOptionId = RegexRule.GroupNumberFromName("option");
    private static readonly int CalloutTitleId = RegexRule.GroupNumberFromName("title");
    private static readonly int CalloutBodyId = RegexRule.GroupNumberFromName("body");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        CalloutMdSyntaxNode node = MdSyntaxNodePool<CalloutMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);

        if (match.Groups[CalloutOptionId] is { Success: true, ValueSpan: { Length: > 0 } option }) {
            node.WithExpandOption(option);
        }

        if (match.Groups[CalloutTypeId] is { Success: true, Value: {} typeName }) {
            node.WithCalloutType(typeName);
        }

        if (match.Groups[CalloutModId] is { Success: true, Value: {} mods }) {
            node.WithModifier(MdSyntaxNodeModifier.FromString(mods));
        }

        if (match.Groups[CalloutTitleId] is { Success: true, Value: {} title }) {
            CalloutTitleMdSyntaxNode titleNode = MdSyntaxNodePool<CalloutTitleMdSyntaxNode>.Shared.Get();
            node.TrySetTitle(titleNode);

            stack.PushSingleLineMatchesToStack(title, titleNode);
        }

        // ReSharper disable once InvertIf
        if (match.Groups[CalloutBodyId] is { Success: true, ValueSpan: var calloutBody }) {
            CalloutBodyMdSyntaxNode bodyNode = MdSyntaxNodePool<CalloutBodyMdSyntaxNode>.Shared.Get();
            node.TrySetBody(bodyNode);

            stack.PushMultiLineMatchesToStack(
                LineNormalization.NormalizeBlockQuote(calloutBody, out _),
                bodyNode
            );
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, CalloutMdSyntaxNode node) {
        queue.Enqueue(">[!");
        queue.Enqueue(node.CalloutType);
        if (node.Modifier is { OriginalInputSpan: var inputSpan }) {
            queue.Enqueue(inputSpan);
        }

        queue.Enqueue(']');

        // Add a collapsed state when present
        string collapsedState = node.CollapsedState switch {
            CalloutCollapseStateOptions.Closed => "-",
            CalloutCollapseStateOptions.Open => "+",
            CalloutCollapseStateOptions.None => string.Empty,
            _ => throw new ArgumentOutOfRangeException(nameof(node), node.CollapsedState, null)
        };
        queue.Enqueue(collapsedState);

        // Title does not contain any multiline structure, so we can deserialize it directly
        if (node.TryGetTitleNode(out CalloutTitleMdSyntaxNode? titleNode)) {
            queue.Enqueue(' ');
            queue.EnqueueChildren(titleNode);
        }

        // Body contains a multiline structure, so we need to deserialize it separately
        if (!node.TryGetBodyNode(out CalloutBodyMdSyntaxNode? bodyNode)) return;

        ReadOnlySpan<IMdSyntaxNode> span = bodyNode.GetChildrenSpan();
        if (span.Length == 0) return;

        // Process content line by line without creating an array
        string content = queue.ProcessAsStandaloneContent(bodyNode);
        ReadOnlySpan<char> contentValue = content.AsSpan();
        int lineStart = 0;
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(node.LeadingSpaces, static i => new string(' ', i));

        for (int i = 0; i <= contentValue.Length; i++) {
            if (i != contentValue.Length && contentValue[i] != '\n') continue;

            ReadOnlySpan<char> line = contentValue.Slice(lineStart, i - lineStart);
            queue.Enqueue('\n');

            queue.Enqueue('>');
            queue.Enqueue(leadingSpaces);
            if (leadingSpaces.Length == 0) queue.Enqueue(' ');
            queue.Enqueue(line);

            // Move to the next line
            lineStart = i + 1;
        }
    }
}
