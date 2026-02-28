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
public interface IMdStringDeserializerQueue {
    bool TryDequeue([NotNullWhen(true)] out IMdSyntaxNode? node, [NotNullWhen(true)] out StringBuilder? builder);
    void Enqueue(IMdSyntaxNode node, StringBuilder builder);
    void EnqueueChildren(IMdSyntaxNode node, StringBuilder builder);
}
