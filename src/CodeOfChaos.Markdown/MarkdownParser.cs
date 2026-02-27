// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Markdown;
using CodeOfChaos.Markdown.Markdown.Parsers.Html;
using CodeOfChaos.Markdown.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Markdown.Parsers.Xml;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IMarkdownParser>]
public class MarkdownParser(
    IHtmlMdSyntaxTreeParser html,
    IMarkdownMdSyntaxTreeParser markdownString,
    IXmlMdSyntaxTreeParser xml,
    IJsonMdSyntaxTreeParser json
) : IMarkdownParser {
    public IHtmlMdSyntaxTreeParser Html => html;
    public IMarkdownMdSyntaxTreeParser Markdown => markdownString;
    public IXmlMdSyntaxTreeParser Xml => xml;
    public IJsonMdSyntaxTreeParser Json => json;
}