// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Blazor.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Resolves user identifiers to profile links.
/// </summary>
public interface IUserLinkProvider {
    /// <summary>
    /// Tries to resolve a link for a user name.
    /// </summary>
    /// <param name="username">The user name.</param>
    /// <param name="link">The resolved link when successful.</param>
    /// <returns><see langword="true" /> when resolved; otherwise <see langword="false" />.</returns>
    bool TryGetUserLink(string username,[NotNullWhen(true)] out string? link);
}
