// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the root node of a markdown syntax tree.
/// </summary>
public interface IRootMdSyntaxNode : IMdSyntaxNode {
    /// <summary>
    /// Associates this root node with a syntax tree.
    /// </summary>
    /// <param name="mdSyntaxTree">The owning syntax tree.</param>
    /// <returns>The current root node instance.</returns>
    IRootMdSyntaxNode WithTreeReference(IMdSyntaxTree mdSyntaxTree);
}
