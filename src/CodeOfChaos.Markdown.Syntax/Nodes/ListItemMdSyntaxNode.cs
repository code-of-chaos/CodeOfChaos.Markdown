// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class ListItemMdSyntaxNode : MdSyntaxNode<ListItemMdSyntaxNode> {
    /// <summary>
    /// Gets a value indicating whether the list item supports a checkable state.
    /// </summary>
    /// <remarks>
    /// A list item is considered checkable if the <c>OriginalCheckMarker</c> property
    /// is not null or empty. Checkable list items may be used to represent states such
    /// as "checked" or "unchecked" in task lists or similar markdown structures.
    /// </remarks>
    /// <value>
    /// <c>true</c> if the list item is checkable; otherwise, <c>false</c>.
    /// </value>
    public bool IsCheckable => OriginalCheckMarker.IsNotNullOrEmpty();
    
    /// <summary>
    /// Indicates whether the list item is inapplicable based on its original check marker.
    /// </summary>
    /// <remarks>
    /// A list item is considered inapplicable if its original check marker starts with the '~' character.
    /// </remarks>
    public bool IsInapplicable => OriginalCheckMarker.ElementAtOrDefault(0) == '~';
    
    /// Indicates whether the current list item is marked as checked.
    /// The property evaluates the `OriginalCheckMarker` string to determine
    /// if it starts with a case-insensitive 'x', signifying a checked state.
    /// Returns `true` if the list item is marked as checked; otherwise, `false`.
    public bool IsChecked => OriginalCheckMarker.ToLowerInvariant().ElementAtOrDefault(0) == 'x';
    
    /// <summary>
    /// Gets the index value associated with the list item.
    /// </summary>
    /// <remarks>
    /// The Index property represents a string identifier that can be used to uniquely reference
    /// or categorize a specific list item within the markdown syntax structure. It is initialized
    /// to an empty string by default and can be set using the <c>WithIndex</c> method.
    /// </remarks>
    public string Index { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the original marker associated with a checkable item in a Markdown list.
    /// This marker determines the state and type of the list item (e.g., checked, unchecked, or inapplicable).
    /// </summary>
    /// <remarks>
    /// The original marker can be analyzed to check various states, such as:
    /// - `~` signifies an inapplicable item.
    /// - `x` (case-insensitive) signifies a checked item.
    /// - Any other valid value signifies an unchecked but checkable item.
    /// An empty or null marker indicates the item is not checkable.
    /// </remarks>
    public string OriginalCheckMarker { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the number of leading spaces before the content within the list item.
    /// This property is used to determine the indentation level or spacing
    /// consistency for the relevant Markdown list item.
    /// </summary>
    public int LeadingSpaces { get; private set; }
    
    /// <summary>
    /// Gets the number of spaces preceding the check marker in a list item.
    /// </summary>
    /// <remarks>
    /// This property represents the leading whitespace specifically associated with the check marker
    /// (e.g., `[ ]`, `[x]`, `[~]`) in a markdown list item. It may differ from the general leading spaces
    /// identified in <see cref="LeadingSpaces"/> if additional indentation exists for formatting purposes.
    /// </remarks>
    public int CheckLeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the current syntax node with a specified index value.
    /// <param name="index">The index value to associate with this syntax node.</param>
    /// <returns>The updated instance of the <c>ListItemMdSyntaxNode</c>.</returns>
    public ListItemMdSyntaxNode WithIndex(string index) {
        Index = index;
        return this;
    }

    /// Updates the check marker for the current list item syntax node and returns the updated instance.
    /// <param name="checkMarker">The new check marker value to assign to the list item syntax node.</param>
    /// <return>The updated instance of <see cref="ListItemMdSyntaxNode"/> with the new check marker applied.</return>
    public ListItemMdSyntaxNode WithCheckMarker(string checkMarker) {
        OriginalCheckMarker = checkMarker;
        return this;
    }

    /// Updates the current instance with the specified number of leading spaces.
    /// <param name="leadingSpaces">The number of leading spaces to set. Negative values will be treated as zero.</param>
    /// <returns>The current instance with the updated leading spaces.</returns>
    public ListItemMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }

    /// Configures the ListItemMdSyntaxNode with the specified number of leading spaces, ensuring
    /// the value is non-negative.
    /// <param name="checkLeadingSpaces">The number of leading spaces to set.
    /// If less than zero, the value will be reset to zero.</param>
    /// <returns>The current instance of ListItemMdSyntaxNode with updated leading space configuration.</returns>
    public ListItemMdSyntaxNode WithCheckLeadingSpaces(int checkLeadingSpaces) {
        CheckLeadingSpaces = Math.Max(0, checkLeadingSpaces);
        return this;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        Index = string.Empty;
        OriginalCheckMarker = string.Empty;
        LeadingSpaces = 0;
        CheckLeadingSpaces = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(ListItemMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Index, other.Index)
            && StringComparer.Ordinal.Equals(OriginalCheckMarker, other.OriginalCheckMarker)
            && LeadingSpaces == other.LeadingSpaces
            && CheckLeadingSpaces == other.CheckLeadingSpaces;

    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Index}' '{OriginalCheckMarker}' LS={LeadingSpaces} CLS={CheckLeadingSpaces}";
}
