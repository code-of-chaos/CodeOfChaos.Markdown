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
        public MarkdownParserContext<string> FromMarkdownString(string markdown)
            => MarkdownParserContext<string>.Create(
                parser,
                markdown,
                static (p, input, _) => p.Markdown.SerializeToSyntaxTree(input)
            );
        #endregion

        #region Json
        public MarkdownParserContext<string> FromJsonString(string json)
            => MarkdownParserContext<string>.Create(
                parser,
                json,
                static (p, input, _) => p.Json.SerializeToSyntaxTree(input)
            );
        
        public MarkdownParserContext<JsonElement> FromJson(JsonElement element)
            => MarkdownParserContext<JsonElement>.Create(
                parser,
                element,
                static (p, input, _) => p.Json.SerializeToSyntaxTree(input)
            );
        
        public MarkdownParserContext<Stream> FromJson(Stream stream)
            => MarkdownParserContext<Stream>.Create(
                parser,
                stream,
                static async (p, input, ct) => await p.Json.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        
        public MarkdownParserContext<string> FromJsonFile(string filePath)
            => MarkdownParserContext<string>.Create(
                parser,
                filePath,
                static async (p, input, ct) => await p.Json.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        #endregion

        #region Xml
        public MarkdownParserContext<string> FromXmlString(string xml)
            => MarkdownParserContext<string>.Create(
                parser,
                xml,
                static (p, input, _) => p.Xml.SerializeStringToSyntaxTree(input)
            );
        
        public MarkdownParserContext<XElement> FromXml(XElement xml)
            => MarkdownParserContext<XElement>.Create(
                parser,
                xml, 
                static (p, input, _) => p.Xml.SerializeToSyntaxTree(input)
            );
        
        public MarkdownParserContext<Stream> FromXml(Stream stream)
            => MarkdownParserContext<Stream>.Create(
                parser,
                stream,
                static async (p, input, ct) => await p.Xml.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        
        public MarkdownParserContext<string> FromXmlFile(string filePath)
            => MarkdownParserContext<string>.Create(
                parser,
                filePath,
                static async (p, input, ct) => await p.Xml.SerializeFileToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        #endregion
    }
}
