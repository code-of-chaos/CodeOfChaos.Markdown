// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMarkdownSyntaxNodeVisitor {
    ReadOnlySpan<char> SerializationTriggerCharacters { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    bool TryGetSerializationMatch(string input, [NotNullWhen(true)] out Match? match, int startPosition = 0);
    void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match);
    
    void Deserialize(INodeDeserializerFragmentQueue queue, IMdSyntaxNode node);
}
