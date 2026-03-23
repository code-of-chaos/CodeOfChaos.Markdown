// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Blazor;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Marker interface for Blazor components that consume a strongly typed markdown syntax node.
/// </summary>
/// <typeparam name="TSyntaxNode">The syntax node type.</typeparam>
public interface IBlazorSyntaxNodeVisitor<TSyntaxNode> where TSyntaxNode : class, IMdSyntaxNode {
    /// <summary>
    /// Gets or sets the syntax node instance bound to the component.
    /// </summary>
    TSyntaxNode SyntaxNode { get; set; }
}
