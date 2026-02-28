// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownMdSyntaxTreeParser {
    IMdSyntaxTree SerializeToSyntaxTree(string input);
    string DeserializeToString(IMdSyntaxTree tree);
}
