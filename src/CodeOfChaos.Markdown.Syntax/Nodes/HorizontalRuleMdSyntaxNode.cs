// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class HorizontalRuleMdSyntaxNode() : MdSyntaxNode<HorizontalRuleMdSyntaxNode>(initialChildCount: 0) {
    /// <summary>
    /// Gets the identifier associated with the current <c>HorizontalRuleMdSyntaxNode</c>.
    /// </summary>
    /// <remarks>
    /// The identifier is a string that uniquely identifies an instance of the node.
    /// This property is initialized with an empty string and can be modified using the <c>WithIdentifier</c> method.
    /// </remarks>
    public string Identifier { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the identifier for the current node.
    /// <param name="identifier">The identifier to be assigned to the node.</param>
    /// <returns>The updated instance of <see cref="HorizontalRuleMdSyntaxNode"/>.</returns>
    public HorizontalRuleMdSyntaxNode WithIdentifier(string identifier) {
        Identifier = identifier;
        return this;   
    }    
    
    /// <inheritdoc />
    public override bool TryReset() {
        Identifier = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(HorizontalRuleMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Identifier, other.Identifier);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Identifier}'";   
}
