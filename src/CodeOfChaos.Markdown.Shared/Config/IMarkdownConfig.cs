// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Markdown;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown.Config;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownConfig {
    ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineNodeVisitors { get; }
    ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineNodeVisitors { get; }
    IMarkdownSyntaxNodeVisitor? FrontMatterNodeVisitor { get; }
    FrozenDictionary<Type, IBlazorComponentBuilderRecord> BlazorComponents { get; }
    FrozenSet<Type> SkippedBlazorComponents { get; }

    bool RenderUnknownBlazorComponents { get; }
    Type? HtmlRendererFootnoteWrapperType { get; }
}
