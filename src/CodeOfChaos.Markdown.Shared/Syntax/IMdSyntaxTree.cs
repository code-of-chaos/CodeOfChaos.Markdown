// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a markdown syntax tree rooted at <see cref="RootNode" />.
/// </summary>
public interface IMdSyntaxTree : IEquatable<IMdSyntaxTree> {
    /// <summary>
    /// Gets the root node.
    /// </summary>
    IRootMdSyntaxNode RootNode { get; }
    
    /// <summary>
    /// Tries to get cached nodes assignable to <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The node type to retrieve.</typeparam>
    /// <param name="nodes">The cached nodes when found.</param>
    /// <returns><see langword="true" /> when cache data exists; otherwise <see langword="false" />.</returns>
    bool TryGetCachedChildrenByType<T>([NotNullWhen(true)] out IEnumerable<T>? nodes) where T : IMdSyntaxNode;
    /// <summary>
    /// Tries to get cached nodes assignable to a runtime type.
    /// </summary>
    /// <param name="type">The node type to retrieve.</param>
    /// <param name="nodes">The cached nodes when found.</param>
    /// <returns><see langword="true" /> when cache data exists; otherwise <see langword="false" />.</returns>
    bool TryGetCachedChildrenByType(Type type, [NotNullWhen(true)] out IEnumerable<IMdSyntaxNode>? nodes);
    
    /// <summary>
    /// Enumerates top-level nodes.
    /// </summary>
    /// <returns>The top-level nodes.</returns>
    IEnumerable<IMdSyntaxNode> VisitTopLevelNodes();
    /// <summary>
    /// Enumerates nodes in breadth-first order.
    /// </summary>
    /// <returns>The nodes in traversal order.</returns>
    IEnumerable<IMdSyntaxNode> VisitNodesBreadthFirst();
    /// <summary>
    /// Enumerates nodes from deepest descendants towards the root level.
    /// </summary>
    /// <returns>The nodes in traversal order.</returns>
    IEnumerable<IMdSyntaxNode> VisitNodesDeepestFirst();

    /// <summary>
    /// Clears internal traversal caches.
    /// </summary>
    void ClearCaches();

    /// <summary>
    /// Gets the total node count excluding the root.
    /// </summary>
    /// <returns>The number of nodes in the tree.</returns>
    int GetCount();
}
