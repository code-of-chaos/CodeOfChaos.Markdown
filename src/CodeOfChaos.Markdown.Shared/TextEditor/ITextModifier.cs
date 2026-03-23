// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.TextEditor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a named text transformation that can be applied by an <see cref="ITextEditor" />.
/// </summary>
public interface ITextModifier {
    /// <summary>
    /// Gets the modifier name used for lookup.
    /// </summary>
    string ModifierName { get; }
    /// <summary>
    /// Gets a value indicating whether the modifier is applied as a single-line structure.
    /// </summary>
    bool IsSingleLineStructure { get; }

    /// <summary>
    /// Applies the modifier to the specified range.
    /// </summary>
    /// <param name="source">The source text.</param>
    /// <param name="range">The target range.</param>
    /// <param name="editor">The active editor instance.</param>
    void Modify(ITextSource source, Range range, ITextEditor editor);
}
