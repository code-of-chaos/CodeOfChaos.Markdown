// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Blazor.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Resolves tag labels to links.
/// </summary>
public interface ITagLinkProvider {
    /// <summary>
    /// Tries to resolve a link for a tag value.
    /// </summary>
    /// <param name="tag">The tag value.</param>
    /// <param name="link">The resolved link when successful.</param>
    /// <returns><see langword="true" /> when resolved; otherwise <see langword="false" />.</returns>
    bool TryGetTagLink(string tag,[NotNullWhen(true)] out string? link);
}
