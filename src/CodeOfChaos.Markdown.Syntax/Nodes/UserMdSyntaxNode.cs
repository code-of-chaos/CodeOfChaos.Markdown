// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class UserMdSyntaxNode() : MdSyntaxNode<UserMdSyntaxNode>(initialChildCount:0) {
    /// <summary>
    /// Gets the content associated with this syntax node.
    /// </summary>
    public string Content { get; private set; } = string.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the content for the current node and returns the modified instance of the node.
    /// <param name="content">The content to set for this node.</param>
    /// <return>The current node instance with the updated content.</return>
    public UserMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(UserMdSyntaxNode? other) 
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content);
    
    /// <inheritdoc />
    public override string ToDebugString() 
        => $"{base.ToDebugString()}: '{Content}'";
}
