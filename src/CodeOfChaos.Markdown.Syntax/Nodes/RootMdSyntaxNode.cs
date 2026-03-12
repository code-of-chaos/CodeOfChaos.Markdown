// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents the root syntax node in a Markdown syntax tree structure.
/// This class serves as the entry point for a hierarchical syntax node model
/// and implements the <see cref="IRootMdSyntaxNode"/> interface.
/// </summary>
/// <remarks>
/// The <see cref="RootMdSyntaxNode"/> is specifically designed to serve
/// as the root node of a Markdown syntax tree. It contains methods for
/// associating the tree structure with the node and interacts with its
/// child elements to manage syntax representation.
/// </remarks>
public sealed class RootMdSyntaxNode : MdSyntaxNode<RootMdSyntaxNode>, IRootMdSyntaxNode {
    /// <inheritdoc />
    public IRootMdSyntaxNode WithTreeReference(IMdSyntaxTree mdSyntaxTree) {
        TreeReference = mdSyntaxTree;
        return this;
    }
}
