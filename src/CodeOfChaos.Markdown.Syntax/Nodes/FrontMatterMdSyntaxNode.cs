// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class FrontMatterMdSyntaxNode() : MdSyntaxNode<FrontMatterMdSyntaxNode>(initialChildCount:0) {
    /// <summary>
    /// Gets the textual content contained within the front matter section of a markdown document.
    /// </summary>
    /// <remarks>
    /// This property holds the content as a string, typically representing the metadata defined in the front matter
    /// of a markdown file. It can be updated using the <c>WithContent</c> method or reset to its default value
    /// through the <c>TryReset</c> method. The default value is an empty string.
    /// </remarks>
    public string Content { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the language specified within the front matter of a Markdown syntax node.
    /// </summary>
    /// <remarks>
    /// This property typically represents the language or format declaration
    /// provided in the front matter section of a Markdown document. It is
    /// commonly used to indicate programming languages, data formats, or
    /// other types of context-specific identifiers.
    /// </remarks>
    public string Language { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the number of dashes used in the front matter syntax of a Markdown document.
    /// </summary>
    /// <remarks>
    /// This property represents the number of consecutive dash characters (`-`) used
    /// to delimit the front matter section in a Markdown file. The default value is 3.
    /// </remarks>
    public int DashesCount { get; private set; } = DashesCountValue;
    /// <summary>
    /// Represents the number of leading spaces present before the content within a front matter syntax node.
    /// </summary>
    /// <remarks>
    /// This property determines how many whitespace characters (spaces) exist at the start of the front matter content.
    /// It is primarily used to preserve or validate formatting in the serialization or deserialization process.
    /// </remarks>
    public int LeadingSpaces { get; private set; } = LeadingSpacesValue;

    private const int DashesCountValue = 3;
    private const int LeadingSpacesValue = 0;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Sets the content of the <see cref="FrontMatterMdSyntaxNode"/> instance to the specified value.
    /// </summary>
    /// <param name="content">The new content to set for the node.</param>
    /// <returns>The current <see cref="FrontMatterMdSyntaxNode"/> instance with updated content.</returns>
    public FrontMatterMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }

    /// Sets the language for the current FrontMatterMdSyntaxNode instance and returns the updated node.
    /// <param name="language">The language to set for the node.</param>
    /// <return>The updated FrontMatterMdSyntaxNode instance with the specified language set.</return>
    public FrontMatterMdSyntaxNode WithLanguage(string language) {
        Language = language;
        return this;
    }

    /// Updates the node with the specified number of dashes and returns the updated node.
    /// <param name="dashesCount">The number of dashes to set for the node.</param>
    /// <return>The updated instance of <see cref="FrontMatterMdSyntaxNode"/>.</return>
    public FrontMatterMdSyntaxNode WithDashesCount(int dashesCount) {
        DashesCount = dashesCount;
        return this;
    }

    /// Updates the current instance with the specified number of leading spaces.
    /// <param name="leadingSpaces">The number of leading spaces to set.</param>
    /// <return>Returns the updated <see cref="FrontMatterMdSyntaxNode"/> instance with the specified leading spaces.</return>
    public FrontMatterMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = leadingSpaces;
        return this;
    }  
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        Language = string.Empty;
        DashesCount = DashesCountValue;
        LeadingSpaces = LeadingSpacesValue;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(FrontMatterMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content)
            && StringComparer.Ordinal.Equals(Language, other.Language)
            && DashesCount == other.DashesCount
            && LeadingSpaces == other.LeadingSpaces;
}
