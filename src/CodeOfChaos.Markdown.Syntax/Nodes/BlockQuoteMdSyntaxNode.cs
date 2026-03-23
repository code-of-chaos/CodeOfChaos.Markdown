// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class BlockQuoteMdSyntaxNode : MdSyntaxNode<BlockQuoteMdSyntaxNode> {
    /// <summary>
    /// Gets the number of leading spaces preceding the content in a block quote node.
    /// </summary>
    /// <remarks>
    /// Leading spaces are used to determine the indentation of the block quote within the Markdown document.
    /// The value is always non-negative and is set using the <c>WithLeadingSpaces</c> method.
    /// To reset the value to zero, the <c>TryReset</c> method can be used.
    /// </remarks>
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the number of leading spaces for the block quote and returns the modified instance.
    /// </summary>
    /// <param name="leadingSpaces">The number of leading spaces to be set. If the value is negative, it will be corrected to zero.</param>
    /// <return>The current instance of <see cref="BlockQuoteMdSyntaxNode"/> with the updated leading spaces.</return>
    public BlockQuoteMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        LeadingSpaces = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(BlockQuoteMdSyntaxNode? other)
        => base.Equals(other)
            && LeadingSpaces == other.LeadingSpaces;
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: LS={LeadingSpaces}";
}
