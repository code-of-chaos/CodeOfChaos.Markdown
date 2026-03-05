// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeOfChaosTests.Markdown.Editors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SingleInstructionModifiersTests {
    [Test]
    public async Task Modify_ValidRange_ShouldWrapSelectionAndUpdateCaret() {
        // Arrange
        var source = new TextSource("abcd");
        var editor = new CaretTrackingEditor();
        var sut = new TestModifier();

        // Act
        sut.Modify(source, new Range(1, 3), editor);

        // Assert
        await Assert.That(source.Text).IsEqualTo("a!!bc!!d");
        await Assert.That(editor.CaretIndex).IsEqualTo(3);
    }

    [Test]
    public async Task Modify_InvalidRange_ShouldNotChangeTextOrCaret() {
        // Arrange
        var source = new TextSource("abcd");
        var editor = new CaretTrackingEditor();
        var sut = new TestModifier();

        // Act
        sut.Modify(source, new Range(3, 1), editor);

        // Assert
        await Assert.That(source.Text).IsEqualTo("abcd");
        await Assert.That(editor.CaretIndex).IsEqualTo(-1);
    }

    private sealed class TestModifier()
        : SingleInstructionModifiers(NullLogger.Instance) {
        protected override string Instruction => "!!";
        public override string ModifierName => "test";
    }

    private sealed class CaretTrackingEditor : ITextEditor {
        public int CaretIndex { get; private set; } = -1;
        public IEnumerable<ITextModifier> Modifiers => [];

        public void Modify(ITextSource source, ReadOnlySpan<char> modifierName, Range range) { }
        public void Insert(ITextSource source, ReadOnlySpan<char> input, Range range) { }
        public bool TryGetCaretLine(ITextSource source, int caretIndex, out Range lineRange) {
            lineRange = new Range(0, 0);
            return false;
        }

        public bool TryGetCaretUpdate(out int caretIndex) {
            caretIndex = CaretIndex;
            return CaretIndex >= 0;
        }

        public void UpdateCaret(int caretIndex) {
            CaretIndex = caretIndex;
        }
    }
}
