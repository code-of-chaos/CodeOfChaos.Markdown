// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown.Config;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownConfig {
    ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineMarkdownSyntaxNodeVisitors { get; }
    ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineMarkdownSyntaxNodeVisitors { get; }
    IMarkdownSyntaxNodeVisitor? FrontMatterMarkdownSyntaxNodeVisitor { get; }
    FrozenDictionary<Type, IXmlSyntaxNodeVisitor> XmlSyntaxNodeVisitors { get; }
    FrozenDictionary<Type, IJsonSyntaxNodeVisitor> JsonSyntaxNodeVisitors { get; }
    FrozenDictionary<Type, IBlazorComponentBuilderRecord> BlazorComponents { get; }
    FrozenSet<Type> SkippedBlazorComponents { get; }

    bool RenderUnknownBlazorComponents { get; }
    Type? HtmlRendererFootnoteWrapperType { get; }
}
