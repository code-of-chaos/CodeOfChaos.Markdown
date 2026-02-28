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
public interface IMarkdownParser {
    IHtmlMdSyntaxTreeParser Html { get; }
    IMarkdownMdSyntaxTreeParser Markdown { get; }
    IXmlMdSyntaxTreeParser Xml { get; }
    IJsonMdSyntaxTreeParser Json { get; }
}
