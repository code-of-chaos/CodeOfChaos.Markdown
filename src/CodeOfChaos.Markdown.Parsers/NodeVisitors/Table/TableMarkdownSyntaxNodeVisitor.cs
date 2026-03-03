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
public sealed partial class TableMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TableMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ^\|(?<head>.+)\|[\ ]*\n
        ^\|(?<sep>[:\-|\ ]+?)\|[\ ]*
        (?<body>(?:\n(?:^\|.*\|$))+)
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private const int StackAllocThreshold = 16;

    private static readonly char[] STriggerCharacters = ['|'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int HeadId = RegexRule.GroupNumberFromName("head");
    private static readonly int SepId = RegexRule.GroupNumberFromName("sep");
    private static readonly int BodyId = RegexRule.GroupNumberFromName("body");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(
        INodeSerializerFragmentStack stack,
        IMdSyntaxNode parentNode,
        Match match
    ) {
        // Extract header, separator, and rows
        ReadOnlySpan<char> header = match.Groups[HeadId].ValueSpan;
        Span<Range> headerColumns = stackalloc Range[header.Length];
        int headerColumnCount = header.Split(headerColumns, '|', StringSplitOptions.TrimEntries);

        ReadOnlySpan<char> separator = match.Groups[SepId].ValueSpan;
        Span<Range> separatorColumns = stackalloc Range[separator.Length - 1];
        int separatorColumnCount = separator.Split(separatorColumns, '|', StringSplitOptions.TrimEntries);
        Span<TableMdSyntaxNode.Alignment> separatorColumData = stackalloc TableMdSyntaxNode.Alignment[separatorColumnCount];
        bool hasSeparatorData = ParseSeparatorData(separator, separatorColumns[..separatorColumnCount], separatorColumData);

        ReadOnlySpan<char> rows = match.Groups[BodyId].ValueSpan;
        Span<Range> rowRanges = stackalloc Range[rows.Length];
        int rowCount = rows.Split(rowRanges, '\n', StringSplitOptions.TrimEntries);

        // Construct table
        TableMdSyntaxNode tableNode = MdSyntaxNodePool<TableMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(tableNode);
        if (hasSeparatorData) tableNode.WithAlignments(separatorColumData);

        // Add headers
        TableRowMdSyntaxNode tableHeadRow = MdSyntaxNodePool<TableRowMdSyntaxNode>.Shared.Get();
        tableNode.TrySetHeader(tableHeadRow);

        for (int index = 0; index < headerColumnCount; index++) {
            TableCellMdSyntaxNode tableHeadCellNode = MdSyntaxNodePool<TableCellMdSyntaxNode>.Shared.Get();
            tableHeadRow.AddChildNode(tableHeadCellNode);

            ReadOnlySpan<char> column = header[headerColumns[index]];
            stack.PushSingleLineMatchesToStack(column.ToString(), tableHeadCellNode);
        }

        // Add rows
        Range[]? rowColumnRanges = null;
        Span<Range> columnBuffer = headerColumnCount <= StackAllocThreshold
            ? stackalloc Range[StackAllocThreshold]
            : rowColumnRanges = ArrayPool<Range>.Shared.Rent(headerColumnCount);

        try {
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++) {
                ReadOnlySpan<char> row = rows[rowRanges[rowIndex]].Trim();
                if (row.IsEmpty) continue;

                int rowColumnCount = row.Split(columnBuffer, '|', StringSplitOptions.RemoveEmptyEntries);

                TableRowMdSyntaxNode tableRow = MdSyntaxNodePool<TableRowMdSyntaxNode>.Shared.Get();
                tableNode.TryAddRow(tableRow);

                for (int columnIndex = 0; columnIndex < rowColumnCount; columnIndex++) {
                    TableCellMdSyntaxNode tableCell = MdSyntaxNodePool<TableCellMdSyntaxNode>.Shared.Get();
                    tableRow.AddChildNode(tableCell);

                    ReadOnlySpan<char> column = row[columnBuffer[columnIndex]];
                    if (column.IsEmpty || column.IsWhiteSpace()) continue;

                    stack.PushSingleLineMatchesToStack(column.ToString(), tableCell);
                }
            }
        }
        finally {
            if (rowColumnRanges != null) ArrayPool<Range>.Shared.Return(rowColumnRanges);
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TableMdSyntaxNode node) {
        ReadOnlySpan<TableCellMdSyntaxNode> headerCells = node.GetHeaderCells();
        ReadOnlySpan<TableRowMdSyntaxNode> rows = node.GetRows();
        int totalColumns = headerCells.Length;
        int totalRows = rows.Length;

        // Use jagged array instead of multi-dimensional to avoid LOH/Gen2
        string[][] tableGrid = new string[totalRows + 1][];

        // Process header cells (row 0)
        tableGrid[0] = new string[totalColumns];
        for (int col = 0; col < totalColumns; col++) {
            TableCellMdSyntaxNode cell = headerCells[col];
            tableGrid[0][col] = queue.ProcessAsStandaloneContent(cell);
        }

        // Process data rows (rows 1 and up)
        for (int row = 0; row < totalRows; row++) {
            tableGrid[row + 1] = new string[totalColumns];
            ReadOnlySpan<IMdSyntaxNode> cells = rows[row].GetChildrenSpan();
            for (int col = 0; col < Math.Min(cells.Length, totalColumns); col++) {
                IMdSyntaxNode cell = cells[col];
                tableGrid[row + 1][col] = queue.ProcessAsStandaloneContent(cell);
            }
        }

        // Calculate the max width for each column
        for (int col = 0; col < totalColumns; col++) {
            int maxCellWidth = 0;
            for (int row = 0; row <= totalRows; row++) {
                maxCellWidth = Math.Max(maxCellWidth, tableGrid[row][col].Trim().Length);
            }

            // Pad all cells in this column
            for (int row = 0; row <= totalRows; row++) {
                tableGrid[row][col] = tableGrid[row][col].Trim().PadRight(maxCellWidth);
            }
        }

        // Write header row
        for (int col = 0; col < totalColumns; col++) {
            queue.Enqueue('|');
            queue.Enqueue(' ');
            queue.Enqueue(tableGrid[0][col]);
            queue.Enqueue(' ');
        }

        queue.Enqueue('|');
        queue.Enqueue('\n');

        // Write header separator
        for (int col = 0; col < totalColumns; col++) {
            TableMdSyntaxNode.Alignment alignment = node.HasAlignments
                ? node.Alignments[col]
                : TableMdSyntaxNode.Alignment.Unknown;
            (char left, char right) = alignment switch {
                TableMdSyntaxNode.Alignment.Left => (':', '-'),
                TableMdSyntaxNode.Alignment.Right => ('-', ':'),
                TableMdSyntaxNode.Alignment.Center => (':', ':'),
                _ => ('-', '-')
            };

            queue.Enqueue('|');
            queue.Enqueue(' ');
            queue.Enqueue(left);
            queue.Enqueue('-', Math.Max(tableGrid[0][col].Length - 2, 1));
            queue.Enqueue(right);
            queue.Enqueue(' ');
        }

        queue.Enqueue('|');

        // Write data rows
        for (int row = 1; row <= totalRows; row++) {
            queue.Enqueue('\n');
            for (int col = 0; col < totalColumns; col++) {
                queue.Enqueue('|');
                queue.Enqueue(' ');
                queue.Enqueue(tableGrid[row][col]);
                queue.Enqueue(' ');
            }

            queue.Enqueue('|');
        }

        if (node.TryGetNextSibling(out IMdSyntaxNode? syntaxNode) && syntaxNode.Type != typeof(NewLineMdSyntaxNode)) {
            queue.Enqueue('\n');
        }
    }

    /// <summary>
    /// Parses separator data from a line of input and determines column alignments.
    /// </summary>
    private static bool ParseSeparatorData(ReadOnlySpan<char> lineInput, Span<Range> columnRanges, Span<TableMdSyntaxNode.Alignment> target) {
        if (lineInput.IsEmpty) return false;
        if (columnRanges.IsEmpty) return false;

        bool hasSeparatorData = false;

        for (int index = 0; index < columnRanges.Length; index++) {
            Range range = columnRanges[index];
            var alignment = TableMdSyntaxNode.Alignment.Unknown;
            ReadOnlySpan<char> columnSlice = lineInput[range].Trim();
            alignment = (columnSlice[0], columnSlice[^1]) switch {
                (':', ':') => TableMdSyntaxNode.Alignment.Center,
                ('-', ':') => TableMdSyntaxNode.Alignment.Right,
                (':', '-') => TableMdSyntaxNode.Alignment.Left,

                _ => alignment
            };

            if (alignment is not TableMdSyntaxNode.Alignment.Unknown) hasSeparatorData = true;
            target[index] = alignment;
        }

        return hasSeparatorData;
    }
}
