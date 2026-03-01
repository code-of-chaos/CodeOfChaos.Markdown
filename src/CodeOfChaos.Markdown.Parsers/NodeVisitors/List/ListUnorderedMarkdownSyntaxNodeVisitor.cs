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
public sealed partial class ListUnorderedMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ListUnOrderedMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ^[^\S\n]*-(?!-).+
        (?:\n(?:(?:-(?!-))|(?:[\ ]+)).+)*
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    [GeneratedRegex(@"^\ *-(?:(?<taskSpace>\ *)\[(?<task>[\ xX~])])?(?:(?<space>\ *)(?<head>.+)|(?<head>\ )|(?<head>))(?<body>(?:\n\ +.*)*)", DefaultMultiLineRegexOptions)]
    private static partial Regex ListItemBodyRegexRule { get; }

    private static readonly int ListTaskItemLeadingSpaces = ListItemBodyRegexRule.GroupNumberFromName("taskSpace");
    private static readonly int LTaskId = ListItemBodyRegexRule.GroupNumberFromName("task");
    private static readonly int ListItemLeadingSpaces = ListItemBodyRegexRule.GroupNumberFromName("space");
    private static readonly int LHeadId = ListItemBodyRegexRule.GroupNumberFromName("head");
    private static readonly int LBodyId = ListItemBodyRegexRule.GroupNumberFromName("body");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
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

            ListUnOrderedMdSyntaxNode listNode = MdSyntaxNodePool<ListUnOrderedMdSyntaxNode>.Shared.Get();
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

                if (groups[LTaskId].TryGetValue(out string? taskMarker)) {
                    listItemNode.WithCheckMarker(taskMarker);
                }
            }
        }
        finally {
            ArrayPool<Match>.Shared.Return(matchArray);
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ListUnOrderedMdSyntaxNode node) {
        foreach (IMdSyntaxNode child in node.GetChildrenSpan()) {
            if (child is not ListItemMdSyntaxNode listItem) continue;

            // Unordered list item prefix
            queue.Enqueue('-');

            if (listItem.IsCheckable) {
                queue.Enqueue(' ', listItem.CheckLeadingSpaces);
                queue.Enqueue('[');
                queue.Enqueue(listItem.OriginalCheckMarker);
                queue.Enqueue(']');
            }

            queue.Enqueue(' ', listItem.LeadingSpaces);

            // Enqueue children with indentation handling
            queue.EnqueueChildren(listItem, node.LeadingSpaces);

            queue.Enqueue('\n');
        }

        // Remove trailing newline
        queue.RemoveLast();
    }
}
