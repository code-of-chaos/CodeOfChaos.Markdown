// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class ListUnorderedMdSyntaxNode : MdSyntaxNode<ListUnorderedMdSyntaxNode> {
    /// <summary>
    /// Represents the count of leading spaces associated with this unordered list syntax node.
    /// This property indicates the level of indentation for the list item within the markdown document.
    /// </summary>
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new <see cref="ListUnorderedMdSyntaxNode"/> instance with the specified number of leading spaces.
    /// </summary>
    /// <param name="leadingSpaces">The number of spaces that lead the list item. The value will be set to 0 if it is negative.</param>
    /// <returns>The modified <see cref="ListUnorderedMdSyntaxNode"/> instance with updated leading spaces.</returns>
    public ListUnorderedMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        LeadingSpaces = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(ListUnorderedMdSyntaxNode? other)
        => base.Equals(other)
            && LeadingSpaces == other.LeadingSpaces;
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: LS={LeadingSpaces}";
}
