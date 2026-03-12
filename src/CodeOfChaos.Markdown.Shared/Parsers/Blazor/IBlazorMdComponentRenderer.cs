// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components;

namespace CodeOfChaos.Markdown.Parsers.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Renders markdown syntax nodes as Blazor render fragments.
/// </summary>
public interface IBlazorMdComponentRenderer {
    /// <summary>
    /// Renders a single syntax node.
    /// </summary>
    /// <param name="node">The node to render.</param>
    /// <returns>The render fragment.</returns>
    RenderFragment RenderComponent(IMdSyntaxNode node);
    /// <summary>
    /// Renders a single syntax node including debug metadata.
    /// </summary>
    /// <param name="node">The node to render.</param>
    /// <returns>The debug render fragment.</returns>
    RenderFragment RenderComponentDebug(IMdSyntaxNode node);
    
    /// <summary>
    /// Renders the child nodes of a parent node.
    /// </summary>
    /// <param name="node">The parent node.</param>
    /// <returns>The render fragment.</returns>
    RenderFragment RenderChildComponents(IMdSyntaxNode node);
    /// <summary>
    /// Renders root-level nodes.
    /// </summary>
    /// <param name="nodes">The root-level nodes.</param>
    /// <returns>The render fragment.</returns>
    RenderFragment RenderRootComponents(IEnumerable<IMdSyntaxNode> nodes);
    /// <summary>
    /// Renders root-level nodes while respecting skipped component configuration.
    /// </summary>
    /// <param name="nodes">The root-level nodes.</param>
    /// <returns>The render fragment.</returns>
    RenderFragment RenderRootComponentsWithSkipped(IEnumerable<IMdSyntaxNode> nodes);
}
