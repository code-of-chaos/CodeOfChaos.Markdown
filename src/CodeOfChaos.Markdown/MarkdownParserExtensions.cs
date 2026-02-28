// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.Json;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class MarkdownParserExtensions {
    extension(IMarkdownParser parser) {
        #region Markdown
        public MarkdownParserContext FromMarkdown(string markdown)
            => MarkdownParserContext.Create(parser, (p, _) => p.Markdown.SerializeToSyntaxTree(markdown));
        #endregion

        #region Json
        public MarkdownParserContext FromJsonString(string json)
            => MarkdownParserContext.Create(parser, (p, _) => p.Json.SerializeToSyntaxTree(json));
        public MarkdownParserContext FromJson(JsonElement element)
            => MarkdownParserContext.Create(parser, (p, _) => p.Json.SerializeToSyntaxTree(element));
        public async Task<MarkdownParserContext> FromJson(Stream stream)
            => MarkdownParserContext.Create(parser, async (p, ct) => await p.Json.SerializeToSyntaxTreeAsync(stream, ct).ConfigureAwait(false));
        public async Task<MarkdownParserContext> FromJsonFile(string filePath)
            => MarkdownParserContext.Create(parser, async (p, ct) => await p.Json.SerializeToSyntaxTreeAsync(filePath, ct).ConfigureAwait(false));
        #endregion

        #region Xml
        public MarkdownParserContext FromXmlString(string xml)
            => MarkdownParserContext.Create(parser, (p, _) => p.Xml.SerializeStringToSyntaxTree(xml));
        public MarkdownParserContext FromXml(XElement xml)
            => MarkdownParserContext.Create(parser, (p, _) => p.Xml.SerializeToSyntaxTree(xml));
        public async Task<MarkdownParserContext> FromXml(Stream stream)
            => MarkdownParserContext.Create(parser, async (p, ct) => await p.Xml.SerializeToSyntaxTreeAsync(stream, ct).ConfigureAwait(false));
        public async Task<MarkdownParserContext> FromXmlFile(string filePath)
            => MarkdownParserContext.Create(parser, async (p, ct) => await p.Xml.SerializeFileToSyntaxTreeAsync(filePath, ct).ConfigureAwait(false));
        #endregion
    }
}
