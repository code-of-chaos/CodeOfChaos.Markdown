// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct NodeDeserializerFragment(
    IMdSyntaxNode? Node,
    string? ContentString,
    char? ContentCharacter
) {
    public static NodeDeserializerFragment AsNodeToBeProcessed(IMdSyntaxNode node)
        => new(node, null, null);
    
    public static NodeDeserializerFragment AsContentToBeProcessed(string content)
        => new(null, content, null);
    
    public static NodeDeserializerFragment AsCharacterToBeProcessed(char content)
        => new(null, null, content);
}
