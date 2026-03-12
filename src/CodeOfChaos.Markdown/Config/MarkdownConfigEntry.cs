// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Langs.Blazor;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownConfigEntry<TSyntaxNode> : IMarkdownConfigEntry where TSyntaxNode : class, IMdSyntaxNode {
    /// <inheritdoc />
    public Type SyntaxNodeType { get; } = typeof(TSyntaxNode);
    
    /// <inheritdoc />
    public IBlazorComponentBuilderRecord? BlazorComponentBuilder { get; set; }
    
    /// <inheritdoc />
    public IJsonSyntaxNodeVisitor? JsonNodeVisitor { get; set; }
    
    /// <inheritdoc />
    public IMarkdownSyntaxNodeVisitor? MarkdownSingleLineNodeVisitor { get; set; }
    /// <inheritdoc />
    public IMarkdownSyntaxNodeVisitor? MarkdownMultiLineNodeVisitor { get; set; }
    /// <inheritdoc />
    public IMarkdownSyntaxNodeVisitor? MarkdownFrontMatterNodeVisitor { get; set; }
    
    /// <inheritdoc />
    public IXmlSyntaxNodeVisitor? XmlNodeVisitor { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Configures the Blazor component builder using a component type.
    /// </summary>
    /// <typeparam name="TVisitor">The component type implementing <see cref="IBlazorSyntaxNodeVisitor{TSyntaxNode}" />.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithBlazorNodeVisitor<TVisitor>() where TVisitor : class, IBlazorSyntaxNodeVisitor<TSyntaxNode> {
        BlazorComponentBuilder = BlazorComponentBuilderRecord.FromType<TSyntaxNode, TVisitor>();
        return this;
    }
    
    /// <summary>
    /// Configures the JSON visitor.
    /// </summary>
    /// <typeparam name="TVisitor">The JSON visitor type.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithJsonNodeVisitor<TVisitor>() where TVisitor : class, IJsonSyntaxNodeVisitor<TSyntaxNode>, new() {
        JsonNodeVisitor = new TVisitor();
        return this;
    }
    
    /// <summary>
    /// Configures the single-line markdown visitor.
    /// </summary>
    /// <typeparam name="TVisitor">The markdown visitor type.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownSingleLineNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownSingleLineNodeVisitor = new TVisitor();
        return this;
    }
    
    /// <summary>
    /// Configures the multi-line markdown visitor.
    /// </summary>
    /// <typeparam name="TVisitor">The markdown visitor type.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownMultiLineNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownMultiLineNodeVisitor = new TVisitor();
        return this;
    }
    
    /// <summary>
    /// Configures the front-matter markdown visitor.
    /// </summary>
    /// <typeparam name="TVisitor">The markdown visitor type.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownFrontMatterNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownFrontMatterNodeVisitor = new TVisitor();
        return this;
    }
    
    /// <summary>
    /// Configures the XML visitor.
    /// </summary>
    /// <typeparam name="TVisitor">The XML visitor type.</typeparam>
    /// <returns>The current configuration entry.</returns>
    public MarkdownConfigEntry<TSyntaxNode> WithXmlNodeVisitor<TVisitor>() where TVisitor : class, IXmlSyntaxNodeVisitor<TSyntaxNode>, new() {
        XmlNodeVisitor = new TVisitor();
        return this;
    }
}


