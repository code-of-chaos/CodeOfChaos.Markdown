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
public interface IMdStringMdSyntaxSerializer {
    SearchValues<char> SingleLineTriggerSearchValues { get; }
    
    ImmutableArray<IMarkdownSyntaxNodeVisitor>[] SingleLineLookup { get; init; }
    ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> SingleLineNonAsciiLookup { get; init; }

    ImmutableArray<IMarkdownSyntaxNodeVisitor>[] MultiLineLookup { get; init; }
    ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> MultiLineNonAsciiLookup { get; init; }
    
    IMarkdownSyntaxNodeVisitor? FrontMatterSerializer { get; }

    ImmutableArray<IMarkdownSyntaxNodeVisitor> GetSingleLineSerializersForChar(char c);
    ImmutableArray<IMarkdownSyntaxNodeVisitor> GetMultiLineSerializersForChar(char c);
    IMdSyntaxTree SerializeToTree(string markdown);
    void SerializeToTree(string markdown, IMdSyntaxTree nodeTree);
}
