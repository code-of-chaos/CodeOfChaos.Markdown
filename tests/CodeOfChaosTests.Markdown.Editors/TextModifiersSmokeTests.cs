// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors.TextModifiers;
using CodeOfChaos.Markdown.TextEditor;
using CodeOfChaos.Markdown.Editors;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeOfChaosTests.Markdown.Editors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextModifiersSmokeTests {
    [Test]
    [Arguments("bold", "**x**")]
    [Arguments("italic", "*x*")]
    [Arguments("code-inline", "`x`")]
    [Arguments("highlight", "==x==")]
    [Arguments("strike", "~~x~~")]
    [Arguments("subscript", "~x~")]
    [Arguments("superscript", "^x^")]
    [Arguments("underline", "_x_")]
    public async Task Modifiers_ShouldWrapSelection(string modifierName, string expected) {
        // Arrange
        ITextModifier modifier = CreateModifier(modifierName);
        var source = new TextSource("x");
        var editor = new NoOpEditor();

        // Act
        modifier.Modify(source, new Range(0, 1), editor);

        // Assert
        await Assert.That(source.Text).IsEqualTo(expected);
        await Assert.That(modifier.IsSingleLineStructure).IsTrue();
        await Assert.That(modifier.ModifierName).IsEqualTo(modifierName);
    }

    private static ITextModifier CreateModifier(string name)
        => name switch {
            "bold" => new BoldModifier(NullLogger<BoldModifier>.Instance),
            "italic" => new ItalicModifier(NullLogger<ItalicModifier>.Instance),
            "code-inline" => new CodeInlineModifier(NullLogger<CodeInlineModifier>.Instance),
            "highlight" => new HighlightModifier(NullLogger<HighlightModifier>.Instance),
            "strike" => new StrikeModifier(NullLogger<StrikeModifier>.Instance),
            "subscript" => new SubscriptModifier(NullLogger<SubscriptModifier>.Instance),
            "superscript" => new SuperscriptModifier(NullLogger<SuperscriptModifier>.Instance),
            "underline" => new UnderlineModifier(NullLogger<UnderlineModifier>.Instance),
            _ => throw new ArgumentOutOfRangeException(nameof(name))
        };

    private sealed class NoOpEditor : ITextEditor {
        public IEnumerable<ITextModifier> Modifiers => [];
        public void Modify(ITextSource source, ReadOnlySpan<char> modifierName, Range range) { }
        public void Insert(ITextSource source, ReadOnlySpan<char> input, Range range) { }
        public bool TryGetCaretLine(ITextSource source, int caretIndex, out Range lineRange) {
            lineRange = new Range(0, 0);
            return false;
        }

        public bool TryGetCaretUpdate(out int caretIndex) {
            caretIndex = -1;
            return false;
        }

        public void UpdateCaret(int caretIndex) { }
    }
}
