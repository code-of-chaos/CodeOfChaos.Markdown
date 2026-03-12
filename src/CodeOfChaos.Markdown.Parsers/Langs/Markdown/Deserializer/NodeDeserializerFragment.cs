// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a queued output fragment for markdown deserialization.
/// </summary>
/// <param name="Node">A node to emit directly.</param>
/// <param name="ContentString">A text fragment to emit.</param>
/// <param name="ContentCharacter">A single character to emit.</param>
/// <param name="ChildrenToProcessDirectly">A node whose children should be emitted directly.</param>
public readonly record struct NodeDeserializerFragment(
    IMdSyntaxNode? Node,
    string? ContentString,
    char? ContentCharacter,
    IMdSyntaxNode? ChildrenToProcessDirectly
) {
    public static NodeDeserializerFragment AsNodeToBeProcessed(IMdSyntaxNode node)
        => new(node, null, null, null);

    public static NodeDeserializerFragment AsContentToBeProcessed(string content)
        => new(null, content, null, null);

    public static NodeDeserializerFragment AsCharacterToBeProcessed(char content)
        => new(null, null, content, null);

    public static NodeDeserializerFragment AsChildrenToProcessDirectly(IMdSyntaxNode parentNode)
        => new(null, null, null, parentNode);
}
