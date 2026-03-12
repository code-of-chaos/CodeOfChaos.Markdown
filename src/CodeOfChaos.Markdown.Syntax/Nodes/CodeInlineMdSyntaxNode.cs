// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class CodeInlineMdSyntaxNode() : MdSyntaxNode<CodeInlineMdSyntaxNode>(initialChildCount: 0) {
    /// Gets the content of the syntax node.
    /// Represents the textual content associated with this instance of the
    /// syntax node. This property is immutable outside the class, and its
    /// value can only be updated using the provided methods. By default,
    /// the content is initialized to an empty string.
    /// Use this property to retrieve the stored string value of the syntax
    /// node for processing or display purposes. Modifications should be made
    /// through the WithContent method to maintain immutability and ensure
    /// proper state management within the node hierarchy.
    public string Content { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the number of backticks used to delimit the inline code block in the Markdown syntax.
    /// </summary>
    /// <remarks>
    /// The <c>BackTickCount</c> property defines the count of backticks (`) that wrap the inline code segment.
    /// It ensures at least one backtick is present by default to properly delimit the inline code syntax.
    /// </remarks>
    public int BackTickCount { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the content of the current node with the specified value.
    /// <param name="content">The new content to set for the node.</param>
    /// <return>The updated instance of <see cref="CodeInlineMdSyntaxNode"/> with the specified content.</return>
    public CodeInlineMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }

    /// <summary>
    /// Updates the current <see cref="CodeInlineMdSyntaxNode"/> instance with the specified number of backticks.
    /// </summary>
    /// <param name="backTickCount">The number of backticks to set. A minimum value of 1 will be applied if the provided value is less than 1.</param>
    /// <returns>The updated <see cref="CodeInlineMdSyntaxNode"/> instance.</returns>
    public CodeInlineMdSyntaxNode WithBackTickCount(int backTickCount) {
        BackTickCount = Math.Max(1, backTickCount);
        return this;
    }

    /// Attempts to write the content of the current node to the specified destination buffer
    /// without including escaped backticks. Escaped backticks are identified by a preceding
    /// backslash followed by a backtick character.
    /// <param name="destination">
    /// A span where the content without escaped backticks will be written. The span must have
    /// sufficient capacity to hold the resulting data.
    /// </param>
    /// <param name="resultLength">
    /// When this method returns, contains the length of the content written to the destination
    /// if the operation is successful, or -1 if the buffer is too small.
    /// </param>
    /// <returns>
    /// True if the content was successfully written to the destination buffer; otherwise, false
    /// if the destination buffer is not large enough.
    /// </returns>
    public bool TryGetContentWithoutEscapedBackTicks(Span<char> destination, out int resultLength) {
        if (destination.Length < Content.Length) {
            resultLength = -1;
            return false;
        }
        ReadOnlySpan<char> originalSpan = Content.AsSpan();

        int destinationIndex = 0;
        for (int i = 0; i < originalSpan.Length; i++) {
            char c = originalSpan[i];
            if (i + 1 <= originalSpan.Length - 1
                && c == '\\'
                && originalSpan[i + 1] == '`'
                && originalSpan.IsEscapedCharacterAtIndex(i + 1)) {
                continue;
            }
            destination[destinationIndex++] = c;
        }

        resultLength = destinationIndex;
        return true;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        Content = string.Empty;
        BackTickCount = 0;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(CodeInlineMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content)
            && BackTickCount == other.BackTickCount;

    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: BTC={BackTickCount}";
}
