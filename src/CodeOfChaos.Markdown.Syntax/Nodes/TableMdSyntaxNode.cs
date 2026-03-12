// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class TableMdSyntaxNode : MdSyntaxNode<TableMdSyntaxNode> {
    /// <summary>
    /// Represents the alignments for columns in a markdown table.
    /// </summary>
    /// <remarks>
    /// The alignments are defined as an array of <see cref="TableAlignment"/> values, each corresponding
    /// to the alignment of a specific column in the table.
    /// Supported alignments are:
    /// - <see cref="TableAlignment.Left"/>: Aligns the content to the left.
    /// - <see cref="TableAlignment.Center"/>: Centers the content.
    /// - <see cref="TableAlignment.Right"/>: Aligns the content to the right.
    /// - <see cref="TableAlignment.Unknown"/>: Represents an undefined or unspecified alignment.
    /// By default, the property is initialized to an empty array. It can be updated using
    /// the <see cref="TableMdSyntaxNode.WithAlignments"/> method.
    /// </remarks>
    public TableAlignment[] Alignments { get; private set; } = Array.Empty<TableAlignment>();
    /// <summary>
    /// Indicates whether the table node has alignment information defined for its columns.
    /// </summary>
    /// <remarks>
    /// This property returns <c>true</c> if column alignments have been configured for the table;
    /// otherwise, it returns <c>false</c>. The alignment configuration can be set using the
    /// <see cref="WithAlignments"/> method and is reset when the table node is reset.
    /// </remarks>
    public bool HasAlignments { get; private set; }
    
    private const int HeaderIndex = 0;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Attempts to set the header row for the table by adding the specified
    /// header row at the designated index.
    /// <param name="headerRow">The header row to set as the table's header.</param>
    /// <returns>True if the header row was successfully added; otherwise, false.</returns>
    public bool TrySetHeader(TableRowMdSyntaxNode headerRow) 
        => TryAddChildNodeAtIndex(HeaderIndex, headerRow);

    /// Attempts to add a row to the table at the appropriate position.
    /// If the row cannot be added, the method returns false.
    /// <param name="row">The row to add to the table.</param>
    /// <return>
    /// True if the row was successfully added; otherwise, false.
    /// </return>
    public bool TryAddRow(TableRowMdSyntaxNode row)
        => TryAddChildNodeAtIndex(Math.Max(1, ChildCount), row); // Index 0 is reserved for header

    /// Retrieves a span of header cells from the child nodes of the table syntax node.
    /// <return>
    /// A read-only span of <see cref="TableCellMdSyntaxNode"/> representing the header cells of the table.
    /// </return>
    public ReadOnlySpan<TableCellMdSyntaxNode> GetHeaderCells() {
        var headerNode = Unsafe.As<TableRowMdSyntaxNode>(ChildNodes[HeaderIndex]);
        ReadOnlySpan<IMdSyntaxNode> childSpan = headerNode.GetChildrenSpan();
        return MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<IMdSyntaxNode, TableCellMdSyntaxNode>(ref MemoryMarshal.GetReference(childSpan)),
            childSpan.Length
        );
    }

    /// Retrieves a read-only span of all rows in the table excluding the header row.
    /// <return>
    /// A read-only span of TableRowMdSyntaxNode instances representing the rows of the table.
    /// </return>
    public ReadOnlySpan<TableRowMdSyntaxNode> GetRows() {
        ReadOnlySpan<IMdSyntaxNode> rowSpan = GetChildrenSpan()[(HeaderIndex + 1)..];
        return MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<IMdSyntaxNode, TableRowMdSyntaxNode>(ref MemoryMarshal.GetReference(rowSpan)),
            rowSpan.Length);
    }

    /// Updates the current instance with the provided table alignments.
    /// This method replaces any existing alignments with the new alignments provided in the input.
    /// <param name="alignments">
    /// A read-only span of <see cref="TableAlignment"/> values that represents the desired alignments
    /// for the table columns.
    /// </param>
    /// <returns>
    /// The current instance of <see cref="TableMdSyntaxNode"/> with the updated alignments.
    /// </returns>
    public TableMdSyntaxNode WithAlignments(scoped ReadOnlySpan<TableAlignment> alignments) {
        int arrayLength = alignments.Length;
        
        if (HasAlignments) ArrayPool<TableAlignment>.Shared.Return(Alignments, true);
        Alignments = ArrayPool<TableAlignment>.Shared.Rent(arrayLength);
        alignments.CopyTo(Alignments);
        
        HasAlignments = true;
        return this;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        if (HasAlignments) ArrayPool<TableAlignment>.Shared.Return(Alignments, true);
        Alignments = Array.Empty<TableAlignment>();
        HasAlignments = false;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(TableMdSyntaxNode? other) {
        for (int i = 0; i < Alignments.Length; i++) {
            TableAlignment? alignmentOther = other?.Alignments.ElementAtOrDefault(i);
            if (alignmentOther is null || alignmentOther != Alignments[i]) return false;
        }
        
        return base.Equals(other)
            && HasAlignments == other.HasAlignments;
    }

}

/// <summary>
/// Defines the alignment options for columns within a markdown table.
/// </summary>
/// <remarks>
/// This enum specifies the allowed column alignments for markdown tables:
/// - <see cref="TableAlignment.Left"/>: Aligns content to the left.
/// - <see cref="TableAlignment.Center"/>: Centers content horizontally.
/// - <see cref="TableAlignment.Right"/>: Aligns content to the right.
/// - <see cref="TableAlignment.Unknown"/>: Represents an unspecified or undefined alignment.
/// These alignment values are typically assigned to individual columns in tables to control
/// how the text is positioned within each cell of the corresponding column.
/// </remarks>
public enum TableAlignment {
    /// <summary>
    /// Aligns the content of a table column to the left.
    /// </summary>
    Left = -1,
    /// <summary>
    /// Aligns the content in the column to the center.
    /// </summary>
    Center = 0,
    /// <summary>
    /// Aligns the content of a table column to the right.
    /// </summary>
    Right = 1,
    /// <summary>
    /// Represents an undefined or unspecified column alignment in a markdown table.
    /// </summary>
    Unknown
}
