// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using System.Collections.Frozen;

namespace CodeOfChaos.Markdown.Config;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownConfig {
    FrozenDictionary<Type, IMdComponentRecord> GetComponentRecords();
    FrozenSet<Type> GetSkippedBlazorComponentTypes();
    bool RenderUnknownBlazorComponents { get; }
    Type? HtmlRendererFootnoteWrapperType { get; }
}
