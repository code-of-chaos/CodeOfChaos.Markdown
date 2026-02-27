// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Parsers.Html;
using CodeOfChaos.Markdown.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Markdown.Parsers.Xml;

namespace CodeOfChaos.Markdown.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownParser {
    IHtmlMdSyntaxTreeParser Html { get; }
    IMarkdownMdSyntaxTreeParser Markdown { get; }
    IXmlMdSyntaxTreeParser Xml { get; }
    IJsonMdSyntaxTreeParser Json { get; }
}
