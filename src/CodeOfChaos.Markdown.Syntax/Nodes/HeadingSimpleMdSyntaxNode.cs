// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class HeadingSimpleMdSyntaxNode : MdSyntaxNode<HeadingSimpleMdSyntaxNode> {
    /// <summary>
    /// Gets the unique identifier associated with this node. This property is primarily used to store
    /// a string value that can act as a logical or contextual identifier for the current instance of
    /// <see cref="HeadingSimpleMdSyntaxNode"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Identifier"/> property is initialized as an empty string by default and can be updated
    /// using the <see cref="WithIdentifier(string)"/> method. It plays a key role in identifying or differentiating
    /// this node within the Markdown syntax tree.
    /// </remarks>
    public string Identifier { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the identifier for the current node and returns the updated instance.
    /// </summary>
    /// <param name="identifier">The identifier to associate with the node.</param>
    /// <returns>The updated <see cref="HeadingSimpleMdSyntaxNode"/> instance.</returns>
    public HeadingSimpleMdSyntaxNode WithIdentifier(string identifier) {
        Identifier = identifier;
        return this;   
    }    
    
    /// <inheritdoc />
    public override bool TryReset() {
        Identifier = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(HeadingSimpleMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Identifier, other.Identifier);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Identifier}'";   
}
