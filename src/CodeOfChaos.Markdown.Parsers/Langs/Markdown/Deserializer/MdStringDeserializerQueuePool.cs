// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MdStringDeserializerQueuePool {
    public static MdStringDeserializerQueuePool Shared { get; } = new();
    
    private ObjectPool<MdStringDeserializerQueue> Pool { get; } = PoolingHelpers.CreateResettablePool<MdStringDeserializerQueue>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public MdStringDeserializerQueue Get() => Pool.Get();
    public void Return(MdStringDeserializerQueue stack) => Pool.Return(stack);
    
}
