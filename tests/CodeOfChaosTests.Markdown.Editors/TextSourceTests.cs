// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors;

namespace CodeOfChaosTests.Markdown.Editors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextSourceTests {

    [Test]
    public async Task Constructor_ShouldInitializeEmptySource() {
        // Arrange
        var sut = new TextSource();

        // Act
        Range[] lines = sut.LineRanges.ToArray();

        // Assert
        await Assert.That(sut.Text).IsEqualTo(string.Empty);
        await Assert.That(sut.Length).IsEqualTo(0);
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.LineCount).IsEqualTo(1);
        await Assert.That(lines.Length).IsEqualTo(1);
        await Assert.That(lines[0].Start.Value).IsEqualTo(0);
        await Assert.That(lines[0].End.Value).IsEqualTo(0);
    }

    [Test]
    public async Task UpdateSource_ShouldPopulateSingleLine() {
        // Arrange
        var sut = new TextSource();

        // Act
        sut.UpdateSource("abc");
        Range[] lines = sut.LineRanges.ToArray();

        // Assert
        await Assert.That(sut.Text).IsEqualTo("abc");
        await Assert.That(sut.Length).IsEqualTo(3);
        await Assert.That(sut.IsEmpty).IsFalse();
        await Assert.That(sut.LineCount).IsEqualTo(1);
        await Assert.That(lines.Length).IsEqualTo(1);
        await Assert.That(lines[0].Start.Value).IsEqualTo(0);
        await Assert.That(lines[0].End.Value).IsEqualTo(3);
    }

    [Test]
    public async Task UpdateSource_ShouldSplitLinesWithoutTrailingNewline() {
        // Arrange
        var sut = new TextSource();

        // Act
        sut.UpdateSource("a\nbc");
        Range[] lines = sut.LineRanges.ToArray();

        // Assert
        await Assert.That(sut.LineCount).IsEqualTo(2);
        await Assert.That(lines.Length).IsEqualTo(2);
        await Assert.That(lines[0].Start.Value).IsEqualTo(0);
        await Assert.That(lines[0].End.Value).IsEqualTo(1);
        await Assert.That(lines[1].Start.Value).IsEqualTo(2);
        await Assert.That(lines[1].End.Value).IsEqualTo(4);
    }

    [Test]
    public async Task UpdateSource_ShouldHandleTrailingNewlineAndNormalizeEndings() {
        // Arrange
        var sut = new TextSource();

        // Act
        sut.UpdateSource("a\r\nb\r\n"); // Windows endings should normalize to '\n'
        Range[] lines = sut.LineRanges.ToArray();

        // Assert
        await Assert.That(sut.Text).IsEqualTo("a\nb\n");
        await Assert.That(sut.LineCount).IsEqualTo(3); // the trailing newline produces an empty last line
        await Assert.That(lines.Length).IsEqualTo(3);
        await Assert.That(lines[0].Start.Value).IsEqualTo(0);
        await Assert.That(lines[0].End.Value).IsEqualTo(1);
        await Assert.That(lines[1].Start.Value).IsEqualTo(2);
        await Assert.That(lines[1].End.Value).IsEqualTo(3);
        await Assert.That(lines[2].Start.Value).IsEqualTo(4);
        await Assert.That(lines[2].End.Value).IsEqualTo(4);
    }

    [Test]
    public async Task UpdateSource_ShouldResetToEmpty() {
        // Arrange
        var sut = new TextSource("initial");

        // Act
        sut.UpdateSource(string.Empty);
        Range[] lines = sut.LineRanges.ToArray();

        // Assert
        await Assert.That(sut.Text).IsEqualTo(string.Empty);
        await Assert.That(sut.Length).IsEqualTo(0);
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.LineCount).IsEqualTo(1);
        await Assert.That(lines.Length).IsEqualTo(1);
        await Assert.That(lines[0].Start.Value).IsEqualTo(0);
        await Assert.That(lines[0].End.Value).IsEqualTo(0);
    }
}