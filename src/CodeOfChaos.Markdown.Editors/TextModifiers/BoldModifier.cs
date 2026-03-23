// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Editors.TextModifiers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a text modifier implementation for applying bold formatting in a Markdown text editor.
/// </summary>
/// <remarks>
/// This class is implemented as a singleton and is part of the Markdown text editor framework. The bold formatting
/// is achieved using double asterisks (**) to wrap the target text.
/// It inherits from <see cref="SingleInstructionModifiers"/> to define a specific instruction for bold formatting.
/// </remarks>
[InjectableSingleton<ITextModifier>]
[SuppressMessage("ReSharper", "ReplaceAutoPropertyWithComputedProperty")]
public class BoldModifier(ILogger<BoldModifier> logger) : SingleInstructionModifiers(logger) {
    private const string Name = "bold";
    
    /// <inheritdoc />
    public override string ModifierName { get; } = Name;
    
    /// <inheritdoc />
    protected override string Instruction { get; } = "**";
}
