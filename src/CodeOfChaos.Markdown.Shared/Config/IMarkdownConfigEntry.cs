// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;

namespace CodeOfChaos.Markdown.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents parser and renderer visitors associated with a syntax node type.
/// </summary>
public interface IMarkdownConfigEntry {
    /// <summary>
    /// Gets the syntax node type this entry targets.
    /// </summary>
    Type SyntaxNodeType { get; }

    /// <summary>
    /// Gets the optional Blazor component builder.
    /// </summary>
    IBlazorComponentBuilderRecord? BlazorComponentBuilder { get; }

    /// <summary>
    /// Gets the optional JSON visitor.
    /// </summary>
    IJsonSyntaxNodeVisitor? JsonNodeVisitor { get; }

    /// <summary>
    /// Gets the optional single-line markdown visitor.
    /// </summary>
    IMarkdownSyntaxNodeVisitor? MarkdownSingleLineNodeVisitor { get; }
    /// <summary>
    /// Gets the optional multi-line markdown visitor.
    /// </summary>
    IMarkdownSyntaxNodeVisitor? MarkdownMultiLineNodeVisitor { get; }
    /// <summary>
    /// Gets the optional front-matter markdown visitor.
    /// </summary>
    IMarkdownSyntaxNodeVisitor? MarkdownFrontMatterNodeVisitor { get; }

    /// <summary>
    /// Gets the optional XML visitor.
    /// </summary>
    IXmlSyntaxNodeVisitor? XmlNodeVisitor { get; }
    
    
}
