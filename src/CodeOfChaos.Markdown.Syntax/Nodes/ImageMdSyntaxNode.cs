// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed partial class ImageMdSyntaxNode : MdSyntaxNode<ImageMdSyntaxNode> {
    /// <summary>
    /// Gets the hyperlink reference (href) associated with this image syntax node.
    /// </summary>
    /// <value>
    /// A string representing the URL or hyperlink reference for the image.
    /// </value>
    public string Href { get; private set; } = string.Empty;

    /// Gets the normalized alternative text of the image syntax node.
    /// The normalization process removes certain patterns or characters from the original alternative text
    /// as defined by the `NormalizeAltText` regular expression. This property is useful for ensuring
    /// a consistent and clean representation of the alternative text.
    public string NormalizedAltText => NormalizeAltText.Replace(OriginalAltText, string.Empty);

    /// <summary>
    /// Gets the original alternate text (alt text) provided for the image in the Markdown syntax node.
    /// This property represents the unprocessed text assigned to the image's alt attribute and retains it as-is
    /// before any normalization or modification.
    /// </summary>
    public string OriginalAltText { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the title associated with the image node.
    /// </summary>
    /// <remarks>
    /// The title provides additional information about the image
    /// and is typically displayed as a tooltip when the user hovers over the image.
    /// </remarks>
    public string Title { get; private set; } = string.Empty;

    [GeneratedRegex(@"\\(?!\\)")]
    private static partial Regex NormalizeAltText { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the hyperlink reference (href) for the image node.
    /// </summary>
    /// <param name="href">The hyperlink reference to associate with the image node.</param>
    /// <returns>The updated <see cref="ImageMdSyntaxNode"/> instance.</returns>
    public ImageMdSyntaxNode WithHref(string href) {
        Href = href;
        return this;
    }

    /// <summary>
    /// Sets the alternate text (alt text) for the image Markdown syntax node.
    /// </summary>
    /// <param name="altText">The alternate text to be assigned to the image node.</param>
    /// <returns>The updated <see cref="ImageMdSyntaxNode"/> instance with the specified alternate text.</returns>
    public ImageMdSyntaxNode WithAltText(string altText) {
        OriginalAltText = altText;
        return this;
    }

    /// Sets the title property of the image node.
    /// <param name="title">The title to assign to the image node.</param>
    /// <return>Returns the updated instance of the image node with the specified title.</return>
    public ImageMdSyntaxNode WithTitle(string title) {
        Title = title;
        return this;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        Href = string.Empty;
        OriginalAltText = string.Empty;
        Title = string.Empty;

        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(ImageMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(OriginalAltText, other.OriginalAltText)
            && StringComparer.Ordinal.Equals(Href, other.Href)
            && StringComparer.Ordinal.Equals(Title, other.Title);

    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{OriginalAltText}' '{Href}' '{Title}'";
}
