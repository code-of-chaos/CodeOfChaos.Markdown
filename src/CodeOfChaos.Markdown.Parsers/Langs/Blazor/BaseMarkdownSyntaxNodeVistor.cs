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
public abstract class BaseMarkdownSyntaxNodeVisitor<TSyntaxNode> : ComponentBase, IBlazorSyntaxNodeVisitor<TSyntaxNode> where TSyntaxNode : class, IMdSyntaxNode {
    [Parameter] public required TSyntaxNode SyntaxNode { get; set; }
    
    [Inject] public IBlazorMdComponentConverter ComponentConverter { get; set; } = null!;
    [CascadingParameter] public MdRenderContext? RenderContext { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected RenderFragment RenderChildContent() => ComponentConverter.RenderChildComponents(SyntaxNode);
}
