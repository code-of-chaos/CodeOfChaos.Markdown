// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class FootnoteDescriptionMdSyntaxNode() : MdSyntaxNode<FootnoteDescriptionMdSyntaxNode>(initialChildCount: 1) {
    /// <summary>
    /// Gets the identifier associated with the <see cref="FootnoteDescriptionMdSyntaxNode"/>.
    /// This property stores a unique string value representing the identifier of the footnote description.
    /// </summary>
    /// <remarks>
    /// The <c>Identifier</c> property is initialized to an empty string by default and is updated
    /// using the <see cref="WithIdentifier(string)"/> method. It must not be set to null.
    /// </remarks>
    public string Identifier { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Updates the identifier of the current syntax node.
    /// <param name="identifier">The new identifier to assign to the node.</param>
    public void WithIdentifier(string identifier) {
        Identifier = identifier;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        if (!base.TryReset()) return false;

        Identifier = string.Empty;
        return true;
    }

    /// <inheritdoc />
    protected override bool Equals([NotNullWhen(true)] FootnoteDescriptionMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Identifier, other.Identifier);

    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Identifier}'";
}
