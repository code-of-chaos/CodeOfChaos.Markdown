// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MdSyntaxFragmentStackPool {
    public static MdSyntaxFragmentStackPool Shared { get; } = new();
    
    private ObjectPool<NodeSerializerFragmentStack> Pool { get; } = PoolingHelpers.CreateResettablePool<NodeSerializerFragmentStack>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public NodeSerializerFragmentStack Get() => Pool.Get();
    public void Return(NodeSerializerFragmentStack stack) => Pool.Return(stack);
}
