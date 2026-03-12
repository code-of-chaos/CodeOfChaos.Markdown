// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Mutable configuration used to register syntax-node visitors and rendering options.
/// </summary>
public sealed class MarkdownConfig {
    internal List<IMarkdownConfigEntry> ConfigEntries { get; } = [];
    internal HashSet<Type> SkippedBlazorComponentTypes { get; } = [];
    
    /// <summary>
    /// Gets or sets whether unknown syntax nodes should still render as fallback Blazor components.
    /// </summary>
    public bool RenderUnknownBlazorComponents { get; set; }
    /// <summary>
    /// Gets or sets the wrapper element type used for rendered footnotes in HTML.
    /// </summary>
    public Type? HtmlRendererFootnoteWrapperType { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Skips Blazor rendering for the specified syntax-node type.
    /// </summary>
    /// <typeparam name="TNode">The syntax-node type to skip.</typeparam>
    /// <returns>The current configuration instance.</returns>
    public MarkdownConfig SkipBlazorRenderingOnComponent<TNode>() where TNode : class, IMdSyntaxNode {
        SkippedBlazorComponentTypes.Add(typeof(TNode));
        return this;
    }
    
    /// <summary>
    /// Adds a syntax-node configuration entry for the specified node type.
    /// </summary>
    /// <typeparam name="TSyntaxNode">The syntax-node type to configure.</typeparam>
    /// <returns>A configuration entry for the syntax-node type.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithSyntaxNode<TSyntaxNode>() where TSyntaxNode : class, IMdSyntaxNode {
        var entry = new MarkdownConfigEntry<TSyntaxNode>();
        ConfigEntries.Add(entry);
        return entry;
    }
}
