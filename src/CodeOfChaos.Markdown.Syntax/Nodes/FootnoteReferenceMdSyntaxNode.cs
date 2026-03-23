// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class FootnoteReferenceMdSyntaxNode() : MdSyntaxNode<FootnoteReferenceMdSyntaxNode>(initialChildCount: 0) {
    /// <summary>
    /// Gets the identifier that uniquely represents this footnote reference within a Markdown syntax tree.
    /// </summary>
    public string Identifier { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the identifier for the current FootnoteReferenceMdSyntaxNode.
    /// <param name="identifier">The identifier to assign to the node.</param>
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
    protected override bool Equals([NotNullWhen(true)] FootnoteReferenceMdSyntaxNode? other) => 
        base.Equals(other)
        && StringComparer.Ordinal.Equals(Identifier, other.Identifier);

    /// <inheritdoc />
    public override string ToDebugString() 
        => $"{base.ToDebugString()}: '{Identifier}'";
}
