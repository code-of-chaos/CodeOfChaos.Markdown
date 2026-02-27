// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Syntax;
using Microsoft.AspNetCore.Components.Rendering;

namespace CodeOfChaos.Markdown.Markdown.Parsers.Blazor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMdComponentRecord {
    Type ComponentType { get; }
    Func<RenderTreeBuilder, int, IMdSyntaxNode, int> Builder { get; }
}
