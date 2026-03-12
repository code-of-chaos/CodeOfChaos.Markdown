// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Buffers;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts markdown text into syntax trees using configured markdown visitors.
/// </summary>
public interface IMdStringMdSyntaxSerializer {
    /// <summary>
    /// Gets trigger characters used to quickly locate possible single-line matches.
    /// </summary>
    SearchValues<char> SingleLineTriggerSearchValues { get; }
    
    /// <summary>
    /// Gets the ASCII lookup table for single-line visitors.
    /// </summary>
    ImmutableArray<IMarkdownSyntaxNodeVisitor>[] SingleLineLookup { get; init; }
    /// <summary>
    /// Gets the non-ASCII lookup table for single-line visitors.
    /// </summary>
    ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> SingleLineNonAsciiLookup { get; init; }

    /// <summary>
    /// Gets the ASCII lookup table for multi-line visitors.
    /// </summary>
    ImmutableArray<IMarkdownSyntaxNodeVisitor>[] MultiLineLookup { get; init; }
    /// <summary>
    /// Gets the non-ASCII lookup table for multi-line visitors.
    /// </summary>
    ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> MultiLineNonAsciiLookup { get; init; }
    
    /// <summary>
    /// Gets the front-matter visitor when configured.
    /// </summary>
    IMarkdownSyntaxNodeVisitor? FrontMatterSerializer { get; }

    /// <summary>
    /// Gets candidate single-line visitors for a character.
    /// </summary>
    /// <param name="c">The trigger character.</param>
    /// <returns>The candidate visitors.</returns>
    ImmutableArray<IMarkdownSyntaxNodeVisitor> GetSingleLineSerializersForChar(char c);
    /// <summary>
    /// Gets candidate multi-line visitors for a character.
    /// </summary>
    /// <param name="c">The trigger character.</param>
    /// <returns>The candidate visitors.</returns>
    ImmutableArray<IMarkdownSyntaxNodeVisitor> GetMultiLineSerializersForChar(char c);
    /// <summary>
    /// Parses markdown text into a new syntax tree.
    /// </summary>
    /// <param name="markdown">The markdown input.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeToTree(string markdown);
    /// <summary>
    /// Parses markdown text into an existing syntax tree instance.
    /// </summary>
    /// <param name="markdown">The markdown input.</param>
    /// <param name="nodeTree">The target tree to populate.</param>
    void SerializeToTree(string markdown, IMdSyntaxTree nodeTree);
}
