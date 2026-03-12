// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Manages a pool of <see cref="NodeDeserializerFragmentQueue"/> objects for efficient reuse.
/// </summary>
/// <remarks>
/// This pool is used to minimize memory allocations by maintaining and reusing
/// instances of <see cref="NodeDeserializerFragmentQueue"/> during Markdown deserialization operations.
/// It leverages a resettable object pool to ensure that returned instances are reset to their initial state
/// and prepared for later use.
/// </remarks>
public class NodeDeserializerFragmentQueuePool {
    /// <summary>
    /// Represents a shared instance of the <see cref="NodeDeserializerFragmentQueuePool"/> class.
    /// This singleton instance is used to manage reusable pools of <see cref="NodeDeserializerFragmentQueue"/> objects,
    /// ensuring efficient resource utilization and reducing runtime memory allocations.
    /// </summary>
    public static NodeDeserializerFragmentQueuePool Shared { get; } = new();

    /// <summary>
    /// Represents an object pool for managing <see cref="NodeDeserializerFragmentQueue"/> instances.
    /// </summary>
    /// <remarks>
    /// The pool provides a mechanism to efficiently reuse instances of <see cref="NodeDeserializerFragmentQueue"/> in order to
    /// improve performance and reduce memory allocation overhead. Internally, a fixed-size object pool is maintained
    /// and instances can be rented or returned as needed.
    /// </remarks>
    private ObjectPool<NodeDeserializerFragmentQueue> Pool { get; } = PoolingHelpers.CreateResettablePool<NodeDeserializerFragmentQueue>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Retrieves a <c>NodeDeserializerFragmentQueue</c> instance from the internal pool,
    /// initializes it with the provided deserializer and string builder, and returns it.
    /// </summary>
    /// <param name="deserializer">The deserializer instance to associate with the queue.</param>
    /// <param name="builder">The string builder to associate with the queue.</param>
    /// <returns>A <c>NodeDeserializerFragmentQueue</c> instance configured with the specified deserializer and builder.</returns>
    public NodeDeserializerFragmentQueue Get(IMdStringMdSyntaxDeserializer deserializer, StringBuilder builder) {
        NodeDeserializerFragmentQueue queue = Pool.Get();
        queue.DeserializerReference = deserializer;
        queue.BuilderReference = builder;
        return queue;
    }

    /// <summary>
    /// Returns a <see cref="NodeDeserializerFragmentQueue"/> instance back to the pool for reuse.
    /// </summary>
    /// <param name="queue">
    /// The <see cref="NodeDeserializerFragmentQueue"/> instance to return to the pool. Must not be null.
    /// </param>
    public void Return(NodeDeserializerFragmentQueue queue) => Pool.Return(queue);

}
