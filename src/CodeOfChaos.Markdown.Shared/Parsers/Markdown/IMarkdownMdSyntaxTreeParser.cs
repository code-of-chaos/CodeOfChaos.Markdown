// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts between markdown text and syntax trees.
/// </summary>
public interface IMarkdownMdSyntaxTreeParser {
    /// <summary>
    /// Parses markdown text into a syntax tree.
    /// </summary>
    /// <param name="input">The markdown input.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeToSyntaxTree(string input);
    /// <summary>
    /// Serializes a syntax tree to markdown text.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <returns>The markdown output.</returns>
    string DeserializeToString(IMdSyntaxTree tree);
}
