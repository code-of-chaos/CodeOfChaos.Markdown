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
public readonly record struct MarkdownParserContext<TInput> {
    private IMarkdownParser Parser { get; init; }
    private TInput Input { get; init; }
    private Func<IMarkdownParser, TInput, CancellationToken, ValueTask<IMdSyntaxTree>> TreeFactory { get; init; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static MarkdownParserContext<TInput> Create(IMarkdownParser parser, TInput input, Func<IMarkdownParser, TInput, CancellationToken, ValueTask<IMdSyntaxTree>> treeFactory) {
        return new MarkdownParserContext<TInput> {
            Parser = parser,
            Input = input,
            TreeFactory = treeFactory
        };
    }
    
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
    public async ValueTask<string> ToMarkdownAsync(CancellationToken ct = default)
        => Parser.Markdown.DeserializeToString(await TreeFactory(Parser, Input, ct));
    #endregion
    
    #region Html
    public async ValueTask<string> ToHtmlAsync(CancellationToken ct = default) 
        => await Parser.Html.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);
    #endregion
    
    #region Json
    public async ValueTask<string> ToJsonStringAsync(CancellationToken ct = default) 
        => await Parser.Json.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);
    
    public async ValueTask<JsonElement> ToJsonElementAsync(CancellationToken ct = default)
        => Parser.Json.DeserializeToJsonElement(await TreeFactory(Parser, Input, ct));
    
    public async ValueTask ToJsonStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonStreamAsync(stream, await TreeFactory(Parser, Input, ct), ct);
    
    public async ValueTask ToJsonFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonFileAsync(filePath, await TreeFactory(Parser, Input, ct), ct);
    #endregion
    
    #region Xml
    public async ValueTask<string> ToXmlStringAsync(CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToStringAsync(await TreeFactory(Parser, Input, ct), ct);  
    
    public async ValueTask<XElement> ToXmlElementAsync(CancellationToken ct = default) 
        => Parser.Xml.DeserializeToXmlElement(await TreeFactory(Parser, Input, ct));
    
    public async ValueTask ToXmlStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlStreamAsync(stream, await TreeFactory(Parser, Input, ct), ct);
    
    public async ValueTask ToXmlFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlFileAsync(filePath, await TreeFactory(Parser, Input, ct), ct);
    #endregion
}
