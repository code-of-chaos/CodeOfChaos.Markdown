// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ImmutableMarkdownConfig : IMarkdownConfig {
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineMarkdownSyntaxNodeVisitors { get; init; }
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineMarkdownSyntaxNodeVisitors { get; init; }
    public required IMarkdownSyntaxNodeVisitor? FrontMatterMarkdownSyntaxNodeVisitor { get; init; }

    public required FrozenDictionary<Type, IXmlSyntaxNodeVisitor> XmlSyntaxNodeVisitors { get; init; }
    public required FrozenDictionary<Type, IJsonSyntaxNodeVisitor> JsonSyntaxNodeVisitors { get; init; }
    public required FrozenDictionary<Type, IBlazorComponentBuilderRecord> BlazorComponents { get; init; }
    public required FrozenSet<Type> SkippedBlazorComponents { get; init; }
    
    public required bool RenderUnknownBlazorComponents { get; init; }
    public required Type? HtmlRendererFootnoteWrapperType { get; init; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static IMarkdownConfig From(MarkdownConfig markdownConfig) {

        ImmutableArray<IMarkdownSyntaxNodeVisitor> singleLine = markdownConfig.ConfigEntries
            .Where(entry => entry.MarkdownSingleLineNodeVisitor is not null)
            .Select(entry => entry.MarkdownSingleLineNodeVisitor!)
            .ToImmutableArray();

        ImmutableArray<IMarkdownSyntaxNodeVisitor> multiLine = markdownConfig.ConfigEntries
            .Where(entry => entry.MarkdownMultiLineNodeVisitor is not null)
            .Select(entry => entry.MarkdownMultiLineNodeVisitor!)
            .ToImmutableArray();

        IMarkdownSyntaxNodeVisitor? frontMatterVisitor = markdownConfig.ConfigEntries.LastOrDefault(entry => entry.MarkdownFrontMatterNodeVisitor is not null)
            ?.MarkdownFrontMatterNodeVisitor;

        FrozenDictionary<Type, IBlazorComponentBuilderRecord> blazorComponents = markdownConfig.ConfigEntries.Where(entry => entry.BlazorComponentBuilder is not null)
            .Select<IMarkdownConfigEntry, IBlazorComponentBuilderRecord>(entry => entry.BlazorComponentBuilder!)
            .ToFrozenDictionary(record => record.ComponentType, record => record);
        
        FrozenDictionary<Type, IJsonSyntaxNodeVisitor> jsonNodeVisitors = markdownConfig.ConfigEntries.Where(entry => entry.JsonNodeVisitor is not null)
            .ToFrozenDictionary(entry => entry.SyntaxNodeType, entry => entry.JsonNodeVisitor!);
        
        FrozenDictionary<Type, IXmlSyntaxNodeVisitor> xmlSyntaxNodeVisitors = markdownConfig.ConfigEntries.Where(entry => entry.XmlNodeVisitor is not null)
            .ToFrozenDictionary(entry => entry.SyntaxNodeType, entry => entry.XmlNodeVisitor!);

        return new ImmutableMarkdownConfig {
            SingleLineMarkdownSyntaxNodeVisitors = singleLine,
            MultiLineMarkdownSyntaxNodeVisitors = multiLine,
            FrontMatterMarkdownSyntaxNodeVisitor = frontMatterVisitor,
            XmlSyntaxNodeVisitors = xmlSyntaxNodeVisitors,
            JsonSyntaxNodeVisitors = jsonNodeVisitors,
            BlazorComponents = blazorComponents,
            SkippedBlazorComponents = markdownConfig.SkippedBlazorComponentTypes.ToFrozenSet(),
            RenderUnknownBlazorComponents = markdownConfig.RenderUnknownBlazorComponents,
            HtmlRendererFootnoteWrapperType = markdownConfig.HtmlRendererFootnoteWrapperType,
        };
    }
}
