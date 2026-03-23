// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a queue used to build markdown output from syntax nodes and content fragments.
/// </summary>
public interface INodeDeserializerFragmentQueue {
    /// <summary>
    /// Enqueues string content.
    /// </summary>
    /// <param name="value">The content to enqueue.</param>
    void Enqueue(string? value);
    /// <summary>
    /// Enqueues a character.
    /// </summary>
    /// <param name="value">The character to enqueue.</param>
    void Enqueue(char value);
    /// <summary>
    /// Enqueues a repeated character sequence.
    /// </summary>
    /// <param name="value">The character to enqueue.</param>
    /// <param name="repeatCount">The number of repetitions.</param>
    void Enqueue(char value, int repeatCount);
    /// <summary>
    /// Enqueues span content.
    /// </summary>
    /// <param name="value">The content to enqueue.</param>
    void Enqueue(ReadOnlySpan<char> value);

    /// <summary>
    /// Enqueues a syntax node to be processed.
    /// </summary>
    /// <param name="value">The node to enqueue.</param>
    void Enqueue(IMdSyntaxNode value);
    /// <summary>
    /// Enqueues a marker to process the children of the given node directly.
    /// </summary>
    /// <param name="value">The node whose children should be processed.</param>
    void EnqueueChildren(IMdSyntaxNode value);

    /// <summary>
    /// Processes a node as standalone markdown content.
    /// </summary>
    /// <param name="node">The node to process.</param>
    /// <returns>The resulting markdown output.</returns>
    string ProcessAsStandaloneContent(IMdSyntaxNode node);
    /// <summary>
    /// Processes only the children of a node as standalone markdown content.
    /// </summary>
    /// <param name="node">The node whose children are processed.</param>
    /// <returns>The resulting markdown output.</returns>
    string ProcessChildrenAsStandaloneContent(IMdSyntaxNode node);
}
