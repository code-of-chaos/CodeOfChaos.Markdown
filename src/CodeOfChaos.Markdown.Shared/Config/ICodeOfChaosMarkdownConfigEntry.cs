// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ICodeOfChaosMarkdownConfigEntry {
    Type SyntaxNodeType { get; }

    Type? BlazorNodeVisitorType { get; }

    Type? JsonNodeVisitorType { get; }

    Type? MarkdownSingleLineNodeVisitorType { get; }
    Type? MarkdownMultiLineNodeVisitorType { get; }
    Type? MarkdownFrontMatterNodeVisitorType { get; }

    Type? XmlNodeVisitorType { get; }
}
