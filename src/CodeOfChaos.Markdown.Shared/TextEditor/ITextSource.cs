// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.TextEditor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents mutable text input with cached line metadata.
/// </summary>
public interface ITextSource {
    /// <summary>
    /// Gets the current text value.
    /// </summary>
    string Text { get; }
    /// <summary>
    /// Gets the text as a span.
    /// </summary>
    ReadOnlySpan<char> TextSpan { get; }

    /// <summary>
    /// Gets the text length.
    /// </summary>
    int Length { get; }
    /// <summary>
    /// Gets the cached line ranges.
    /// </summary>
    ReadOnlySpan<Range> LineRanges { get; }
    /// <summary>
    /// Gets the number of cached lines.
    /// </summary>
    int LineCount { get; }
    
    /// <summary>
    /// Gets a value indicating whether the source is empty.
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// Replaces the source text and updates line caches.
    /// </summary>
    /// <param name="value">The new text value.</param>
    void UpdateSource(string value);
}
