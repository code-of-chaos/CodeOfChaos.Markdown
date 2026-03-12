// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.Json;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Provides fluent entry points for creating <see cref="MarkdownParserContext{TInput}" /> values from supported input formats.
/// </summary>
public static class MarkdownParserExtensions {
    extension(IMarkdownParser parser) {
        #region Markdown
        /// <summary>
        /// Creates a conversion context from markdown text.
        /// </summary>
        /// <param name="markdown">The markdown input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<string> FromMarkdownString(string markdown)
            => MarkdownParserContext<string>.Create(
                parser,
                markdown,
                static (p, input, _) => p.Markdown.SerializeToSyntaxTree(input)
            );
        #endregion

        #region Json
        /// <summary>
        /// Creates a conversion context from JSON text.
        /// </summary>
        /// <param name="json">The JSON input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<string> FromJsonString(string json)
            => MarkdownParserContext<string>.Create(
                parser,
                json,
                static (p, input, _) => p.Json.SerializeToSyntaxTree(input)
            );
        
        /// <summary>
        /// Creates a conversion context from a JSON element.
        /// </summary>
        /// <param name="element">The JSON element input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<JsonElement> FromJson(JsonElement element)
            => MarkdownParserContext<JsonElement>.Create(
                parser,
                element,
                static (p, input, _) => p.Json.SerializeToSyntaxTree(input)
            );
        
        /// <summary>
        /// Creates a conversion context from a JSON stream.
        /// </summary>
        /// <param name="stream">The JSON stream input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<Stream> FromJson(Stream stream)
            => MarkdownParserContext<Stream>.Create(
                parser,
                stream,
                static async (p, input, ct) => await p.Json.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        
        /// <summary>
        /// Creates a conversion context from a JSON file.
        /// </summary>
        /// <param name="filePath">The JSON file path.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<string> FromJsonFile(string filePath)
            => MarkdownParserContext<string>.Create(
                parser,
                filePath,
                static async (p, input, ct) => await p.Json.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        #endregion

        #region Xml
        /// <summary>
        /// Creates a conversion context from XML text.
        /// </summary>
        /// <param name="xml">The XML input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<string> FromXmlString(string xml)
            => MarkdownParserContext<string>.Create(
                parser,
                xml,
                static (p, input, _) => p.Xml.SerializeStringToSyntaxTree(input)
            );
        
        /// <summary>
        /// Creates a conversion context from an XML element.
        /// </summary>
        /// <param name="xml">The XML element input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<XElement> FromXml(XElement xml)
            => MarkdownParserContext<XElement>.Create(
                parser,
                xml, 
                static (p, input, _) => p.Xml.SerializeToSyntaxTree(input)
            );
        
        /// <summary>
        /// Creates a conversion context from an XML stream.
        /// </summary>
        /// <param name="stream">The XML stream input.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<Stream> FromXml(Stream stream)
            => MarkdownParserContext<Stream>.Create(
                parser,
                stream,
                static async (p, input, ct) => await p.Xml.SerializeToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        
        /// <summary>
        /// Creates a conversion context from an XML file.
        /// </summary>
        /// <param name="filePath">The XML file path.</param>
        /// <returns>A parser context for the input.</returns>
        public MarkdownParserContext<string> FromXmlFile(string filePath)
            => MarkdownParserContext<string>.Create(
                parser,
                filePath,
                static async (p, input, ct) => await p.Xml.SerializeFileToSyntaxTreeAsync(input, ct).ConfigureAwait(false)
            );
        #endregion
    }
}
