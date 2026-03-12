// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A stack pool designed for efficiently reusing instances of the <see cref="NodeSerializerFragmentStack"/> class.
/// </summary>
/// <remarks>
/// This class provides a shared pool of <see cref="NodeSerializerFragmentStack"/> objects to minimize memory
/// allocations and improve performance, particularly in scenarios where multiple instances of stack-like data
/// structures are frequently required during Markdown serialization.
/// </remarks>
public class MdSyntaxFragmentStackPool {
    /// <summary>
    /// Provides access to a shared instance of the <see cref="MdSyntaxFragmentStackPool"/> class.
    /// This property allows the reuse of <see cref="NodeSerializerFragmentStack"/> instances
    /// across different components, optimizing memory and performance by leveraging object pooling.
    /// </summary>
    public static MdSyntaxFragmentStackPool Shared { get; } = new();

    /// <summary>
    /// Represents a pool for managing instances of <see cref="NodeSerializerFragmentStack"/>.
    /// </summary>
    /// <remarks>
    /// This pool is designed to optimize the reuse of <see cref="NodeSerializerFragmentStack"/> objects,
    /// reducing memory allocations and improving performance in scenarios involving frequent object creation
    /// and disposal. It leverages the capabilities of a resettable object pool to ensure that each
    /// instance returned to the pool is properly reset before reuse.
    /// </remarks>
    private ObjectPool<NodeSerializerFragmentStack> Pool { get; } = PoolingHelpers.CreateResettablePool<NodeSerializerFragmentStack>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Retrieves an instance of the NodeSerializerFragmentStack from the pool.
    /// This method gets a reusable NodeSerializerFragmentStack object
    /// from the internal object pool. The retrieved instance can be used
    /// for processing markdown syntax nodes and related operations.
    /// After use, it is recommended to return the instance back to the pool
    /// for reuse to optimize resource consumption.
    /// <returns>
    /// A NodeSerializerFragmentStack instance retrieved from the internal pool.
    /// </returns>
    public NodeSerializerFragmentStack Get() => Pool.Get();
    /// <summary>
    /// Returns a <see cref="NodeSerializerFragmentStack"/> instance to the object pool for reuse.
    /// This helps in reducing object allocation and garbage collection overhead by reusing objects.
    /// </summary>
    /// <param name="stack">
    /// The <see cref="NodeSerializerFragmentStack"/> instance to be returned to the pool.
    /// It should have been previously retrieved using the <see cref="Get"/> method.
    /// </param>
    public void Return(NodeSerializerFragmentStack stack) => Pool.Return(stack);
}
