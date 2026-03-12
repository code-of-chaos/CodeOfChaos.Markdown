// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.TextEditor;
using System.Collections.Frozen;

namespace CodeOfChaosTests.Markdown.Editors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextEditorTests {
    private static TextEditor CreateEditor(params ITextModifier[] modifiers) {
        FrozenDictionary<string, ITextModifier> lookup = modifiers.ToFrozenDictionary(x => x.ModifierName, x => x);
        return new TextEditor {
            ModifierLookup = lookup
        };
    }

    [Test]
    public async Task Modify_UnknownModifier_ShouldDoNothing() {
        // Arrange
        TextEditor sut = CreateEditor();
        var source = new TextSource("abc");

        // Act
        sut.Modify(source, "missing".AsSpan(), new Range(0, 3));

        // Assert
        await Assert.That(source.Text).IsEqualTo("abc");
    }

    [Test]
    public async Task Modify_NonSingleLineModifier_ShouldCallOnceWithFullRange() {
        // Arrange
        var modifier = new RecordingModifier("rec", isSingleLineStructure: false);
        TextEditor sut = CreateEditor(modifier);
        var source = new TextSource("a\nb\nc");

        // Act
        sut.Modify(source, "rec".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(modifier.Ranges).Count().IsEqualTo(1);
        await Assert.That(modifier.Ranges[0]).IsEqualTo(new Range(0, source.Length));
    }

    [Test]
    public async Task Modify_SingleLineModifier_ShouldSplitByLine() {
        // Arrange
        var modifier = new RecordingModifier("rec", isSingleLineStructure: true);
        TextEditor sut = CreateEditor(modifier);
        var source = new TextSource("a\nb\nc");

        // Act
        sut.Modify(source, "rec".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(modifier.Ranges).Count().IsEqualTo(3);
        await Assert.That(modifier.Ranges).Contains(x => x.Equals(new Range(0, 1)));
        await Assert.That(modifier.Ranges).Contains(x => x.Equals(new Range(2, 3)));
        await Assert.That(modifier.Ranges).Contains(x => x.Equals(new Range(4, 5)));
    }

    [Test]
    public async Task Modify_SingleLineModifier_ShouldStripListPrefixFromSelection() {
        // Arrange
        var modifier = new RecordingModifier("rec", isSingleLineStructure: true);
        TextEditor sut = CreateEditor(modifier);
        var source = new TextSource("- item\n- two");

        // Act
        sut.Modify(source, "rec".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(modifier.Ranges).Count().IsEqualTo(2);
        await Assert.That(modifier.Ranges).Contains(x => x.Equals(new Range(2, 6)));
        await Assert.That(modifier.Ranges).Contains(x => x.Equals(new Range(9, 12)));
    }

    [Test]
    public async Task Modify_SingleLineModifier_ShouldApplyPerTableCell() {
        // Arrange
        var modifier = new RecordingModifier("rec", isSingleLineStructure: true);
        TextEditor sut = CreateEditor(modifier);
        var source = new TextSource("| a | b |");

        // Act
        sut.Modify(source, "rec".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(modifier.Ranges).Count().IsEqualTo(2);
        await Assert.That(modifier.Ranges).Contains(x => x.Start.Value >= 0 && x.End.Value <= source.Length);
        await Assert.That(modifier.Ranges).Contains(x => x.Start.Value < x.End.Value);
    }

    [Test]
    public async Task Modify_SingleLineModifier_TableHeader_ShouldSkipModification() {
        // Arrange
        var modifier = new RecordingModifier("rec", isSingleLineStructure: true);
        TextEditor sut = CreateEditor(modifier);
        var source = new TextSource("| --- | --- |");

        // Act
        sut.Modify(source, "rec".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(modifier.Ranges).Count().IsEqualTo(0);
    }

    [Test]
    public async Task Insert_ShouldReplaceRange() {
        // Arrange
        TextEditor sut = CreateEditor();
        var source = new TextSource("abcde");

        // Act
        sut.Insert(source, "XX".AsSpan(), new Range(1, 4));

        // Assert
        await Assert.That(source.Text).IsEqualTo("aXXe");
    }

    [Test]
    public async Task Insert_InvalidRange_ShouldThrow() {
        // Arrange
        TextEditor sut = CreateEditor();
        var source = new TextSource("abc");

        // Act
        Action act = () => sut.Insert(source, "x".AsSpan(), new Range(3, 1));

        // Assert
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task CaretHelpers_ShouldWorkAsExpected() {
        // Arrange
        TextEditor sut = CreateEditor();
        var source = new TextSource("a\nbc");

        // Act
        bool foundLine = sut.TryGetCaretLine(source, 2, out Range line);
        bool hasUpdateBefore = sut.TryGetCaretUpdate(out _);
        sut.UpdateCaret(4);
        bool hasUpdateAfter = sut.TryGetCaretUpdate(out int caret);
        bool hasUpdateAfterRead = sut.TryGetCaretUpdate(out _);

        // Assert
        await Assert.That(foundLine).IsTrue();
        await Assert.That(line).IsEqualTo(new Range(2, 4));
        await Assert.That(hasUpdateBefore).IsFalse();
        await Assert.That(hasUpdateAfter).IsTrue();
        await Assert.That(caret).IsEqualTo(4);
        await Assert.That(hasUpdateAfterRead).IsFalse();
    }

    [Test]
    public async Task UpdateCaret_Negative_ShouldBeIgnored() {
        // Arrange
        TextEditor sut = CreateEditor();

        // Act
        sut.UpdateCaret(-1);
        bool hasUpdate = sut.TryGetCaretUpdate(out _);

        // Assert
        await Assert.That(hasUpdate).IsFalse();
    }

    private sealed class RecordingModifier(string name, bool isSingleLineStructure) : ITextModifier {
        public string ModifierName => name;
        public bool IsSingleLineStructure => isSingleLineStructure;
        public List<Range> Ranges { get; } = [];

        public void Modify(ITextSource source, Range range, ITextEditor editor)
            => Ranges.Add(range);
    }
}
