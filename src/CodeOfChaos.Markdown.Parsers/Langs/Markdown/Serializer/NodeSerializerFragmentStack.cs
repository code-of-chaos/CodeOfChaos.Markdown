// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using Microsoft.Extensions.ObjectPool;
using System.Buffers;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class NodeSerializerFragmentStack : INodeSerializerFragmentStack, IResettable {
    public IMdStringMdSyntaxSerializer SerializerReference { get; set; } = null!;
    
    private readonly Stack<NodeSerializerFragment> _stack = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region PushToStack
    /// <inheritdoc />
    public void PushMultiLineMatchesToStack(string input, IMdSyntaxNode node, int startIndex = 0) {
        if (input.Length == 0) return;

        int scanPos = startIndex;
        int inputLength = input.Length;
        int index = 0;

        NodeSerializerFragment[] fragments = ArrayPool<NodeSerializerFragment>.Shared.Rent(32);

        try {
            while (scanPos < inputLength) {
                char currentChar = input[scanPos];
                bool matched = false;

                // Get serializers that can trigger on this specific line-start character
                ImmutableArray<IMarkdownSyntaxNodeVisitor> candidates = SerializerReference.GetMultiLineSerializersForChar(currentChar);

                foreach (IMarkdownSyntaxNodeVisitor serializer in candidates) {
                    if (!serializer.TryGetSerializationMatch(input, out Match? match, scanPos)) continue;
                    if (match.Index != scanPos) continue;

                    EnsureCapacity(ref fragments, ref index, 1);
                    fragments[index++] = NodeSerializerFragment.AsUnhandledMatch(match, node, serializer);

                    scanPos += Math.Max(1, match.Length);
                    matched = true;
                    break;
                }

                if (matched) continue;

                // If no multiline block matched at this position, jump to the start of the next line.
                // We don't need to check characters mid-line for block structures.
                int nextLine = input.IndexOf('\n', scanPos);
                if (nextLine == -1) break;
                
                scanPos = nextLine + 1;
            }

            _stack.EnsureCapacity(_stack.Count + index);
            for (int i = index - 1; i >= 0; i--) {
                _stack.Push(fragments[i]);
            }
        }
        finally {
            ArrayPool<NodeSerializerFragment>.Shared.Return(fragments, clearArray: false);
        }
    }

    /// <inheritdoc />
    public void PushSingleLineMatchesToStack(string input, IMdSyntaxNode node) {
        if (input.Length == 0) return;

        int scanPos = 0;
        int length = input.Length;
        int textStart = 0;
        int index = 0;

        SearchValues<char> searchValues = SerializerReference.SingleLineTriggerSearchValues;
        ReadOnlySpan<char> span = input.AsSpan();
        
        NodeSerializerFragment[] fragments = ArrayPool<NodeSerializerFragment>.Shared.Rent(128);
        
        try {
            while (scanPos < length) {
                // Find the next character that could potentially be a tag
                int offset = span[scanPos..].IndexOfAny(searchValues);
                if (offset == -1) break; // No more trigger characters left in the whole string
                
                // Move scanPos to the found trigger
                scanPos += offset;
                char currentChar = span[scanPos];
                
                ImmutableArray<IMarkdownSyntaxNodeVisitor> candidates = SerializerReference.GetSingleLineSerializersForChar(currentChar);
                IMarkdownSyntaxNodeVisitor? winner = null;
                Match? winningMatch = null;

                foreach (IMarkdownSyntaxNodeVisitor serializer in candidates) {
                    if (!serializer.TryGetSerializationMatch(input, out Match? match, scanPos)) continue;
                    if (match.Index != scanPos) continue;

                    winner = serializer;
                    winningMatch = match;
                    break;
                }

                if (winner != null && winningMatch != null) {
                    // Push everything from textStart up to scanPos as a TextNode
                    if (scanPos > textStart) {
                        TextMdSyntaxNode contentNode = MdSyntaxNodePool<TextMdSyntaxNode>.Shared.Get();
                        contentNode.WithContent(input[textStart..scanPos]);
                        EnsureCapacity(ref fragments, ref index, 1);
                        fragments[index++] = NodeSerializerFragment.AsProcessedNode(node, contentNode);
                    }

                    // Push the actual Tag/Match
                    EnsureCapacity(ref fragments, ref index, 1);
                    fragments[index++] = NodeSerializerFragment.AsUnhandledMatch(winningMatch, node, winner);

                    scanPos += Math.Max(1, winningMatch.Length);
                    textStart = scanPos; 
                }
                else {
                    // It was a trigger character (like '*') but not a valid tag.
                    // Increment scanPos so IndexOfAny finds the NEXT trigger.
                    scanPos++;
                }
            }

            // Push any remaining trailing text
            if (textStart < length) {
                TextMdSyntaxNode tail = MdSyntaxNodePool<TextMdSyntaxNode>.Shared.Get();
                tail.WithContent(input[textStart..]);
                EnsureCapacity(ref fragments, ref index, 1);
                fragments[index++] = NodeSerializerFragment.AsProcessedNode(node, tail);
            }

            _stack.EnsureCapacity(_stack.Count + index);
            for (int i = index - 1; i >= 0; i--) {
                _stack.Push(fragments[i]);
            }
        }
        finally {
            ArrayPool<NodeSerializerFragment>.Shared.Return(fragments, clearArray: false);
        }
    }

    private static void EnsureCapacity(ref NodeSerializerFragment[] arr, ref int index, int required) {
        if (index + required <= arr.Length) return;

        int newSize = arr.Length * 2;
        NodeSerializerFragment[] newArr = ArrayPool<NodeSerializerFragment>.Shared.Rent(newSize);
        Array.Copy(arr, newArr, index);
        ArrayPool<NodeSerializerFragment>.Shared.Return(arr, clearArray: false);
        arr = newArr;
    }

    /// <inheritdoc />
    public void PushProcessedNodeToStack(IMdSyntaxNode parentNode, IMdSyntaxNode childNode)
        => _stack.Push(NodeSerializerFragment.AsProcessedNode(parentNode, childNode));
    #endregion

    public bool TryPopDto(out NodeSerializerFragment dto) {
        return _stack.TryPop(out dto);
    }

    public bool TryReset() {
        _stack.Clear();
        return true;
    }
}
