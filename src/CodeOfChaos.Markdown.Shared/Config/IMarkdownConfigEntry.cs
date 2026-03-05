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

    IBlazorComponentBuilderRecord? BlazorComponentBuilder { get; }

    IJsonSyntaxNodeVisitor? JsonNodeVisitor { get; }

    IMarkdownSyntaxNodeVisitor? MarkdownSingleLineNodeVisitor { get; }
    IMarkdownSyntaxNodeVisitor? MarkdownMultiLineNodeVisitor { get; }
    IMarkdownSyntaxNodeVisitor? MarkdownFrontMatterNodeVisitor { get; }

    IXmlSyntaxNodeVisitor? XmlNodeVisitor { get; }
    
    
}
