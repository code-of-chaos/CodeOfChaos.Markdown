// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CodeOfChaos.Markdown.Syntax;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public abstract class MdSyntaxNode<T>(int initialChildCount = 2) : IMdSyntaxNode
    where T : MdSyntaxNode<T>, new() {
    /// <inheritdoc />
    public Guid Id { get; } = Guid.CreateVersion7();// Not reset during TryReset, this is by design.

    /// <inheritdoc />
    public int ChildCount { get; private set; }

    /// <summary>
    /// Represents an array of child nodes associated with the current syntax node instance.
    /// This property provides access to child nodes, which can include elements such as
    /// headers, rows, or other nested objects depending on the specific type of syntax node.
    /// </summary>
    /// <remarks>
    /// The child nodes are initialized with a specified initial size and can be dynamically accessed
    /// or modified based on the operations performed on the parent syntax node. This property is
    /// crucial for enabling hierarchical relationships between syntax nodes within the Markdown syntax tree.
    /// </remarks>
    protected IMdSyntaxNode[] ChildNodes { get; private set; } = GetInitialChildNodeArray(initialChildCount);
    private readonly bool _isEmptyInitialized = GetEmptyInitializedState(initialChildCount);

    /// <inheritdoc />
    public int Depth { get; private set; }
    /// <inheritdoc />
    public IMdSyntaxNode? Parent { get; private set; }
    private static readonly Type TypeBacking = typeof(T);
    /// <inheritdoc />
    public Type Type => TypeBacking;

    /// <inheritdoc />
    public IMdSyntaxNodeModifier? Modifier { get; private set; }
    /// <inheritdoc />
    public IMdSyntaxTree? TreeReference { get; protected set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    private static IMdSyntaxNode[] GetInitialChildNodeArray(int initialChildCount) => initialChildCount <= 0
        ? Array.Empty<IMdSyntaxNode>()
        : ArrayPool<IMdSyntaxNode>.Shared.Rent(Math.Max(1, initialChildCount));

    private static bool GetEmptyInitializedState(int initialChildCount)
        => initialChildCount <= 0;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region GetChild(ren)
    // ReSharper disable once ConvertIfStatementToReturnStatement
    /// <inheritdoc />
    public ReadOnlySpan<IMdSyntaxNode> GetChildrenSpan() {
        if (ChildCount == 0) return ReadOnlySpan<IMdSyntaxNode>.Empty;

        return ChildNodes.AsSpan(0, ChildCount);
    }

    /// <inheritdoc />
    public IEnumerable<IMdSyntaxNode> GetChildren() {
        for (int i = 0; i < ChildCount; i++) {
            yield return ChildNodes[i];
        }
    }

    /// <inheritdoc />
    public IEnumerable<TChild> GetChildrenByType<TChild>() where TChild : IMdSyntaxNode {
        for (int i = 0; i < ChildCount; i++) {
            IMdSyntaxNode child = ChildNodes[i];
            if (child is not TChild casted) continue;

            yield return casted;
        }
    }

    /// <inheritdoc />
    public IMdSyntaxNode GetChildAt(int index)
        => ChildNodes[index];

    /// <inheritdoc />
    public bool TryGetChildAt(int index, [NotNullWhen(true)] out IMdSyntaxNode? childNode) {
        if (index < 0 || index >= ChildCount) {
            childNode = null;
            return false;
        }

        childNode = ChildNodes[index];
        return true;
    }

    /// <inheritdoc />
    public bool TryGetChildAt<TChild>(int index, [NotNullWhen(true)] out TChild? childNode) where TChild : IMdSyntaxNode {
        if (index < 0 || index >= ChildCount || ChildNodes[index] is not TChild casted) {
            childNode = default;
            return false;
        }

        childNode = casted;
        return true;
    }
    #endregion

    #region GetNextSibling(s)
    /// <inheritdoc />
    public bool TryGetNextSibling([NotNullWhen(true)] out IMdSyntaxNode? mdSyntaxNode) {
        mdSyntaxNode = null;
        if (Parent is null) return false;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = 0; i < span.Length; i++) {
            if (!ReferenceEquals(span[i], this)) continue;

            if (i + 1 >= span.Length) return false;

            mdSyntaxNode = span[i + 1];
            return true;
        }

        // We should never get here because we are within our own parent
        return false;
    }

    /// <inheritdoc />
    public bool TryGetNextSibling<TSibling>([NotNullWhen(true)] out TSibling? mdSyntaxNode) where TSibling : IMdSyntaxNode {
        mdSyntaxNode = default;
        if (!TryGetNextSibling(out IMdSyntaxNode? nextSibling)) return false;
        if (nextSibling is not TSibling casted) return false;
        mdSyntaxNode = casted;
        return true;
    }
    
    /// <inheritdoc />
    public bool NextSiblingIsTypeOf<TSibling>() where TSibling : IMdSyntaxNode {
        if (Parent is null) return false;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = 0; i < span.Length; i++) {
            if (!ReferenceEquals(span[i], this)) continue;

            if (i + 1 >= span.Length) return false;

            IMdSyntaxNode foundNode = span[i + 1];
            return foundNode is TSibling;
        }

        // We should never get here because we are within our own parent
        return false;
    }

    /// <inheritdoc />
    public bool HasNextSibling() {
        if (Parent is null) return false;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = 0; i < span.Length; i++) {
            if (!ReferenceEquals(span[i], this)) continue;

            return i + 1 < span.Length;
        }

        // We should never get here because we are within our own parent
        return false;
    }
    #endregion

    #region GetPreviousSibling(s)
    /// <inheritdoc />
    public bool TryGetPreviousSibling([NotNullWhen(true)] out IMdSyntaxNode? mdSyntaxNode) {
        mdSyntaxNode = null;
        if (Parent is null) return false;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = span.Length - 1; i >= 0; i--) {
            if (!ReferenceEquals(span[i], this)) continue;

            if (i - 1 < 0) return false;

            mdSyntaxNode = span[i - 1];
            return true;
        }

        // We should never get here because we are within our own parent
        return false;
    }

    /// <inheritdoc />
    public bool HasPreviousSibling() {
        if (Parent is null) return false;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = span.Length - 1; i >= 0; i--) {
            if (!ReferenceEquals(span[i], this)) continue;

            return i - 1 >= 0;
        }

        // We should never get here because we are within our own parent
        return false;
    }
    #endregion

    #region Index
    /// <inheritdoc />
    public int GetIndexAtParent() {
        if (Parent is null) return -1;

        ReadOnlySpan<IMdSyntaxNode> span = Parent.GetChildrenSpan();
        for (int i = 0; i < span.Length; i++) {
            if (!ReferenceEquals(span[i], this)) continue;

            return i;
        }

        return -1;
    }
    #endregion

    #region AddChild(ren)
    /// <inheritdoc />
    public void AddChildNode(IMdSyntaxNode childNode) {
        // Check if we need to resize
        EnsureChildNodeExpansionCapacity();
        childNode.WithParent(this);
        childNode.WithDepth(Depth + 1);

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        ChildNodes[ChildCount++] = childNode;
    }

    /// <inheritdoc />
    public TChild AddChildNode<TChild>(TChild childNode) where TChild : IMdSyntaxNode {
        // Check if we need to resize
        EnsureChildNodeExpansionCapacity();
        childNode.WithParent(this);
        childNode.WithDepth(Depth + 1);

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        ChildNodes[ChildCount++] = childNode;
        return childNode;
    }

    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

    /// <summary>
    /// Attempts to add a child node at the specified index within the current node's list of child nodes.
    /// </summary>
    /// <param name="index">The index at which the child node should be added. Must be within the valid range of current children indices.</param>
    /// <param name="childNode">The child node to add. Cannot be null and must implement <see cref="IMdSyntaxNode"/>.</param>
    /// <returns>
    /// <c>true</c> if the child node was successfully added at the specified index;
    /// otherwise, <c>false</c> if the index is invalid, the position is already occupied, or any other validation fails.
    /// </returns>
    protected bool TryAddChildNodeAtIndex(int index, IMdSyntaxNode childNode) {
        if (index < 0 || index > ChildNodes.Length - 1) return false;
        if (ChildNodes[index] is not null) return false;

        EnsureChildNodeExpansionCapacity();
        childNode.WithParent(this);
        childNode.WithDepth(Depth + 1);

        // ReSharper disable once ConvertTypeCheckPatternToNullCheck
        if (ChildNodes[index] is IMdSyntaxNode existingNode) {
            existingNode.ReturnToPool();
            ChildCount--;
        }

        ChildNodes[index] = childNode;
        ChildCount++;
        return true;
    }

    // ReSharper disable once InvertIf
    private void EnsureChildNodeExpansionCapacity() {
        int childNodeArrayLength = ChildNodes.Length;
        if (childNodeArrayLength == 0 && _isEmptyInitialized) {
            // We are initializing with an empty array-shared object, so we need to initialize it from the pool, else we won't be able to return it to a pool
            ChildNodes = ArrayPool<IMdSyntaxNode>.Shared.Rent(2);
            return;
        }

        if (ChildCount + 1 <= childNodeArrayLength) return;

        int newSize = (childNodeArrayLength + 1) * 2;
        IMdSyntaxNode[] newArray = ArrayPool<IMdSyntaxNode>.Shared.Rent(newSize);
        Array.Copy(ChildNodes, newArray, ChildCount);

        ArrayPool<IMdSyntaxNode>.Shared.Return(ChildNodes);
        ChildNodes = newArray;
    }
    #endregion

    #region RemoveChild(ren)
    /// <inheritdoc />
    public bool RemoveChildAt(int index) {
        if (index < 0 || index >= ChildCount) return false;

        IMdSyntaxNode removed = ChildNodes[index];

        int moveCount = ChildCount - index - 1;
        if (moveCount > 0) {
            Array.Copy(ChildNodes, index + 1, ChildNodes, index, moveCount);
        }

        ChildNodes[--ChildCount] = null!;
        TreeReference?.ClearCaches();

        removed.ReturnToPool();

        return true;
    }

    /// <inheritdoc />
    public bool RemoveChild(IMdSyntaxNode childNode) {
        for (int i = 0; i < ChildCount; i++) {
            if (!ReferenceEquals(ChildNodes[i], childNode)) continue;

            return RemoveChildAt(i);
        }

        return false;
    }
    #endregion

    #region With...
    /// <inheritdoc />
    public IMdSyntaxNode WithText(string content) {
        if (ChildNodes.LastOrDefault() is not TextMdSyntaxNode lastNode) {
            TextMdSyntaxNode newNode = MdSyntaxNodePool<TextMdSyntaxNode>.Shared.Get();
            newNode.WithContent(content);
            AddChildNode(newNode);
            return this;
        }

        int contentLength = lastNode.Content.Length;
        int length = contentLength + content.Length;
        lastNode.WithContent(string.Create(
            length,
            (contentLength, OriginalContent: lastNode.Content, NewContent: content),
            action: static (span, state) => {
                state.OriginalContent.AsSpan().CopyTo(span);
                state.NewContent.AsSpan().CopyTo(span[state.contentLength..]);
            }));

        return this;
    }

    /// <inheritdoc />
    public IMdSyntaxNode WithParent(IMdSyntaxNode parent) {
        Parent = parent;
        TreeReference = parent.TreeReference;
        TreeReference?.ClearCaches();

        return this;
    }

    /// <inheritdoc />
    public IMdSyntaxNode WithDepth(int depth) {
        Depth = depth;
        if (ChildCount == 0) return this;

        foreach (IMdSyntaxNode node in GetChildrenSpan()) {
            node.WithDepth(depth + 1);
        }

        return this;
    }

    /// <inheritdoc />
    public IMdSyntaxNode WithModifier(IMdSyntaxNodeModifier modifier) {
        Modifier?.ReturnToPool();
        Modifier = modifier;
        return this;
    }

    /// <inheritdoc />
    public IMdSyntaxNode WithChild<TChild>(TChild child) where TChild : IMdSyntaxNode {
        AddChildNode(child);
        return this;
    }
    #endregion

    #region ReturnToPool and Cleanup
    void IMdSyntaxNode.ReturnToPool() => MdSyntaxNodePool<T>.Shared.Return(Unsafe.As<T>(this));

    /// <inheritdoc />
    public virtual bool TryReset() {
        if (ChildNodes.Length > 0) {
            ArrayPool<IMdSyntaxNode>.Shared.Return(ChildNodes, true);
            ChildNodes = GetInitialChildNodeArray(initialChildCount);
        }

        ChildCount = 0;
        Depth = 0;
        Parent = null;
        TreeReference = null;

        // ReSharper disable once InvertIf
        if (Modifier is not null) {
            MdSyntaxNodeModifier.Pool.Return(Unsafe.As<MdSyntaxNodeModifier>(Modifier));
            Modifier = null;
        }

        return true;
    }
    #endregion

    #region Equality
    // ReSharper disable once NonReadonlyMemberInGetHashCode
    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Id, ChildCount);

    /// <inheritdoc />
    public bool Equals(IMdSyntaxNode? other) => Equals(other as T);
    /// <inheritdoc />
    public override bool Equals(object? other) => Equals(other as T);

    /// <summary>
    /// Determines whether the specified <typeparamref name="T"/> instance is equal to the current instance.
    /// </summary>
    /// <param name="other">The instance of type <typeparamref name="T"/> to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true"/> if the specified instance is equal to the current instance; otherwise, <see langword="false"/>.
    /// </returns>
    protected virtual bool Equals([NotNullWhen(true)] T? other) {
        if (other is null) return false;
        if (ChildCount != other.ChildCount) return false;

        ReadOnlySpan<IMdSyntaxNode> span = other.GetChildrenSpan();
        for (int i = 0; i < ChildCount; i++) {
            if (!ChildNodes[i].Equals(span[i])) return false;
        }

        return Depth == other.Depth
            && (Parent is not null || other.Parent is null)
            && (Parent is null || other.Parent is not null)
            && Type == other.Type
            && (Modifier is null && other.Modifier is null
                || Modifier != null && Modifier.Equals(other.Modifier)
            );

    }
    #endregion

    #region ToString
    /// <inheritdoc />
    public virtual string ToDebugString() => GetType().Name;

    /// <inheritdoc />
    public override string ToString() => ToDebugString();
    #endregion
}
