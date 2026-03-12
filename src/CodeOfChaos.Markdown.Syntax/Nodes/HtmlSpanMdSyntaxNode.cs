// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class HtmlSpanMdSyntaxNode : MdSyntaxNode<HtmlSpanMdSyntaxNode> {
    /// Gets or sets the HTML attributes associated with this node.
    /// This property is used to define or update the attribute string for the HTML span element.
    /// Commonly used for rendering HTML elements with additional attributes such as styles, classes, or custom data.
    /// It defaults to an empty string when the node is reset or initialized.
    public string Attributes { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the current instance with the specified HTML attributes and returns the modified instance.
    /// <param name="attributes">The string representation of the HTML attributes to be set.</param>
    /// <return>The current instance with the updated attributes.</return>
    public HtmlSpanMdSyntaxNode WithAttributes(string attributes) {
        Attributes = attributes;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Attributes = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(HtmlSpanMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Attributes, other.Attributes);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Attributes}'";
}
