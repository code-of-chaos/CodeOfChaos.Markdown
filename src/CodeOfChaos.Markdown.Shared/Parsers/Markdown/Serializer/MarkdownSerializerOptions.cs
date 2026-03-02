// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownSerializerOptions {
    public List<IMarkdownSyntaxNodeVisitor> SingleLine { get; init; } = [];
    public List<IMarkdownSyntaxNodeVisitor> MultiLine { get; init; } = [];
    public IMarkdownSyntaxNodeVisitor? FrontMatter { get; init; }
}
