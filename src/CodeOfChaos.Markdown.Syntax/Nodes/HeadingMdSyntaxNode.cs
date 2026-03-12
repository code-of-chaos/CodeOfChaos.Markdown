// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class HeadingMdSyntaxNode : MdSyntaxNode<HeadingMdSyntaxNode> {
    /// <summary>
    /// Gets the level of the heading syntax node in the Markdown structure.
    /// </summary>
    /// <remarks>
    /// The level represents the depth of the heading, typically ranging from 1 (highest/most important heading)
    /// to 6 (lowest/least important heading). This corresponds to the Markdown heading levels (# to ######).
    /// </remarks>
    public int Level { get; private set; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Configures the current instance of the `HeadingMdSyntaxNode` with a specified heading level.
    /// <param name="level">
    /// The level to set for the heading node. Must be a value between 1 and 6, inclusive.
    /// </param>
    /// <return>
    /// The current instance of `HeadingMdSyntaxNode` with the updated level.
    /// </return>
    public HeadingMdSyntaxNode WithLevel(int level) {
        Level = Math.Clamp(1, level, 6);
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Level = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(HeadingMdSyntaxNode? other)
        => base.Equals(other)
            && Level == other.Level;
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: LVL={Level}";
}
