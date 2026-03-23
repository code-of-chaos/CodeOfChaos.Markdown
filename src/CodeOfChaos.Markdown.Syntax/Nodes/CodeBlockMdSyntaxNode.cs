// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class CodeBlockMdSyntaxNode() : MdSyntaxNode<CodeBlockMdSyntaxNode>(initialChildCount: 0) {
    /// Gets the content of the code block node. This property holds the textual content
    /// within the code block element of Markdown syntax.
    /// The value of this property can be modified using the `WithContent` method.
    /// It is reset to an empty string when `TryReset` is called.
    public string Content { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the programming language associated with the code block.
    /// </summary>
    /// <remarks>
    /// This property specifies the language used for the code contained in the Markdown syntax node.
    /// It is typically used to enable syntax highlighting and other language-specific processing.
    /// </remarks>
    public string Language { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of <see cref="CodeBlockMdSyntaxNode"/> with the specified content.
    /// </summary>
    /// <param name="content">The content to set for the code block.</param>
    /// <returns>A new instance of <see cref="CodeBlockMdSyntaxNode"/> with the updated content.</returns>
    public CodeBlockMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }

    /// <summary>
    /// Sets the programming language associated with the code block.
    /// </summary>
    /// <param name="language">
    /// The programming language to be set.
    /// </param>
    /// <return>
    /// A new instance of <see cref="CodeBlockMdSyntaxNode"/> with the specified language set.
    /// </return>
    public CodeBlockMdSyntaxNode WithLanguage(string language) {
        Language = language;
        return this;
    }
    
    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        Language = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(CodeBlockMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content)
            && StringComparer.Ordinal.Equals(Language, other.Language);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Language}'";
}
