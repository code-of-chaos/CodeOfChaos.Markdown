// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components;

namespace CodeOfChaos.Markdown.Parsers.Langs.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Serves as an abstract base class for Blazor components that handle rendering
/// of specific Markdown syntax nodes using a strongly-typed approach.
/// </summary>
/// <typeparam name="TSyntaxNode">The type of the Markdown syntax node to be visited and rendered.</typeparam>
/// <remarks>
/// This class integrates the rendering pipeline for Markdown syntax nodes into the
/// Blazor component model by leveraging dependency injection and cascading parameters.
/// </remarks>
public abstract class BaseMarkdownSyntaxNodeVisitor<TSyntaxNode> : ComponentBase, IBlazorSyntaxNodeVisitor<TSyntaxNode> where TSyntaxNode : class, IMdSyntaxNode {
    /// <summary>
    /// Gets or sets the syntax node being processed by the visitor.
    /// </summary>
    /// <remarks>
    /// Represents the core data structure associated with the current Blazor syntax node visitor,
    /// providing necessary information for rendering or processing markdown syntax nodes in a Blazor context.
    /// </remarks>
    [Parameter] public required TSyntaxNode SyntaxNode { get; set; }
    
    /// <summary>
    /// Provides an injectably configurable renderer responsible for transforming markdown syntax nodes
    /// into Blazor render fragments.
    /// </summary>
    /// <remarks>
    /// This property is used to facilitate the rendering of child components from syntax nodes by leveraging
    /// the methods defined in the <see cref="IBlazorMdComponentRenderer"/> interface, such as
    /// <c>RenderChildComponents</c>.
    /// </remarks>
    /// 
    [Inject] public IBlazorMdComponentRenderer ComponentConverter { get; set; } = null!;
    
    /// <summary>
    /// Provides contextual information for rendering markdown components in a Blazor application.
    /// </summary>
    /// <remarks>
    /// The RenderContext property is a cascading parameter used to manage and share rendering state, interactions,
    /// and contextual information required during the rendering of markdown syntax nodes.
    /// </remarks>
    [CascadingParameter] public MdRenderContext? RenderContext { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a render fragment that renders the child components of the current syntax node.
    /// </summary>
    /// <returns>A render fragment representing the child components of the syntax node.</returns>
    protected RenderFragment RenderChildContent() {
        return ComponentConverter.RenderChildComponents(SyntaxNode);
    }
}
