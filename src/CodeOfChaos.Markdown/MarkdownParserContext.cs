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
public readonly record struct MarkdownParserContext {
    private IMarkdownParser Parser { get; init; }
    private Func<IMarkdownParser, CancellationToken, ValueTask<IMdSyntaxTree>> TreeFactory { get; init; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static MarkdownParserContext Create(IMarkdownParser parser, Func<IMarkdownParser, CancellationToken, ValueTask<IMdSyntaxTree>> treeFactory) {
        return new MarkdownParserContext {
            Parser = parser,
            TreeFactory = treeFactory
        };
    }
    
    public static MarkdownParserContext Create(IMarkdownParser parser, Func<IMarkdownParser, CancellationToken, IMdSyntaxTree> treeFactory) {
        return new MarkdownParserContext {
            Parser = parser,
            TreeFactory = (p, ct) => ValueTask.FromResult(treeFactory(p, ct))
        };
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Markdown
    public async ValueTask<string> ToMarkdownAsync(CancellationToken ct = default)
        => Parser.Markdown.DeserializeToString(await TreeFactory(Parser, ct));
    #endregion
    
    #region Html
    public async ValueTask<string> ToHtmlAsync(CancellationToken ct = default) 
        => await Parser.Html.DeserializeToStringAsync(await TreeFactory(Parser, ct), ct);
    #endregion
    
    #region Json
    public async ValueTask<string> ToJsonStringAsync(CancellationToken ct = default) 
        => await Parser.Json.DeserializeToStringAsync(await TreeFactory(Parser, ct), ct);
    
    public async ValueTask<JsonElement> ToJsonElementAsync(CancellationToken ct = default)
        => Parser.Json.DeserializeToJsonElement(await TreeFactory(Parser, ct));
    
    public async ValueTask ToJsonStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonStreamAsync(stream, await TreeFactory(Parser, ct), ct);
    
    public async ValueTask ToJsonFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Json.DeserializeToJsonFileAsync(filePath, await TreeFactory(Parser, ct), ct);
    #endregion
    
    #region Xml
    public async ValueTask<string> ToXmlStringAsync(CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToStringAsync(await TreeFactory(Parser, ct), ct);  
    
    public async ValueTask<XElement> ToXmlElementAsync(CancellationToken ct = default) 
        => Parser.Xml.DeserializeToXmlElement(await TreeFactory(Parser, ct));
    
    public async ValueTask ToXmlStreamAsync(Stream stream, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlStreamAsync(stream, await TreeFactory(Parser, ct), ct);
    
    public async ValueTask ToXmlFileAsync(string filePath, CancellationToken ct = default) 
        => await Parser.Xml.DeserializeToXmlFileAsync(filePath, await TreeFactory(Parser, ct), ct);
    #endregion
}
