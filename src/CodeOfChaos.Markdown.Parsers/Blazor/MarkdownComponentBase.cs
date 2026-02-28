// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Markdown.Syntax;
using Microsoft.AspNetCore.Components;

namespace CodeOfChaos.Markdown.Parsers.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class MarkdownComponentBase<T> : ComponentBase
    where T : class, IMdSyntaxNode {
    [Parameter] public required T SyntaxNode { get; set; }
    [Inject] public IBlazorMdComponentConverter ComponentConverter { get; set; } = null!;
    [CascadingParameter] public MdRenderContext? RenderContext { get; set; }

    protected RenderFragment RenderChildContent() => ComponentConverter.RenderChildComponents(SyntaxNode);
}
