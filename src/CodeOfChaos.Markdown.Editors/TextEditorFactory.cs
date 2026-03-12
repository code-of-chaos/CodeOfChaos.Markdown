// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;

namespace CodeOfChaos.Markdown.Editors;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A factory class for creating instances of the <see cref="ITextEditor"/> interface.
/// </summary>
/// <remarks>
/// Use this factory to create text editors with specific text modifiers.
/// The factory supports creating both generic and keyed text editors based on the provided service configuration.
/// </remarks>
public static class TextEditorFactory {
    /// <summary>
    /// Creates an instance of <see cref="ITextEditor"/> using the specified service provider.
    /// </summary>
    /// <param name="provider">
    /// The service provider used to resolve dependencies required by the text editor.
    /// </param>
    /// <returns>
    /// An instance of <see cref="ITextEditor"/> configured with the provided services.
    /// </returns>
    public static ITextEditor CreateTextEditor(IServiceProvider provider) => CreateKeyedTextEditor(provider, null);
   
    /// <summary>
    /// Creates an instance of <see cref="ITextEditor"/> using the specified service provider
    /// and an optional key to filter text modifiers.
    /// </summary>
    /// <param name="provider">An <see cref="IServiceProvider"/> used to resolve keyed services of type <see cref="ITextModifier"/>.</param>
    /// <param name="key">An optional key used to filter the <see cref="ITextModifier"/> instances to include. If null, all modifiers will be included.</param>
    /// <returns>An initialized instance of <see cref="ITextEditor"/> with the resolved modifiers applied.</returns>
    public static ITextEditor CreateKeyedTextEditor(IServiceProvider provider, object? key) {
        IEnumerable<ITextModifier> modifiers = provider.GetKeyedServices<ITextModifier>(key);
        
        // Take the last modifier for each modifier name to handle duplicates
        FrozenDictionary<string, ITextModifier> modifierLookup = modifiers
            .GroupBy(modifier => modifier.ModifierName)
            .ToFrozenDictionary(
                group => group.Key,
                group => group.Last()
            );
        
        return new TextEditor {
            ModifierLookup = modifierLookup
        };

    }
}
