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
public sealed partial class ListOrderedMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ListOrderedMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ^[^\S\n]*(?:\d+\.|\.\d+).+
        (?:\n(?:(?:\d+\.|\.\d+)|(?:[\ ]+)).+)*
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    [GeneratedRegex(@"^\ *(?<index>\d*)\.(?:(?<taskSpace>\ *)\[(?<task>[\ xX~])])?(?:(?<space>\ *)(?<head>.+)|(?<head>\ )|(?<head>))(?<body>(?:\n\ +.*)*)", DefaultMultiLineRegexOptions)]
    private static partial Regex ListItemBodyRegexRule { get; }

    private static readonly int LIndexId = ListItemBodyRegexRule.GroupNumberFromName("index");
    private static readonly int ListTaskItemLeadingSpaces = ListItemBodyRegexRule.GroupNumberFromName("taskSpace");
    private static readonly int LTaskId = ListItemBodyRegexRule.GroupNumberFromName("task");
    private static readonly int ListItemLeadingSpaces = ListItemBodyRegexRule.GroupNumberFromName("space");
    private static readonly int LHeadId = ListItemBodyRegexRule.GroupNumberFromName("head");
    private static readonly int LBodyId = ListItemBodyRegexRule.GroupNumberFromName("body");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(
        INodeSerializerFragmentStack stack,
        IMdSyntaxNode parentNode,
        Match match
    ) {
        string listBody = match.Value;

        MatchCollection matchCollection = ListItemBodyRegexRule.Matches(listBody);
        int matchCount = matchCollection.Count;
        Match[] matchArray = ArrayPool<Match>.Shared.Rent(matchCount);

        try {
            matchCollection.CopyTo(matchArray, 0);

            ListOrderedMdSyntaxNode listNode = MdSyntaxNodePool<ListOrderedMdSyntaxNode>.Shared.Get();
            parentNode.AddChildNode(listNode);

            for (int i = 0; i < matchCount; i++) {
                GroupCollection groups = matchArray[i].Groups;

                ListItemMdSyntaxNode listItemNode = MdSyntaxNodePool<ListItemMdSyntaxNode>.Shared.Get();
                listNode.AddChildNode(listItemNode);

                groups[ListItemLeadingSpaces].TryGetLength(out int listItemLeadingSpaces);
                listItemNode.WithLeadingSpaces(Math.Max(listItemLeadingSpaces, 0));

                groups[ListTaskItemLeadingSpaces].TryGetLength(out int listTaskItemLeadingSpaces);
                listItemNode.WithCheckLeadingSpaces(Math.Max(listTaskItemLeadingSpaces, 0));

                if (groups[LBodyId].TryGetValueSpan(out ReadOnlySpan<char> itemBody) && !itemBody.IsEmpty) {
                    string normalizedBody = LineNormalization.NormalizeLineIndentation(itemBody, out int leadingSpaces);
                    stack.PushMultiLineMatchesToStack(normalizedBody, listItemNode);
                    listNode.WithLeadingSpaces(leadingSpaces);
                }

                if (groups[LHeadId].TryGetValue(out string? listHeader)) {
                    stack.PushSingleLineMatchesToStack(listHeader, listItemNode);
                }

                if (groups[LIndexId].TryGetValue(out string? listIndex)) {
                    listItemNode.WithIndex(listIndex);
                }

                if (groups[LTaskId].TryGetValue(out string? taskMarker)) {
                    listItemNode.WithCheckMarker(taskMarker);
                }
            }
        }
        finally {
            ArrayPool<Match>.Shared.Return(matchArray);
        }
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ListOrderedMdSyntaxNode node) {
        bool isFirstItem = true;
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(node.LeadingSpaces, 0), static i => new string(' ', i));

        foreach (IMdSyntaxNode child in node.GetChildrenSpan()) {
            if (child is not ListItemMdSyntaxNode listItem) continue;

            if (!isFirstItem) queue.Enqueue('\n');
            isFirstItem = false;

            // Process item content
            string content = queue.ProcessAsStandaloneContent(listItem);
            ReadOnlySpan<char> contentValue = content.AsSpan();
            int lineStart = 0;
            bool isFirstLine = true;

            string itemLeadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(listItem.LeadingSpaces, 0), static i => new string(' ', i));
            string checkLeadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(listItem.CheckLeadingSpaces, 0), static i => new string(' ', i));

            for (int i = 0; i <= contentValue.Length; i++) {
                if (i != contentValue.Length && contentValue[i] != '\n') continue;

                ReadOnlySpan<char> line = contentValue.Slice(lineStart, i - lineStart);

                if (!isFirstLine) {
                    queue.Enqueue('\n');
                    queue.Enqueue(leadingSpaces);
                }
                else {
                    // First line - add list item prefix
                    queue.Enqueue(listItem.Index);
                    queue.Enqueue('.');

                    if (listItem.IsCheckable) {
                        queue.Enqueue(checkLeadingSpaces);
                        queue.Enqueue('[');
                        queue.Enqueue(listItem.OriginalCheckMarker);
                        queue.Enqueue(']');
                    }

                    queue.Enqueue(itemLeadingSpaces);
                }

                queue.Enqueue(line);

                lineStart = i + 1;
                isFirstLine = false;
            }
        }
    }
}
