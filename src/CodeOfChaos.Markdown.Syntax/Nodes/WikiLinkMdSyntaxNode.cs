// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class WikiLinkMdSyntaxNode() : MdSyntaxNode<WikiLinkMdSyntaxNode>(initialChildCount: 0) {
    public string Content { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public WikiLinkMdSyntaxNode WithContent(string content) {
        Content = content;
        return this;
    }
    
    public override bool TryReset() {
        Content = string.Empty;
        return base.TryReset();
    }

    protected override bool Equals(WikiLinkMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(Content, other.Content);
    
    public override string ToDebugString() 
        => $"{base.ToDebugString()}: '{Content}'";
}
