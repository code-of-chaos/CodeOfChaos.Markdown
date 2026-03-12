// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Blazor.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Resolves emote names to URLs.
/// </summary>
public interface IEmoteDataProvider {
    /// <summary>
    /// Tries to resolve the URL for an emote name.
    /// </summary>
    /// <param name="emoteName">The emote name.</param>
    /// <param name="emoteUrl">The resolved URL when successful.</param>
    /// <returns><see langword="true" /> when resolved; otherwise <see langword="false" />.</returns>
    bool TryGetEmoteUrl(string emoteName, [NotNullWhen(true)] out string? emoteUrl);
}
