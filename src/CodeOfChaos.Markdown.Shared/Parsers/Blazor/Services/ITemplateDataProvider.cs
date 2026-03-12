// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Blazor.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Resolves template variable values for markdown templating.
/// </summary>
public interface ITemplateDataProvider {
    /// <summary>
    /// Tries to resolve data for a template variable.
    /// </summary>
    /// <param name="variableName">The variable name.</param>
    /// <param name="data">The resolved data when successful.</param>
    /// <returns><see langword="true" /> when resolved; otherwise <see langword="false" />.</returns>
    bool TryGetData(string variableName,[NotNullWhen(true)] out object? data);
}
