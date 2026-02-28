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
[InjectableSingleton<ITextModifier>]
[SuppressMessage("ReSharper", "ReplaceAutoPropertyWithComputedProperty")]
public class UnderlineModifier(ILogger<UnderlineModifier> logger) : SingleInstructionModifiers(logger) {
    public const string Name = "underline";
    
    public override string ModifierName { get; } = Name;
    protected override string Instruction { get; } = "_";
}
