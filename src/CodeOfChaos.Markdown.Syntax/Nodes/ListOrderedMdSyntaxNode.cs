// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class ListOrderedMdSyntaxNode : MdSyntaxNode<ListOrderedMdSyntaxNode> {
    /// <summary>
    /// Gets the number of leading spaces associated with the current syntax node.
    /// </summary>
    /// <remarks>
    /// The LeadingSpaces property represents the count of whitespace characters that precede
    /// the content within a Markdown ordered list syntax node. This property is integral
    /// for maintaining proper alignment and formatting in Markdown documents,
    /// ensuring the correct structure and readability of lists.
    /// </remarks>
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Adjusts the leading spaces for the current instance and returns the updated instance.
    /// <param name="leadingSpaces">The number of leading spaces to be set. Values less than 0 will be treated as 0.</param>
    /// <return>Returns the modified instance of <see cref="ListOrderedMdSyntaxNode"/>.</return>
    public ListOrderedMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        LeadingSpaces = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(ListOrderedMdSyntaxNode? other)
        => base.Equals(other)
            && LeadingSpaces == other.LeadingSpaces;
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: LS={LeadingSpaces}";
}
