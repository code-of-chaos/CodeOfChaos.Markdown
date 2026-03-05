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
public interface IMarkdownConfigEntry {
    Type SyntaxNodeType { get; }

    Type? BlazorNodeVisitorType { get; }
    IBlazorComponentBuilderRecord? BlazorComponentBuilderRecord { get; }

    IJsonSyntaxNodeVisitor? JsonNodeVisitor { get; }

    IMarkdownSyntaxNodeVisitor? MarkdownSingleLineNodeVisitor { get; }
    IMarkdownSyntaxNodeVisitor? MarkdownMultiLineNodeVisitor { get; }
    IMarkdownSyntaxNodeVisitor? MarkdownFrontMatterNodeVisitor { get; }

    IXmlSyntaxNodeVisitor? XmlNodeVisitor { get; }
    
    
}
