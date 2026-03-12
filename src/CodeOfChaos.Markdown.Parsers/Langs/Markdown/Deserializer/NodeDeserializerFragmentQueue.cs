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
    
    
    /// <inheritdoc />
    public void Enqueue(string? value) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        if (value.IsNullOrEmpty()) return;

        if (_queue.IsEmpty()) {
            BuilderReference.Append(value);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsContentToBeProcessed(value);
        _queue.Enqueue(fragment);
    }
    
    /// <inheritdoc />
    public void Enqueue(char value) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        
        if (_queue.IsEmpty()) {
            BuilderReference.Append(value);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsCharacterToBeProcessed(value);
        _queue.Enqueue(fragment);
    }
    
    /// <inheritdoc />
    public void Enqueue(char value, int repeatCount) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        
        if (_queue.IsEmpty()) {
            BuilderReference.Append(value, repeatCount);
            return;
        }
        
        string stringValue = new(value, repeatCount);
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsContentToBeProcessed(stringValue);
        _queue.Enqueue(fragment);
    }
    
    /// <inheritdoc />
    public void Enqueue(ReadOnlySpan<char> value) {
        ArgumentNullException.ThrowIfNull(BuilderReference);
        
        if (_queue.IsEmpty()) {
            BuilderReference.Append(value);
            return;
        }
        
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsContentToBeProcessed(value.ToString());
        _queue.Enqueue(fragment);
    }

    /// <inheritdoc />
    public void Enqueue(IMdSyntaxNode node) {
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsNodeToBeProcessed(node);
        _queue.Enqueue(fragment);
    }

    /// <inheritdoc />
    public void EnqueueChildren(IMdSyntaxNode node) {
        NodeDeserializerFragment fragment = NodeDeserializerFragment.AsChildrenToProcessDirectly(node);
        _queue.Enqueue(fragment);
    }

    /// <inheritdoc />
    public string ProcessAsStandaloneContent(IMdSyntaxNode node) {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(DeserializerReference);
        return DeserializerReference.DeserializeToString(node);
    }

    /// <inheritdoc />
    public string ProcessChildrenAsStandaloneContent(IMdSyntaxNode node) {
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(DeserializerReference);
        return DeserializerReference.DeserializeToString(node.GetChildrenSpan());
    }

    public bool TryReset() {
        _queue.Clear();
        DeserializerReference = null;
        BuilderReference = null;
        return true;
    } 
}
