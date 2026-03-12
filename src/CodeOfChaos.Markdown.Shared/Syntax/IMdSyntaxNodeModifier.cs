// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents parsed modifier attributes attached to a syntax node.
/// </summary>
public interface IMdSyntaxNodeModifier : IEquatable<IMdSyntaxNodeModifier> {
    /// <summary>
    /// Gets modifier attributes as key-to-range mappings into <see cref="OriginalInput" />.
    /// </summary>
    FrozenDictionary<string, Range> Attributes { get; }
    /// <summary>
    /// Gets the original modifier input string.
    /// </summary>
    string OriginalInput { get; }
    /// <summary>
    /// Gets the original modifier input as a span.
    /// </summary>
    ReadOnlySpan<char> OriginalInputSpan { get; }
    /// <summary>
    /// Tries to get a string attribute value.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value when found.</param>
    /// <returns><see langword="true" /> when the key exists; otherwise <see langword="false" />.</returns>
    bool TryGetValue(string key, [NotNullWhen(true)] out string? value);
    /// <summary>
    /// Tries to get a boolean flag value.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The parsed flag value when found.</param>
    /// <returns><see langword="true" /> when the key exists and is parseable; otherwise <see langword="false" />.</returns>
    bool TryGetFlag(string key, out bool value);

    /// <summary>
    /// Returns the modifier instance to its pool.
    /// </summary>
    void ReturnToPool();

    /// <summary>
    /// Tries to get the <c>icon</c> attribute.
    /// </summary>
    bool TryGetIconName([NotNullWhen(true)] out string? iconName);
    /// <summary>
    /// Tries to get the <c>title</c> attribute.
    /// </summary>
    bool TryGetTitle([NotNullWhen(true)] out string? title);
    /// <summary>
    /// Tries to parse the <c>size</c> attribute.
    /// </summary>
    bool TryGetSize(out (int Width, int Height) size);
    /// <summary>
    /// Tries to parse the <c>fit</c> flag.
    /// </summary>
    bool TryGetFit(out bool state);
    /// <summary>
    /// Tries to get the <c>align</c> attribute.
    /// </summary>
    bool TryGetAlign([NotNullWhen(true)] out string? align);
    /// <summary>
    /// Tries to get the <c>color</c> attribute.
    /// </summary>
    bool TryGetColor([NotNullWhen(true)] out string? color);
    /// <summary>
    /// Tries to get the <c>style</c> attribute.
    /// </summary>
    bool TryGetStyle([NotNullWhen(true)] out string? style);
}
