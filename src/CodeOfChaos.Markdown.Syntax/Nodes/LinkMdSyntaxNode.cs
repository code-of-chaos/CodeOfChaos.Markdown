// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class LinkMdSyntaxNode : MdSyntaxNode<LinkMdSyntaxNode> {
    /// Represents the URL or hyperlink reference associated with the link.
    /// This property defines the destination for a hyperlink within a markdown
    /// syntax node. It is intended to store a valid URL or URI string and is
    /// enforced as a private set to ensure immutability after initialization.
    /// The default value for this property is an empty string.
    public string Href { get; private set; } = string.Empty;
    
    /// Gets the title associated with the link, which typically serves as optional descriptive text.
    /// The title can provide additional context or information about the link's target.
    public string Title { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Defines a hyperlink reference for the node.
    /// <param name="href">The URL or hyperlink destination to associate with the syntax node.</param>
    /// <return>The updated <see cref="LinkMdSyntaxNode"/> instance with the specified href set.</return>
    public LinkMdSyntaxNode WithHref(string href) {
        Href = href;
        return this;   
    }

    /// Sets the title of the link node.
    /// <param name="title">The title to associate with the link node.</param>
    /// <returns>A new instance of <see cref="LinkMdSyntaxNode"/> with the specified title set.</returns>
    public LinkMdSyntaxNode WithTitle(string title) {
        Title = title;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Href = string.Empty;
        Title = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(LinkMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Href, other.Href)
            && StringComparer.Ordinal.Equals(Title, other.Title);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Href}' '{Title}'";
}
