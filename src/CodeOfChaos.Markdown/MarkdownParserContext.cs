// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a deferred parser conversion context from an input value to one or more output formats.
/// </summary>
/// <typeparam name="TInput">The input type used to create the syntax tree.</typeparam>
public readonly record struct MarkdownParserContext<TInput> {
    private IMarkdownParser Parser { get; init; }
    private TInput Input { get; init; }
    private Func<IMarkdownParser, TInput, CancellationToken, ValueTask<IMdSyntaxTree>> TreeFactory { get; init; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a parser context from an asynchronous syntax-tree factory.
    /// </summary>
    /// <param name="parser">The parser facade used for conversions.</param>
    /// <param name="input">The input source value.</param>
    /// <param name="treeFactory">The delegate that builds an <see cref="IMdSyntaxTree" /> from the input.</param>
    /// <returns>A new parser context.</returns>
    public static MarkdownParserContext<TInput> Create(IMarkdownParser parser, TInput input, Func<IMarkdownParser, TInput, CancellationToken, ValueTask<IMdSyntaxTree>> treeFactory) {
        return new MarkdownParserContext<TInput> {
            Parser = parser,
            Input = input,
            TreeFactory = treeFactory
        };
    }
    
    /// <summary>
    /// Creates a parser context from a synchronous syntax-tree factory.
    /// </summary>
    /// <param name="parser">The parser facade used for conversions.</param>
    /// <param name="input">The input source value.</param>
    /// <param name="treeFactory">The delegate that builds an <see cref="IMdSyntaxTree" /> from the input.</param>
    /// <returns>A new parser context.</returns>
    public static MarkdownParserContext<TInput> Create(IMarkdownParser parser, TInput input, Func<IMarkdownParser,TInput, CancellationToken, IMdSyntaxTree> treeFactory) {
        return new MarkdownParserContext<TInput> {
            Parser = parser,
            Input = input,
            TreeFactory = (p, i, ct) => ValueTask.FromResult(treeFactory(p,i, ct))
        };
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Markdown
    /// <summary>
    /// Converts the input to markdown text.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The markdown representation.</returns>
    public async ValueTask<string> ToMarkdownAsync(CancellationToken ct = default)
        => Parser.Markdown.DeserializeToString(await TreeFactory(Parser, Input, ct));
    #endregion
    
    #region Html
    /// <summary>
    /// Converts the input to HTML.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The HTML representation.</returns>
    public async ValueTask<string> ToHtmlAsync(CancellationToken ct = default) 
        => await Parser.Html.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);
    #endregion
    
    #region Json
    /// <summary>
    /// Converts the input to JSON text.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The JSON string representation.</returns>
    public async ValueTask<string> ToJsonStringAsync(CancellationToken ct = default) 
        => await Parser.Json.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);
    
    /// <summary>
    /// Converts the input to a <see cref="JsonElement" />.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The JSON element representation.</returns>
    public async ValueTask<JsonElement> ToJsonElementAsync(CancellationToken ct = default)
        => Parser.Json.DeserializeToJsonElement(await TreeFactory(Parser, Input, ct));
    
    /// <summary>
    /// Converts the input to JSON and writes it to a stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="ct">The cancellation token.</param>
    public async ValueTask ToJsonStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonStreamAsync(stream, await TreeFactory(Parser, Input, ct), ct);
    
    /// <summary>
    /// Converts the input to JSON and writes it to a file.
    /// </summary>
    /// <param name="filePath">The output file path.</param>
    /// <param name="ct">The cancellation token.</param>
    public async ValueTask ToJsonFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonFileAsync(filePath, await TreeFactory(Parser, Input, ct), ct);
    #endregion
    
    #region Xml
    /// <summary>
    /// Converts the input to XML text.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The XML string representation.</returns>
    public async ValueTask<string> ToXmlStringAsync(CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);  
    
    /// <summary>
    /// Converts the input to an <see cref="XElement" />.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The XML element representation.</returns>
    public async ValueTask<XElement> ToXmlElementAsync(CancellationToken ct = default) 
        => Parser.Xml.DeserializeToXmlElement(await TreeFactory(Parser, Input, ct));
    
    /// <summary>
    /// Converts the input to XML and writes it to a stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="ct">The cancellation token.</param>
    public async ValueTask ToXmlStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlStreamAsync(stream, await TreeFactory(Parser, Input, ct), ct);
    
    /// <summary>
    /// Converts the input to XML and writes it to a file.
    /// </summary>
    /// <param name="filePath">The output file path.</param>
    /// <param name="ct">The cancellation token.</param>
    public async ValueTask ToXmlFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlFileAsync(filePath, await TreeFactory(Parser, Input, ct), ct);
    #endregion
}
