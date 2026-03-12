// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components.Rendering;

namespace CodeOfChaos.Markdown.Parsers.Blazor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Describes how to render a markdown syntax node as a Blazor component.
/// </summary>
public interface IBlazorComponentBuilderRecord {
    /// <summary>
    /// Gets the component type used for rendering.
    /// </summary>
    Type ComponentType { get; }
    /// <summary>
    /// Gets the builder delegate used to render the component.
    /// </summary>
    Func<RenderTreeBuilder, int, IMdSyntaxNode, int> Builder { get; }
}
