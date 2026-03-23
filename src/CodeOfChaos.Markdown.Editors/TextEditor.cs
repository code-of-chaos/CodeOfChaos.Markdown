// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Pooling;
using CodeOfChaos.Markdown.TextEditor;
using System.Collections.Frozen;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Editors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc/>
public class TextEditor : ITextEditor {
    /// <inheritdoc />
    public IEnumerable<ITextModifier> Modifiers => ModifierLookup.Values;
    
    /// <summary>
    /// A required property that provides a frozen dictionary mapping unique modifier names to their corresponding
    /// implementations of the <see cref="ITextModifier"/> interface. This dictionary is used internally by the
    /// <see cref="TextEditor"/> to look up and apply text modifications based on the specified modifier name.
    /// </summary>
    public required FrozenDictionary<string, ITextModifier> ModifierLookup { private get; init; }  
    
    private FrozenDictionary<string, ITextModifier>.AlternateLookup<ReadOnlySpan<char>>? _lookupCache;
    private FrozenDictionary<string, ITextModifier>.AlternateLookup<ReadOnlySpan<char>> AlternateLookup => _lookupCache ??= ModifierLookup.GetAlternateLookup<ReadOnlySpan<char>>();
    private int _caretIndexToUpdate = -1;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public void Modify(ITextSource source, ReadOnlySpan<char> modifierName, Range range) {
        if (!AlternateLookup.TryGetValue(modifierName, out ITextModifier? modifier)) return;

        if (!modifier.IsSingleLineStructure || range.Start.Value == range.End.Value) {
            modifier.Modify(source, range, this);
            return;
        }

        // if it is a single line structure, then we need to treat every line as a new modifier
        int totalLength = source.Length;
        int start = range.Start.GetOffset(totalLength);
        int end = range.End.GetOffset(totalLength);

        for (int index = source.LineCount - 1; index >= 0; index--) {
            Range lineRange = source.LineRanges[index];
            int lineStart = lineRange.Start.GetOffset(totalLength);
            int lineEnd = lineRange.End.GetOffset(totalLength);

            // Skip lines outside the selected range
            if (lineEnd <= start || lineStart >= end) continue;

            // Calculate the overlapping portion of the current line with the selection
            int intersectStart = Math.Max(lineStart, start);
            int intersectEnd = Math.Min(lineEnd, end);

            // check if the line is empty
            if (source.Text.AsSpan(intersectStart, intersectEnd - intersectStart).IsEmpty) continue;

            // handle special structures
            if (TryHandleAsLine(source, intersectStart, intersectEnd, modifier)) continue;
            if (TryHandleAsTableRow(source, intersectStart, intersectEnd, modifier)) continue;

            // Handle like normal
            modifier.Modify(source, new Range(intersectStart, intersectEnd), this);
        }
    }

    /// <inheritdoc />
    public void Insert(ITextSource source, ReadOnlySpan<char> input, Range range) {
        int totalLength = source.Length;
        int start = range.Start.GetOffset(totalLength);
        int end = range.End.GetOffset(totalLength);

        // Ensure the range is valid
        if (start < 0 || end > totalLength || start > end) {
            throw new ArgumentOutOfRangeException(nameof(range), "Invalid range provided for text insertion.");
        }

        // Generate the new text by replacing the specified range with the input text
        source.UpdateSource(string.Concat(source.Text.AsSpan(0, start), input, source.Text.AsSpan(end)));
    }

    /// <inheritdoc />
    public bool TryGetCaretLine(ITextSource source, int caretIndex, out Range lineRange) {
        int normalizedCaretIndex = Math.Max(0, caretIndex);

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (Range lr in source.LineRanges) {
            if (normalizedCaretIndex < lr.Start.Value) continue;
            if (normalizedCaretIndex > lr.End.Value) continue;

            lineRange = lr;
            return true;
        }

        lineRange = new Range(0, 0);
        return false;
    }

    /// <inheritdoc />
    public bool TryGetCaretUpdate(out int caretIndex) {
        caretIndex = _caretIndexToUpdate;
        if (_caretIndexToUpdate < 0) return false;
        _caretIndexToUpdate = -1;
        return true;
    }

    /// <inheritdoc />
    public void UpdateCaret(int caretIndex) {
        if (caretIndex < 0) return;

        _caretIndexToUpdate = caretIndex;
    }

    #region Modify Special Structures
    private bool TryHandleAsLine(ITextSource source, int intersectStart, int intersectEnd, ITextModifier modifier) {
        Match lineMatch = TextEditorRegexLib.ListItemsRegex.Match(source.Text, intersectStart, intersectEnd - intersectStart);
        if (!lineMatch.Success || !lineMatch.Groups[1].TryGetLength(out int prefixLength)) return false;

        modifier.Modify(source, new Range(intersectStart + prefixLength, intersectEnd), this);
        return true;
    }

    private bool TryHandleAsTableRow(ITextSource source, int intersectStart, int intersectEnd, ITextModifier modifier) {
        ReadOnlySpan<char> possibleTable = source.Text.AsSpan(intersectStart, intersectEnd - intersectStart);
        if (!TextEditorRegexLib.IsTableLineRegex.IsMatch(possibleTable)) return TextEditorRegexLib.IsTableHeaderLineRegex.IsMatch(possibleTable);

        Regex.ValueMatchEnumerator tableCellMatches = TextEditorRegexLib.TableCellsRegex.EnumerateMatches(possibleTable);
        Stack<Range> tableCellMatchesStack = GlobalPools.RangeStack.Get();
        try {
            while (tableCellMatches.MoveNext()) {
                ValueMatch current = tableCellMatches.Current;
                if (current.Index < 0 || current.Length <= 0) continue;

                tableCellMatchesStack.Push(new Range(
                    intersectStart + current.Index,
                    intersectStart + current.Index + current.Length
                ));
            }

            while (tableCellMatchesStack.TryPop(out Range result)) {
                modifier.Modify(source, result, this);
            }

            return true;
        }
        finally {
            GlobalPools.RangeStack.Return(tableCellMatchesStack);
        }
    }
    #endregion
}
