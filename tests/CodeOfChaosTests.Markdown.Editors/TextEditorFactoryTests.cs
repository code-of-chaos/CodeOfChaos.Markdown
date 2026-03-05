// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CodeOfChaosTests.Markdown.Editors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class TextEditorFactoryTests {
    [Test]
    public async Task CreateTextEditor_DuplicateNames_ShouldUseLastModifier() {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ILogger<TextEditor>>(NullLogger<TextEditor>.Instance);
        services.AddSingleton<ITextModifier>(new AssigningModifier("dup", "first"));
        services.AddSingleton<ITextModifier>(new AssigningModifier("dup", "second"));
        ServiceProvider provider = services.BuildServiceProvider();
        var source = new TextSource("initial");

        // Act
        ITextEditor editor = TextEditorFactory.CreateTextEditor(provider);
        editor.Modify(source, "dup".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(source.Text).IsEqualTo("second");
    }

    [Test]
    public async Task CreateKeyedTextEditor_ShouldOnlyUseMatchingKeyedModifiers() {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ILogger<TextEditor>>(NullLogger<TextEditor>.Instance);
        services.AddKeyedSingleton<ITextModifier>("a", new AssigningModifier("m", "A"));
        services.AddKeyedSingleton<ITextModifier>("b", new AssigningModifier("m", "B"));
        ServiceProvider provider = services.BuildServiceProvider();
        var source = new TextSource("initial");

        // Act
        ITextEditor editor = TextEditorFactory.CreateKeyedTextEditor(provider, "b");
        editor.Modify(source, "m".AsSpan(), new Range(0, source.Length));

        // Assert
        await Assert.That(source.Text).IsEqualTo("B");
    }

    private sealed class AssigningModifier(string name, string replacement) : ITextModifier {
        public string ModifierName => name;
        public bool IsSingleLineStructure => false;

        public void Modify(ITextSource source, Range range, ITextEditor editor)
            => source.UpdateSource(replacement);
    }
}
