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
public class NodeDeserializerFragmentQueuePool {
    public static NodeDeserializerFragmentQueuePool Shared { get; } = new();
    
    private ObjectPool<NodeDeserializerFragmentQueue> Pool { get; } = PoolingHelpers.CreateResettablePool<NodeDeserializerFragmentQueue>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public NodeDeserializerFragmentQueue Get(IMdStringMdSyntaxDeserializer deserializer, StringBuilder builder) {
        NodeDeserializerFragmentQueue queue = Pool.Get();
        queue.DeserializerReference = deserializer;
        queue.BuilderReference = builder;
        return queue;
    }
    public void Return(NodeDeserializerFragmentQueue queue) => Pool.Return(queue);
    
}
