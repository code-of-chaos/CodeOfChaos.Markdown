// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Html;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts markdown syntax trees to HTML output.
/// </summary>
public interface IHtmlMdSyntaxTreeParser {
    /// <summary>
    /// Serializes a syntax tree to HTML.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The HTML output.</returns>
    Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default);
}
