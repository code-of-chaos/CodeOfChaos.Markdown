// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.Extensions.Logging;
using System.Buffers;

namespace CodeOfChaos.Markdown.Editors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents an abstract base class for text modifiers that apply single instruction-based
/// modifications to a text source within a specified range in a Markdown editing context.
/// </summary>
/// <remarks>
/// This class provides a foundational implementation for modifying text in a way that requires
/// applying a single, predefined instruction on a specific range of text. Derived classes must
/// define the specific instruction to be applied and the name of the modifier.
/// </remarks>
/// <example>
/// This class is intended to be extended by more specific implementations like `StrikeModifier`
/// or `HighlightModifier` that define their own custom instructions and behavior.
/// </example>
public abstract class SingleInstructionModifiers(ILogger logger) : ITextModifier {
    /// <summary>
    /// Represents the instruction used to define the behavior of a text modifier.
    /// </summary>
    /// <remarks>
    /// The <c>Instruction</c> property provides the specific syntax marker that a text modifier will apply to transform text,
    /// such as highlighting, underlining, italicizing, or other forms of Markdown text alterations.
    /// </remarks>
    protected abstract string Instruction { get; }
    
    /// <inheritdoc />
    public abstract string ModifierName { get; }

    /// <inheritdoc />
    public bool IsSingleLineStructure => true;

    /// <inheritdoc />
    public void Modify(ITextSource source, Range range, ITextEditor editor) {
        ReadOnlySpan<char> inputSpan = source.TextSpan;

        // Calculate indices for range
        int length = source.Length;
        int start = range.Start.GetOffset(length);
        int end = range.End.GetOffset(length);

        // Validate range
        if (start < 0 || end > length || start > end) {
            logger.Warning("Invalid range: start={start}, end={end}, length={length}", start, end, length);
            return;
        }

        // Precompute the final size needed for the buffer
        ReadOnlySpan<char> instructionSpan = Instruction.AsSpan();
        int instructionLength = instructionSpan.Length;
        int finalLength = length + instructionLength * 2;

        // Rent a buffer using ArrayPool to avoid frequent allocations
        char[] buffer = ArrayPool<char>.Shared.Rent(finalLength);
        Span<char> bufferSpan = buffer.AsSpan();

        try {
            // Build the resulting string with modifications
            inputSpan[..start].CopyTo(bufferSpan[..start]);
            instructionSpan.CopyTo(bufferSpan[start..]);
            inputSpan.Slice(start, end - start).CopyTo(bufferSpan[(start + instructionLength)..]);
            instructionSpan.CopyTo(bufferSpan[(start + instructionLength + (end - start))..]);
            inputSpan[end..].CopyTo(bufferSpan[(start + instructionLength * 2 + (end - start))..]);

            // Place caret after the first instruction
            editor.UpdateCaret(range.Start.Value + instructionLength);

            // Return only the relevant portion of the buffer as a string
            source.UpdateSource(new string(bufferSpan[..finalLength]));
        }
        finally {
            // Return the buffer to the pool to avoid memory leaks
            ArrayPool<char>.Shared.Return(buffer);
        }
    }
}
