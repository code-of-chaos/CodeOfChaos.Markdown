// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace CodeOfChaos.Markdown.Pooling;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
internal static class GlobalPools {
    public static ObjectPool<StringBuilder> StringBuilder { get; } = new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
    public static ObjectPool<Stack<Range>> RangeStack { get; } = PoolingHelpers.CreateStackPool<Range>(16);
}
