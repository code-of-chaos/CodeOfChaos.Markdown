// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.Extensions.ObjectPool;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MdStringDeserializerQueue : IMdStringDeserializerQueue, IResettable {
    private readonly Queue<(IMdSyntaxNode, StringBuilder)> _queue = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryDequeue([NotNullWhen(true)] out IMdSyntaxNode? node, [NotNullWhen(true)] out StringBuilder? builder) {
        if (!_queue.TryDequeue(out (IMdSyntaxNode, StringBuilder) tuple)) {
            node = null;
            builder = null;
            return false;
        }
        (node, builder) = tuple;
        return true;
    }

    public void Enqueue(IMdSyntaxNode node, StringBuilder builder) 
        => _queue.Enqueue((node, builder));
    
    public void EnqueueChildren(IMdSyntaxNode node, StringBuilder builder) {
        foreach (IMdSyntaxNode child in node.GetChildrenSpan()) {
            Enqueue(child, builder);
        }
    }
    
    public bool TryReset() {
        _queue.Clear();
        return true;
    } 
}
