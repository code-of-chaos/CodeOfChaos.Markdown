// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;

namespace CodeOfChaos.Markdown.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownConfigEntry {
    Type SyntaxNodeType { get; }

    Type? BlazorNodeVisitorType { get; }
    IBlazorComponentBuilderRecord? BlazorComponentBuilderRecord { get; }

    Type? JsonNodeVisitorType { get; }

    Type? MarkdownSingleLineNodeVisitorType { get; }
    Type? MarkdownMultiLineNodeVisitorType { get; }
    Type? MarkdownFrontMatterNodeVisitorType { get; }

    Type? XmlNodeVisitorType { get; }
    
    
}
