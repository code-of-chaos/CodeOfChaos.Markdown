// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components.Rendering;

namespace CodeOfChaos.Markdown.Parsers.Blazor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IBlazorComponentBuilderRecord {
    Type ComponentType { get; }
    Func<RenderTreeBuilder, int, IMdSyntaxNode, int> Builder { get; }
}
