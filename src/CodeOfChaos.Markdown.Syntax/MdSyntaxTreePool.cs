// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Markdown.Syntax;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a pool for managing reusable instances of <see cref="MdSyntaxTree"/> objects.
/// The pool is designed to optimize memory usage and improve performance by reusing instances
/// of syntax trees instead of creating new ones repeatedly.
/// </summary>
public class MdSyntaxTreePool {
    /// <summary>
    /// A shared, globally accessible instance of the <see cref="MdSyntaxTreePool"/> class,
    /// designed to manage and provide reusable instances of <see cref="MdSyntaxTree"/> objects.
    /// </summary>
    /// <remarks>
    /// The <see cref="Shared"/> property centralizes access to a single instance of the pool,
    /// which helps mitigate frequent object allocations by reusing instances of <see cref="MdSyntaxTree"/>.
    /// This instance is thread-safe and ensures efficient resource management when
    /// working with Markdown syntax trees in the application.
    /// </remarks>
    public static MdSyntaxTreePool Shared { get; } = new();
    
    private ObjectPool<MdSyntaxTree> Pool { get; } = PoolingHelpers.CreateResettablePool<MdSyntaxTree>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves the value associated with the specified key from the collection.
    /// </summary>
    /// <returns>
    /// The value associated with the specified key if the key exists in the collection;
    /// otherwise, the default value for the type of the value.
    /// </returns>
    public IMdSyntaxTree Get() => Pool.Get();
    
    /// <summary>
    /// Returns an instance of <see cref="MdSyntaxTree"/> to the object pool for reuse.
    /// </summary>
    /// <param name="tree">
    /// The <see cref="MdSyntaxTree"/> instance to be returned
    /// to the pool. Must not be null and must be of type <see cref="MdSyntaxTree"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided tree is not of type <see cref="MdSyntaxTree"/>.
    /// </exception>
    public void Return(MdSyntaxTree tree) => Pool.Return(tree);
    
    /// <summary>
    /// Returns the specified <see cref="IMdSyntaxTree"/> instance back to the pool for reuse.
    /// </summary>
    /// <param name="tree">
    /// The instance of <see cref="IMdSyntaxTree"/> to return. Must be of type <see cref="MdSyntaxTree"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided <paramref name="tree"/> is not of type <see cref="MdSyntaxTree"/>.
    /// </exception>
    public void Return(IMdSyntaxTree tree) => Pool.Return(tree as MdSyntaxTree ?? throw new ArgumentException("Tree must be of type MdSyntaxTree", nameof(tree)));
}
