// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INodeDeserializerFragmentQueue {
    void Enqueue(string s);
    void Enqueue(char s);
    void Enqueue(ReadOnlySpan<char> line);
    
    void Enqueue(IMdSyntaxNode node);
    void EnqueueChildren(IMdSyntaxNode node);

    string ProcessAsStandaloneContent(IMdSyntaxNode node);
}
