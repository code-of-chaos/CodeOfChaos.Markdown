// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class MarkdownConfig {
    internal List<IMarkdownConfigEntry> ConfigEntries { get; } = [];
    internal HashSet<Type> SkippedBlazorComponentTypes { get; } = [];
    
    public bool RenderUnknownBlazorComponents { get; set; }
    public Type? HtmlRendererFootnoteWrapperType { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public MarkdownConfig SkipBlazorRenderingOnComponent<TNode>() where TNode : class, IMdSyntaxNode {
        SkippedBlazorComponentTypes.Add(typeof(TNode));
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithSyntaxNode<TSyntaxNode>() where TSyntaxNode : class, IMdSyntaxNode {
        var entry = new MarkdownConfigEntry<TSyntaxNode>();
        ConfigEntries.Add(entry);
        return entry;
    }
}
