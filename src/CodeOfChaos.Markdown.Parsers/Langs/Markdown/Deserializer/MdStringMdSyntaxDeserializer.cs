// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
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
[InjectableSingleton<IMdStringMdSyntaxDeserializer>]
public class MdStringMdSyntaxDeserializer(ILogger<MdStringMdSyntaxDeserializer> logger, IMarkdownConfig config) : IMdStringMdSyntaxDeserializer {
    public FrozenDictionary<Type, IMarkdownSyntaxNodeVisitor> Deserializers { get; } = config.MarkdownSyntaxNodeVisitors;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public string DeserializeToString(IMdSyntaxTree tree) {
        StringBuilder builder = GlobalPools.StringBuilder.Get();
        NodeDeserializerFragmentQueue queue = NodeDeserializerFragmentQueuePool.Shared.Get(this, builder);

        try {
            foreach (IMdSyntaxNode node in tree.VisitTopLevelNodes()) {
                queue.Enqueue(node);
                ProcessFragmentQueue(queue, builder);
            }

            return builder.ToString();
        }
        finally {
            GlobalPools.StringBuilder.Return(builder);
            NodeDeserializerFragmentQueuePool.Shared.Return(queue);
        }
    }
    
    public string DeserializeToString(IMdSyntaxNode node) {
        StringBuilder builder = GlobalPools.StringBuilder.Get();
        NodeDeserializerFragmentQueue queue = NodeDeserializerFragmentQueuePool.Shared.Get(this, builder);

        try {
            foreach (IMdSyntaxNode child in node.GetChildrenSpan()) {
                queue.Enqueue(child);
                ProcessFragmentQueue(queue, builder);
            }

            return builder.ToString();
        }
        finally {
            GlobalPools.StringBuilder.Return(builder);
            NodeDeserializerFragmentQueuePool.Shared.Return(queue);
        }
    }

    public string DeserializeToString(ReadOnlySpan<IMdSyntaxNode> nodes) {
        StringBuilder builder = GlobalPools.StringBuilder.Get();
        NodeDeserializerFragmentQueue queue = NodeDeserializerFragmentQueuePool.Shared.Get(this, builder);
        
        try {
            foreach (IMdSyntaxNode node in nodes) {
                queue.Enqueue(node);
                ProcessFragmentQueue(queue, builder);
            }
            
            return builder.ToString();
        }
        finally {
            GlobalPools.StringBuilder.Return(builder);
            NodeDeserializerFragmentQueuePool.Shared.Return(queue);
        }
    }

    private void ProcessFragmentQueue(NodeDeserializerFragmentQueue queue, StringBuilder builder) {
        while (queue.TryDequeue(out NodeDeserializerFragment fragment)) {
            switch (fragment) {
                case { Node: {} dequeuedNode }: {
                    if (!Deserializers.TryGetValue(dequeuedNode.Type, out IMarkdownSyntaxNodeVisitor? deserializer)) {
                        logger.Error("No deserializer found for node type {NodeType}", dequeuedNode.Type);
                        continue;
                    }

                    deserializer.Deserialize(queue, dequeuedNode);
                    break;
                }

                case { ContentCharacter: {} dequeuedCharacter }: {
                    builder.Append(dequeuedCharacter);
                    break;
                }

                case { ContentString: {} dequeuedString }: {
                    builder.Append(dequeuedString);
                    break;
                }

                case { ChildrenToProcessDirectly: {} parentNode }: {
                    // Process all children with a temporary queue to avoid queue ordering issues
                    // This ensures children content is written before any subsequent markers
                    NodeDeserializerFragmentQueue tempQueue = NodeDeserializerFragmentQueuePool.Shared.Get(this, builder);
                    try {
                        foreach (IMdSyntaxNode child in parentNode.GetChildrenSpan()) {
                            tempQueue.Enqueue(child);
                            ProcessFragmentQueue(tempQueue, builder);
                        }
                    }
                    finally {
                        NodeDeserializerFragmentQueuePool.Shared.Return(tempQueue);
                    }
                    break;
                }
            }


        }
    }
}
