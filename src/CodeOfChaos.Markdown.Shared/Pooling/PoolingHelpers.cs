// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.ObjectPool;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Markdown.Pooling;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Factory helpers for creating shared object pools used by markdown processing.
/// </summary>
public static class PoolingHelpers {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates an object pool for resettable reference types.
    /// </summary>
    /// <typeparam name="T">The pooled object type.</typeparam>
    /// <param name="maxRetained">The maximum number of retained objects.</param>
    /// <returns>A configured object pool.</returns>
    public static ObjectPool<T> CreateResettablePool<T>(int maxRetained) where T : class, IResettable, new()
        => new DefaultObjectPool<T>(new ResettablePoolPolicy<T>(), maxRetained);

    /// <summary>
    /// Creates an object pool for stacks.
    /// </summary>
    /// <typeparam name="TItem">The stack item type.</typeparam>
    /// <param name="maxRetained">The maximum number of retained stacks.</param>
    /// <returns>A configured stack pool.</returns>
    public static ObjectPool<Stack<TItem>> CreateStackPool<TItem>(int maxRetained)  
        => new DefaultObjectPool<Stack<TItem>>(new StackPoolPolicy<Stack<TItem>,TItem>(), maxRetained);
}
