// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Serializes and deserializes a specific syntax node type to and from JSON.
/// </summary>
public interface IJsonSyntaxNodeVisitor {
    /// <summary>
    /// Writes node-specific JSON properties.
    /// </summary>
    /// <param name="node">The node to serialize.</param>
    /// <param name="writer">The target JSON writer.</param>
    void DeserializeToJson(IMdSyntaxNode node, Utf8JsonWriter writer);
    /// <summary>
    /// Creates a syntax node from a JSON element and attaches it to a parent node.
    /// </summary>
    /// <param name="element">The source JSON element.</param>
    /// <param name="parentNode">The parent node in the tree.</param>
    /// <returns>The created syntax node.</returns>
    IMdSyntaxNode SerializeToNode(JsonElement element, IMdSyntaxNode parentNode);
}

// ReSharper disable once UnusedTypeParameter
public interface IJsonSyntaxNodeVisitor<TSyntaxNode> : IJsonSyntaxNodeVisitor where TSyntaxNode : class, IMdSyntaxNode;
