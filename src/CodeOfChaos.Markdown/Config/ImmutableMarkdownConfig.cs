// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ImmutableMarkdownConfig : IMarkdownConfig {
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineNodeVisitors { get; init; }
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineNodeVisitors { get; init; }
    public required IMarkdownSyntaxNodeVisitor? FrontMatterNodeVisitor { get; init; }

    public required FrozenDictionary<Type, IJsonSyntaxNodeVisitor> JsonNodeVisitors { get; init; }
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

        return new ImmutableMarkdownConfig {
            SingleLineNodeVisitors = singleLine,
            MultiLineNodeVisitors = multiLine,
            FrontMatterNodeVisitor = frontMatterVisitor,
            JsonNodeVisitors = jsonNodeVisitors,
            BlazorComponents = blazorComponents,
            SkippedBlazorComponents = markdownConfig.SkippedBlazorComponentTypes.ToFrozenSet(),
            RenderUnknownBlazorComponents = markdownConfig.RenderUnknownBlazorComponents,
            HtmlRendererFootnoteWrapperType = markdownConfig.HtmlRendererFootnoteWrapperType,
        };
    }
    
    private static IMarkdownSyntaxNodeVisitor CreateMarkdownVisitor(Type visitorType) {
        object? instance = Activator.CreateInstance(visitorType);
        
        if (instance is null) throw new InvalidOperationException($"Could not create instance of visitor type '{visitorType.FullName}'.");
        if (instance is not IMarkdownSyntaxNodeVisitor visitor) throw new InvalidOperationException($"Configured visitor type '{visitorType.FullName}' does not implement {nameof(IMarkdownSyntaxNodeVisitor)}.");

        return visitor;
    }
    
    private static IJsonSyntaxNodeVisitor CreateJsonVisitor(Type visitorType) {
        object? instance = Activator.CreateInstance(visitorType);
        
        if (instance is null) throw new InvalidOperationException($"Could not create instance of visitor type '{visitorType.FullName}'.");
        if (instance is not IJsonSyntaxNodeVisitor visitor) throw new InvalidOperationException($"Configured visitor type '{visitorType.FullName}' does not implement {nameof(IJsonSyntaxNodeVisitor)}.");

        return visitor;
    }
}
