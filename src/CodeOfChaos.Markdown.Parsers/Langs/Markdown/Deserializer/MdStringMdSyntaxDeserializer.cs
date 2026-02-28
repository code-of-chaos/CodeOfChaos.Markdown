// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Pooling;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.Extensions.Logging;
using System.Collections.Frozen;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MdStringMdSyntaxDeserializer(ILogger<MdStringMdSyntaxDeserializer> logger) : IMdStringMdSyntaxDeserializer {
    public FrozenDictionary<Type, IMarkdownSyntaxNodeVisitor> Deserializers { get; internal set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string DeserializeToString(IMdSyntaxTree tree) {
        StringBuilder builder = GlobalPools.StringBuilder.Get();
        MdStringDeserializerQueue queue = MdStringDeserializerQueuePool.Shared.Get();
        
        try {
            foreach (IMdSyntaxNode node in tree.VisitTopLevelNodes()) {
                queue.Enqueue(node, builder);

                while (queue.TryDequeue(out IMdSyntaxNode? dequeuedNode, out StringBuilder? dequeuedBuilder)) {
                    if (!Deserializers.TryGetValue(node.Type, out IMarkdownSyntaxNodeVisitor? deserializer)) {
                        logger.Error("No deserializer found for node type {NodeType}", node.Type);
                        continue;
                    }
                    
                    deserializer.Deserialize(queue, dequeuedNode, dequeuedBuilder);
                }
            }

            return builder.ToString();
        }
        finally {
            GlobalPools.StringBuilder.Return(builder);
            MdStringDeserializerQueuePool.Shared.Return(queue);
        }
    }
}
