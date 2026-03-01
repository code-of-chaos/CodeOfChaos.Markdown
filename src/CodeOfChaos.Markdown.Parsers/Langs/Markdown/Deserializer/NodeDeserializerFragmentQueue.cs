// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class NodeDeserializerFragmentQueue : INodeDeserializerFragmentQueue, IResettable {
    private readonly Queue<NodeDeserializerFragment> _queue = new();
    public IMdStringMdSyntaxDeserializer? DeserializerReference { get; set; }
    public StringBuilder? BuilderReference { get; set; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryDequeue(out NodeDeserializerFragment fragment) 
        => _queue.TryDequeue(out fragment);
    
    
    public void Enqueue(string s) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        if (s.IsNullOrEmpty()) return;

        if (_queue.IsEmpty()) {
            BuilderReference.Append(s);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsContentToBeProcessed(s);
        _queue.Enqueue(fragment);
    }
    
    public void Enqueue(char s) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        
        if (_queue.IsEmpty()) {
            BuilderReference.Append(s);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsCharacterToBeProcessed(s);
        _queue.Enqueue(fragment);
    }
    public void Enqueue(ReadOnlySpan<char> line) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        
        if (_queue.IsEmpty()) {
            BuilderReference.Append(line);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsContentToBeProcessed(line.ToString());
        _queue.Enqueue(fragment);
    }

    public void Enqueue(IMdSyntaxNode node) {
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsNodeToBeProcessed(node);
        _queue.Enqueue(fragment);
    }

    public void EnqueueChildren(IMdSyntaxNode node) {
        foreach (IMdSyntaxNode child in node.GetChildrenSpan()) {
            NodeDeserializerFragment fragment = NodeDeserializerFragment.AsNodeToBeProcessed(child);
            _queue.Enqueue(fragment);
        }
    }

    public string ProcessAsStandaloneContent(IMdSyntaxNode node) {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(DeserializerReference);
        return DeserializerReference.DeserializeToString(node);
    }
    
    public bool TryReset() {
        _queue.Clear();
        DeserializerReference = null;
        BuilderReference = null;
        return true;
    } 
}
