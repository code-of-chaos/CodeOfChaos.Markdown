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
/// Represents a thread-safe pool for managing the reuse of markdown syntax node objects
/// that implement the <see cref="IMdSyntaxNode"/> interface. This pool reduces memory
/// allocation overhead by recycling objects of the specified type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of objects managed by the pool.
/// The type must be a class, implement the <see cref="IMdSyntaxNode"/> interface, and have a parameterless constructor.
/// </typeparam>
public class MdSyntaxNodePool<T> where T : class, IMdSyntaxNode, new() {
    /// <summary>
    /// A singleton instance of the <see cref="MdSyntaxNodePool{T}"/> class, providing a shared pool
    /// for managing and reusing instances of <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The <c>Shared</c> property ensures that there is a single, globally accessible pool instance
    /// for <typeparamref name="T"/>. It helps reduce allocations and improve performance by reusing
    /// objects instead of creating new ones each time they are needed.
    /// </remarks>
    public static MdSyntaxNodePool<T> Shared { get; } = new();

    private static ObjectPool<T> Pool { get; } = PoolingHelpers.CreateResettablePool<T>(64);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Retrieves an instance of type T from the object pool.
    /// The retrieved instance is intended to be reset and reused,
    /// minimizing memory allocations and improving performance.
    /// <returns>
    /// An instance of type T from the object pool. If no instance is currently available,
    /// a new one will be created and returned.
    /// </returns>
    public T Get() => Pool.Get();
    
    /// <summary>
    /// Returns an <see cref="IMdSyntaxNode"/> instance to the object pool for reuse.
    /// This helps improve performance by reducing the overhead of memory allocation and garbage collection.
    /// </summary>
    /// <param name="node">
    /// The <typeparamref name="T"/> instance to be returned to the pool.
    /// It should implement the <see cref="IMdSyntaxNode"/> interface and must support reset operations.
    /// </param>
    public void Return(T node) => Pool.Return(node);
}
