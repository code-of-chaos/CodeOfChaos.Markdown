// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class FootnoteDescriptionMdSyntaxNode() : MdSyntaxNode<FootnoteDescriptionMdSyntaxNode>(initialChildCount: 1) {
    public string Identifier { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void WithIdentifier(string identifier) {
        Identifier = identifier;
    }

    public override bool TryReset() {
        if (!base.TryReset()) return false;

        Identifier = string.Empty;
        return true;
    }

    protected override bool Equals([NotNullWhen(true)] FootnoteDescriptionMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Identifier, other.Identifier);

    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{Identifier}'";
}
