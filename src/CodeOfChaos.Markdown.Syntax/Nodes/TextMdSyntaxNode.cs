// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class TextMdSyntaxNode() : MdSyntaxNode<TextMdSyntaxNode>(initialChildCount: 0) {
    /// <summary>
    /// Gets the content of the current syntax node represented as a string.
    /// This property holds the textual data that is associated with the node.
    /// </summary>
    public string Content { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the content of the current syntax node with the provided string value.
    /// <param name="content">The new content to set for the syntax node.</param>
    /// <return>The updated instance of the <see cref="TextMdSyntaxNode"/>.</return>
    public TextMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(TextMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content);

    /// <inheritdoc />
    public override string ToDebugString() 
        => $"{base.ToDebugString()}:'{Content}'";
}
