// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class ListUnorderedMdSyntaxNode : MdSyntaxNode<ListUnorderedMdSyntaxNode> {
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public ListUnorderedMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }
    
    public override bool TryReset() {
        LeadingSpaces = 0;
        return base.TryReset();
    }

    protected override bool Equals(ListUnorderedMdSyntaxNode? other)
        => base.Equals(other)
            && LeadingSpaces == other.LeadingSpaces;
    
    public override string ToDebugString()
        => $"{base.ToDebugString()}: LS={LeadingSpaces}";
}
