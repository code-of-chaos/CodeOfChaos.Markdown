// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class EscapedCharacterMdSyntaxNode() : MdSyntaxNode<EscapedCharacterMdSyntaxNode>(initialChildCount:0) {
    /// Gets the character content of the `EscapedCharacterMdSyntaxNode`.
    /// This property represents the escaped character encapsulated within the node.
    /// Its default value is `char.MinValue`, indicating no character is set.
    /// Assigning a value to this property updates the content of the node to the specified character.
    public char Content { get; private set; } = char.MinValue;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the content of the current node with the specified character.
    /// <param name="content">The character to set as the content of the node.</param>
    /// <return>Returns the current instance of <see cref="EscapedCharacterMdSyntaxNode"/> with updated content.</return>
    public EscapedCharacterMdSyntaxNode WithContent(char content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = char.MinValue;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(EscapedCharacterMdSyntaxNode? other)
        => base.Equals(other)
            && Content == other.Content;
    
    /// <inheritdoc />
    public override string ToDebugString() 
        => $"{base.ToDebugString()}: '{Content}'";
}
