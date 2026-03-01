// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INodeDeserializerFragmentQueue {
    void Enqueue(string? value);
    void Enqueue(char value);
    void Enqueue(ReadOnlySpan<char> value);
    
    void Enqueue(IMdSyntaxNode value);
    void EnqueueChildren(IMdSyntaxNode value);

    string ProcessAsStandaloneContent(IMdSyntaxNode node);
}
