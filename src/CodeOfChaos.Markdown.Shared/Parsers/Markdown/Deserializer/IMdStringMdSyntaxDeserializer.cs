// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts syntax nodes and trees into markdown text.
/// </summary>
public interface IMdStringMdSyntaxDeserializer {
    /// <summary>
    /// Serializes a syntax tree to markdown text.
    /// </summary>
    /// <param name="tree">The tree to serialize.</param>
    /// <returns>The markdown output.</returns>
    string DeserializeToString(IMdSyntaxTree tree);
    /// <summary>
    /// Serializes the children of a node to markdown text.
    /// </summary>
    /// <param name="node">The node whose children are serialized.</param>
    /// <returns>The markdown output.</returns>
    string DeserializeToString(IMdSyntaxNode node);
    /// <summary>
    /// Serializes a node span to markdown text.
    /// </summary>
    /// <param name="nodes">The nodes to serialize.</param>
    /// <returns>The markdown output.</returns>
    string DeserializeToString(ReadOnlySpan<IMdSyntaxNode> nodes);
}
