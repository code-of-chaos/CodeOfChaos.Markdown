// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using CollectionExtensions=System.Collections.Generic.CollectionExtensions;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class MdStringMdSyntaxSerializer(ILogger<MdStringMdSyntaxSerializer> logger) : IMdStringMdSyntaxSerializer {
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> SingleLineSerializers { get; init; }
    /// <inheritdoc />
    public required SearchValues<char> SingleLineTriggerSearchValues { get; init; }
    /// <inheritdoc />
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor>[] SingleLineLookup { get; init; }
    /// <inheritdoc />
    public required ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> SingleLineNonAsciiLookup { get; init; }
    
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor> MultiLineSerializers { get; init; }
    /// <inheritdoc />
    public required ImmutableArray<IMarkdownSyntaxNodeVisitor>[] MultiLineLookup { get; init; }
    /// <inheritdoc />
    public required ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> MultiLineNonAsciiLookup { get; init; }
    
    /// <inheritdoc />
    public required IMarkdownSyntaxNodeVisitor? FrontMatterSerializer { get; init; }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public ImmutableArray<IMarkdownSyntaxNodeVisitor> GetSingleLineSerializersForChar(char c)
        => c < 256
            ? SingleLineLookup[c]
            : CollectionExtensions.GetValueOrDefault(SingleLineNonAsciiLookup, c, SingleLineSerializers);

    /// <inheritdoc />
    public ImmutableArray<IMarkdownSyntaxNodeVisitor> GetMultiLineSerializersForChar(char c)
        => c < 256
            ? MultiLineLookup[c]
            : CollectionExtensions.GetValueOrDefault(MultiLineNonAsciiLookup, c, MultiLineSerializers);

    /// <inheritdoc />
    public IMdSyntaxTree SerializeToTree(string markdown) {
        IMdSyntaxTree nodeTree = MdSyntaxTreePool.Shared.Get();
        SerializeToTree(markdown, nodeTree);
        return nodeTree;
    }

    /// <inheritdoc />
    public void SerializeToTree(string markdown, IMdSyntaxTree nodeTree) {
        NodeSerializerFragmentStack fragmentStack = MdSyntaxFragmentStackPool.Shared.Get();
        fragmentStack.SerializerReference = this;

        string normalized = markdown.Contains('\r')
            ? markdown.ReplaceLineEndings("\n")
            : markdown;

        try {
            TryExtractFrontMatter(fragmentStack, normalized, nodeTree, out int newStartAtIndex);
            fragmentStack.PushMultiLineMatchesToStack(normalized, nodeTree.RootNode, newStartAtIndex);

            while (fragmentStack.TryPopDto(out NodeSerializerFragment fragment)) {
                switch (fragment) {
                    // Not yet processed
                    case { ParentNode: { } parentNode, Match: { } match, NodeSerializer: { } serializer }: {
                        serializer.Serialize(fragmentStack, parentNode, match);
                        break;
                    }

                    // Already processed and simply needs to be added in correct location
                    case { ParentNode: { } parentNode, ChildNode: { } childNode }: {
                        parentNode.AddChildNode(childNode);
                        break;
                    }

                    // Unhandled state which should never happen
                    default: {
                        logger.LogError("Unhandled state in MdStringMdSyntaxSerializer.SerializeToTree with fragment '{Fragment}'.", fragment);
                        break;
                    }
                }
            }
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error parsing Markdown during tree conversion.");
            throw;
        }
        finally {
            MdSyntaxFragmentStackPool.Shared.Return(fragmentStack);
        }
    }

    private void TryExtractFrontMatter(NodeSerializerFragmentStack fragmentStack, string markdown, IMdSyntaxTree nodeTree, out int newStartAtIndex) {
        newStartAtIndex = 0;
        if (FrontMatterSerializer is null) return;
        if (!FrontMatterSerializer.TryGetSerializationMatch(markdown, out Match? match)) return;
        
        newStartAtIndex = match.Index + match.Length;
        FrontMatterSerializer.Serialize(fragmentStack, nodeTree.RootNode, match);
    }
}
