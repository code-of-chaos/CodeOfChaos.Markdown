// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Markdown.Parsers.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownMdSyntaxTreeParser {
    IMdSyntaxTree SerializeToSyntaxTree(string input);
    string DeserializeToString(IMdSyntaxTree tree);
}
