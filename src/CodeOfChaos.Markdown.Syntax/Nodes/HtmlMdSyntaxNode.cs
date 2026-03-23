// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class HtmlMdSyntaxNode() : MdSyntaxNode<HtmlMdSyntaxNode>(initialChildCount: 0) {
    /// Gets the textual content associated with the current HTML Markdown syntax node.
    /// This property represents the raw content held by the node, and it can be set explicitly
    /// using the `WithContent` method or reset to an empty string during a node reset operation.
    public string Content { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the content of the current node.
    /// </summary>
    /// <param name="content">The content to assign to the node.</param>
    /// <returns>The current instance of <see cref="HtmlMdSyntaxNode"/> with updated content.</returns>
    public HtmlMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(HtmlMdSyntaxNode? other)
        => base.Equals(other)
            && Content == other.Content;
}
