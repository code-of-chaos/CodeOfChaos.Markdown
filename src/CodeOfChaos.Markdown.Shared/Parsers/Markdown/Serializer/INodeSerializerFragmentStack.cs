// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a fragment stack used while parsing markdown into syntax nodes.
/// </summary>
public interface INodeSerializerFragmentStack {
    /// <summary>
    /// Pushes multi-line match fragments into the stack.
    /// </summary>
    /// <param name="input">The markdown input.</param>
    /// <param name="parentNode">The parent node receiving generated nodes.</param>
    /// <param name="startIndex">The input index to start scanning from.</param>
    void PushMultiLineMatchesToStack(string input, IMdSyntaxNode parentNode, int startIndex = 0);
    /// <summary>
    /// Pushes single-line match fragments into the stack.
    /// </summary>
    /// <param name="input">The markdown input.</param>
    /// <param name="parentNode">The parent node receiving generated nodes.</param>
    void PushSingleLineMatchesToStack(string input, IMdSyntaxNode parentNode);
    
    /// <summary>
    /// Pushes an already processed child node onto the stack.
    /// </summary>
    /// <param name="parentNode">The parent node.</param>
    /// <param name="childNode">The processed child node.</param>
    void PushProcessedNodeToStack(IMdSyntaxNode parentNode, IMdSyntaxNode childNode);
}
