// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Defines supported vertical alignment values for rendered images.
/// </summary>
public enum VerticalAlignImage {
    [UsedImplicitly] Baseline,
    [UsedImplicitly] Sub,
    [UsedImplicitly] Super,
    [UsedImplicitly] TextTop,
    [UsedImplicitly] TextBottom,
    [UsedImplicitly] Middle,
    [UsedImplicitly] Top,
    [UsedImplicitly] Bottom,
    [UsedImplicitly] Initial,
    [UsedImplicitly] Inherit,
    [UsedImplicitly] Revert,
    [UsedImplicitly] RevertLayer,
    [UsedImplicitly] Unset
}

/// <summary>
/// Provides conversion helpers for <see cref="VerticalAlignImage" />.
/// </summary>
public static class VerticalAlignImageUtilities {
    /// <summary>
    /// Tries to parse a string as a <see cref="VerticalAlignImage" /> value.
    /// </summary>
    /// <param name="input">The input text.</param>
    /// <param name="verticalAlign">The parsed enum value when successful.</param>
    /// <returns><see langword="true" /> when parsing succeeds; otherwise <see langword="false" />.</returns>
    public static bool TryGetFromString(string? input, out VerticalAlignImage verticalAlign) {
        verticalAlign = default;
        return !input.IsNullOrWhiteSpace() && Enum.TryParse(input, true, out verticalAlign);

    }
}
