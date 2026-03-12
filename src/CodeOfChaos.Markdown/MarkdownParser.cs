// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Parsers.Html;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Aggregates markdown syntax-tree parsers for supported formats.
/// </summary>
/// <param name="html">The HTML parser.</param>
/// <param name="markdownString">The markdown-string parser.</param>
/// <param name="xml">The XML parser.</param>
/// <param name="json">The JSON parser.</param>
[InjectableSingleton<IMarkdownParser>]
public class MarkdownParser(
    IHtmlMdSyntaxTreeParser html,
    IMarkdownMdSyntaxTreeParser markdownString,
    IXmlMdSyntaxTreeParser xml,
    IJsonMdSyntaxTreeParser json
) : IMarkdownParser {
    /// <inheritdoc />
    public IHtmlMdSyntaxTreeParser Html => html;
    /// <inheritdoc />
    public IMarkdownMdSyntaxTreeParser Markdown => markdownString;
    /// <inheritdoc />
    public IXmlMdSyntaxTreeParser Xml => xml;
    /// <inheritdoc />
    public IJsonMdSyntaxTreeParser Json => json;
}
