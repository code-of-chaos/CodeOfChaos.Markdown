// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class TagMdSyntaxNode() : MdSyntaxNode<TagMdSyntaxNode>(initialChildCount:0) {
    /// Represents the content of a tag in the Markdown syntax.
    /// This property stores the inner content associated with the tag node.
    /// The value is initialized to an empty string by default and can be updated
    /// using the `WithContent` method. The content is compared using an ordinal
    /// string comparer to ensure precise matching.
    /// Accessibility:
    /// - Getter: Public
    /// - Setter: Private
    /// Key Behavior:
    /// - Reset: When the `TryReset` method is invoked, the content is cleared (set to an empty string).
    /// - Equality: Compared using the `StringComparer.Ordinal` during the equality check of the node.
    public string Content { get; private set; } = string.Empty;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the content for the current TagMdSyntaxNode instance.
    /// <param name="content">The content string to be assigned to the node.</param>
    /// <return>The current instance of <see cref="TagMdSyntaxNode"/> with the updated content.</return>
    public TagMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(TagMdSyntaxNode? other) 
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Content}'";
}
