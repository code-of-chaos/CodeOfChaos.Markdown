// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class WikiLinkMdSyntaxNode() : MdSyntaxNode<WikiLinkMdSyntaxNode>(initialChildCount: 0) {
    /// Represents the textual content of a WikiLinkMdSyntaxNode.
    /// This property holds the value of the content associated with the node.
    /// The content is initialized as an empty string by default and is immutable
    /// outside of the class. It can only be updated or modified through methods
    /// provided by the WikiLinkMdSyntaxNode class, such as WithContent or TryReset.
    /// This property is relevant for scenarios where a node represents a portion
    /// of markdown syntax that includes customizable or user-defined content.
    /// Thread Safety:
    /// - Access to this property is thread-safe for reading as no external modification is allowed.
    /// - Methods updating this property (e.g., WithContent) should ensure thread safety when used concurrently.
    public string Content { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the current instance with the specified content and returns the updated instance.
    /// <param name="content">The new content to set for the node.</param>
    /// <return>The current instance with the updated content.</return>
    public WikiLinkMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(WikiLinkMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content);
    
    /// <inheritdoc />
    public override string ToDebugString() 
        => $"{base.ToDebugString()}: '{Content}'";
}
