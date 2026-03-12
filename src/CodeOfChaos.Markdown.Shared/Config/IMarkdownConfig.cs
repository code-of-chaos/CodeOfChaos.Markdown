// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown.Config;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an immutable set of parser visitors and rendering options.
/// </summary>
public interface IMarkdownConfig {
    /// <summary>
    /// Gets single-line markdown visitors.
    /// </summary>
    ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineMarkdownSyntaxNodeVisitors { get; }
    /// <summary>
    /// Gets multi-line markdown visitors.
    /// </summary>
    ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineMarkdownSyntaxNodeVisitors { get; }
    /// <summary>
    /// Gets the optional front-matter markdown visitor.
    /// </summary>
    IMarkdownSyntaxNodeVisitor? FrontMatterMarkdownSyntaxNodeVisitor { get; }
    
    /// <summary>
    /// Gets XML visitors by syntax node type.
    /// </summary>
    FrozenDictionary<Type, IXmlSyntaxNodeVisitor> XmlSyntaxNodeVisitors { get; }
    /// <summary>
    /// Gets JSON visitors by syntax node type.
    /// </summary>
    FrozenDictionary<Type, IJsonSyntaxNodeVisitor> JsonSyntaxNodeVisitors { get; }
    /// <summary>
    /// Gets Blazor component builders by syntax node type.
    /// </summary>
    FrozenDictionary<Type, IBlazorComponentBuilderRecord> BlazorComponents { get; }
    /// <summary>
    /// Gets markdown visitors by syntax node type.
    /// </summary>
    FrozenDictionary<Type, IMarkdownSyntaxNodeVisitor> MarkdownSyntaxNodeVisitors { get; }
    
    /// <summary>
    /// Gets syntax node types that should be skipped during Blazor rendering.
    /// </summary>
    FrozenSet<Type> SkippedBlazorComponents { get; }

    /// <summary>
    /// Gets a value indicating whether unknown Blazor components should be rendered.
    /// </summary>
    bool RenderUnknownBlazorComponents { get; }
    /// <summary>
    /// Gets the optional wrapper component type used for HTML footnotes.
    /// </summary>
    Type? HtmlRendererFootnoteWrapperType { get; }
}
