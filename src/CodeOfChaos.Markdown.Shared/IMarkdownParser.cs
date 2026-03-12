// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Html;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Exposes format-specific parsers for converting between markdown syntax trees and external representations.
/// </summary>
public interface IMarkdownParser {
    /// <summary>
    /// Gets the HTML parser.
    /// </summary>
    IHtmlMdSyntaxTreeParser Html { get; }
    /// <summary>
    /// Gets the markdown text parser.
    /// </summary>
    IMarkdownMdSyntaxTreeParser Markdown { get; }
    /// <summary>
    /// Gets the XML parser.
    /// </summary>
    IXmlMdSyntaxTreeParser Xml { get; }
    /// <summary>
    /// Gets the JSON parser.
    /// </summary>
    IJsonMdSyntaxTreeParser Json { get; }
}
