// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.TextEditor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines editing operations for text-based markdown inputs.
/// </summary>
public interface ITextEditor {
    /// <summary>
    /// Gets the available text modifiers.
    /// </summary>
    IEnumerable<ITextModifier> Modifiers { get; }

    /// <summary>
    /// Applies a named modifier to the specified range.
    /// </summary>
    /// <param name="source">The editable text source.</param>
    /// <param name="modifierName">The modifier identifier.</param>
    /// <param name="range">The target range.</param>
    void Modify(ITextSource source, ReadOnlySpan<char> modifierName, Range range);
    /// <summary>
    /// Replaces the specified range with the provided input.
    /// </summary>
    /// <param name="source">The editable text source.</param>
    /// <param name="input">The input to insert.</param>
    /// <param name="range">The range to replace.</param>
    void Insert(ITextSource source, ReadOnlySpan<char> input, Range range);

    /// <summary>
    /// Tries to resolve the line range containing the caret index.
    /// </summary>
    /// <param name="source">The source to query.</param>
    /// <param name="caretIndex">The caret position.</param>
    /// <param name="lineRange">The resolved line range when found.</param>
    /// <returns><see langword="true" /> when a line is found; otherwise <see langword="false" />.</returns>
    bool TryGetCaretLine(ITextSource source, int caretIndex, out Range lineRange);
    /// <summary>
    /// Tries to get a pending caret update.
    /// </summary>
    /// <param name="caretIndex">The pending caret index when available.</param>
    /// <returns><see langword="true" /> when a pending update exists; otherwise <see langword="false" />.</returns>
    bool TryGetCaretUpdate(out int caretIndex);
    /// <summary>
    /// Schedules a caret index update.
    /// </summary>
    /// <param name="caretIndex">The caret position to apply.</param>
    void UpdateCaret(int caretIndex);
}
