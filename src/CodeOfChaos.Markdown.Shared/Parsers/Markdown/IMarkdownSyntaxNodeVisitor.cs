// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines serialization and deserialization behavior for a markdown syntax node type.
/// </summary>
public interface IMarkdownSyntaxNodeVisitor {
    /// <summary>
    /// Gets characters that can trigger this visitor during single-line scanning.
    /// </summary>
    ReadOnlySpan<char> SerializationTriggerCharacters { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Tries to find a markdown match handled by this visitor.
    /// </summary>
    /// <param name="input">The markdown input.</param>
    /// <param name="match">The resulting match when successful.</param>
    /// <param name="startPosition">The position to start scanning from.</param>
    /// <returns><see langword="true" /> when a match is found; otherwise <see langword="false" />.</returns>
    bool TryGetSerializationMatch(string input, [NotNullWhen(true)] out Match? match, int startPosition = 0);
    /// <summary>
    /// Converts a regex match into syntax-tree fragments.
    /// </summary>
    /// <param name="stack">The fragment stack to write into.</param>
    /// <param name="parentNode">The parent node receiving produced nodes.</param>
    /// <param name="match">The matched markdown segment.</param>
    void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match);
    
    /// <summary>
    /// Writes markdown output fragments for a syntax node.
    /// </summary>
    /// <param name="queue">The output fragment queue.</param>
    /// <param name="node">The source node.</param>
    void Deserialize(INodeDeserializerFragmentQueue queue, IMdSyntaxNode node);
}

// ReSharper disable once UnusedTypeParameter
/// <summary>
/// Marks a markdown visitor that targets a specific syntax-node type.
/// </summary>
/// <typeparam name="TSyntaxNode">The syntax-node type handled by the visitor.</typeparam>
public interface IMarkdownSyntaxNodeVisitor<TSyntaxNode> : IMarkdownSyntaxNodeVisitor where TSyntaxNode : class, IMdSyntaxNode;
