// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a node in a markdown syntax tree.
/// </summary>
public interface IMdSyntaxNode : IResettable, IEquatable<IMdSyntaxNode>{
    /// <summary>
    /// Gets the node identifier.
    /// </summary>
    Guid Id { get; }
    /// <summary>
    /// Gets the parent node, if any.
    /// </summary>
    IMdSyntaxNode? Parent { get; }
    /// <summary>
    /// Gets the number of direct children.
    /// </summary>
    int ChildCount { get; }
    /// <summary>
    /// Gets the depth in the syntax tree.
    /// </summary>
    int Depth { get; }
    /// <summary>
    /// Gets the runtime node type.
    /// </summary>
    Type Type { get; }
    /// <summary>
    /// Gets the optional node modifier.
    /// </summary>
    IMdSyntaxNodeModifier? Modifier { get; }
    /// <summary>
    /// Gets the owning tree reference when available.
    /// </summary>
    IMdSyntaxTree? TreeReference { get; }

    /// <summary>
    /// Gets children as a span.
    /// </summary>
    ReadOnlySpan<IMdSyntaxNode> GetChildrenSpan();
    /// <summary>
    /// Enumerates direct children.
    /// </summary>
    IEnumerable<IMdSyntaxNode> GetChildren();
    /// <summary>
    /// Enumerates direct children assignable to <typeparamref name="TChild" />.
    /// </summary>
    IEnumerable<TChild> GetChildrenByType<TChild>() where TChild : IMdSyntaxNode;

    /// <summary>
    /// Gets a child at an index.
    /// </summary>
    IMdSyntaxNode GetChildAt(int index);
    /// <summary>
    /// Tries to get a child at an index.
    /// </summary>
    bool TryGetChildAt(int index, [NotNullWhen(true)] out IMdSyntaxNode? childNode);
    /// <summary>
    /// Tries to get a typed child at an index.
    /// </summary>
    bool TryGetChildAt<TChild>(int index, [NotNullWhen(true)] out TChild? childNode) where TChild : IMdSyntaxNode;

    /// <summary>
    /// Tries to get the next sibling.
    /// </summary>
    bool TryGetNextSibling([NotNullWhen(true)] out IMdSyntaxNode? mdSyntaxNode);
    /// <summary>
    /// Tries to get the next sibling as a specific type.
    /// </summary>
    bool TryGetNextSibling<TChild>([NotNullWhen(true)] out TChild? mdSyntaxNode) where TChild : IMdSyntaxNode;
    /// <summary>
    /// Determines whether the next sibling is of the specified type.
    /// </summary>
    bool NextSiblingIsTypeOf<TSibling>() where TSibling : IMdSyntaxNode;
    /// <summary>
    /// Determines whether a next sibling exists.
    /// </summary>
    bool HasNextSibling();
    
    /// <summary>
    /// Tries to get the previous sibling.
    /// </summary>
    bool TryGetPreviousSibling([NotNullWhen(true)] out IMdSyntaxNode? mdSyntaxNode);
    /// <summary>
    /// Determines whether a previous sibling exists.
    /// </summary>
    bool HasPreviousSibling();
    
    /// <summary>
    /// Gets the node index at its parent, or <c>-1</c> when unavailable.
    /// </summary>
    int GetIndexAtParent();
    
    /// <summary>
    /// Returns a simple string representation of the node for tree visualization.
    /// </summary>
    string ToDebugString();
    
    /// <summary>
    /// Adds a child node.
    /// </summary>
    void AddChildNode(IMdSyntaxNode childNode);
    /// <summary>
    /// Adds a child node and returns it.
    /// </summary>
    TChild AddChildNode<TChild>(TChild childNode) where TChild : IMdSyntaxNode;
    
    /// <summary>
    /// Removes a child at the specified index.
    /// </summary>
    bool RemoveChildAt(int index);
    /// <summary>
    /// Removes a specific child node.
    /// </summary>
    bool RemoveChild(IMdSyntaxNode childNode);

    /// <summary>
    /// Updates depth for this node and descendants.
    /// </summary>
    IMdSyntaxNode WithDepth(int depth);
    /// <summary>
    /// Appends text content to this node.
    /// </summary>
    IMdSyntaxNode WithText(string content);
    /// <summary>
    /// Sets the parent node reference.
    /// </summary>
    IMdSyntaxNode WithParent(IMdSyntaxNode parent);
    /// <summary>
    /// Sets a node modifier.
    /// </summary>
    IMdSyntaxNode WithModifier(IMdSyntaxNodeModifier modifier);
    /// <summary>
    /// Adds a child node and returns this node.
    /// </summary>
    IMdSyntaxNode WithChild<TChild>(TChild child) where TChild : IMdSyntaxNode;
    
    /// <summary>
    /// Returns this node instance to its pool.
    /// </summary>
    internal void ReturnToPool();
}
