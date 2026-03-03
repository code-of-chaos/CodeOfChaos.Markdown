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
            .Where(entry => entry.MarkdownSingleLineNodeVisitorType is not null)
            .Select(entry => CreateMarkdownVisitor(entry.MarkdownSingleLineNodeVisitorType!))
            .ToImmutableArray();

        ImmutableArray<IMarkdownSyntaxNodeVisitor> multiLine = markdownConfig.ConfigEntries
            .Where(entry => entry.MarkdownMultiLineNodeVisitorType is not null)
            .Select(entry => CreateMarkdownVisitor(entry.MarkdownMultiLineNodeVisitorType!))
            .ToImmutableArray();

        Type? frontMatterVisitorType =
            markdownConfig.ConfigEntries.LastOrDefault(entry => entry.MarkdownFrontMatterNodeVisitorType is not null)
                ?.MarkdownFrontMatterNodeVisitorType;

        IMarkdownSyntaxNodeVisitor? frontMatterVisitor =
            frontMatterVisitorType is null ? null : CreateMarkdownVisitor(frontMatterVisitorType);

        FrozenDictionary<Type, IBlazorComponentBuilderRecord> blazorComponents = markdownConfig.ConfigEntries.Where(entry => entry.BlazorNodeVisitorType is not null)
            .Select<IMarkdownConfigEntry, IBlazorComponentBuilderRecord>(entry => entry.BlazorComponentBuilderRecord!)
            .ToFrozenDictionary(record => record.ComponentType, record => record);
        
        FrozenDictionary<Type, IJsonSyntaxNodeVisitor> jsonNodeVisitors = markdownConfig.ConfigEntries.Where(entry => entry.JsonNodeVisitorType is not null)
            .ToFrozenDictionary(entry => entry.SyntaxNodeType, entry => CreateJsonVisitor(entry.JsonNodeVisitorType!));
        
        FrozenDictionary<Type, IXmlSyntaxNodeVisitor> xmlSyntaxNodeVisitors = markdownConfig.ConfigEntries.Where(entry => entry.XmlNodeVisitorType is not null)
            .ToFrozenDictionary(entry => entry.SyntaxNodeType, entry => CreateXmlVisitor(entry.XmlNodeVisitorType!));

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
    
    private static IMarkdownSyntaxNodeVisitor CreateMarkdownVisitor(Type visitorType) {
        object? instance = Activator.CreateInstance(visitorType);
        
        // ReSharper disable twice ConvertIfStatementToReturnStatement
        if (instance is null) throw new InvalidOperationException($"Could not create instance of visitor type '{visitorType.FullName}'.");
        if (instance is not IMarkdownSyntaxNodeVisitor visitor) throw new InvalidOperationException($"Configured visitor type '{visitorType.FullName}' does not implement {nameof(IMarkdownSyntaxNodeVisitor)}.");

        return visitor;
    }
    
    private static IJsonSyntaxNodeVisitor CreateJsonVisitor(Type visitorType) {
        object? instance = Activator.CreateInstance(visitorType);
        
        // ReSharper disable twice ConvertIfStatementToReturnStatement
        if (instance is null) throw new InvalidOperationException($"Could not create instance of visitor type '{visitorType.FullName}'.");
        if (instance is not IJsonSyntaxNodeVisitor visitor) throw new InvalidOperationException($"Configured visitor type '{visitorType.FullName}' does not implement {nameof(IJsonSyntaxNodeVisitor)}.");

        return visitor;
    }
    
    private static IXmlSyntaxNodeVisitor CreateXmlVisitor(Type visitorType) {
        object? instance = Activator.CreateInstance(visitorType);
        
        // ReSharper disable twice ConvertIfStatementToReturnStatement
        if (instance is null) throw new InvalidOperationException($"Could not create instance of visitor type '{visitorType.FullName}'.");
        if (instance is not IXmlSyntaxNodeVisitor visitor) throw new InvalidOperationException($"Configured visitor type '{visitorType.FullName}' does not implement {nameof(IXmlSyntaxNodeVisitor)}.");

        return visitor;
    }
}
