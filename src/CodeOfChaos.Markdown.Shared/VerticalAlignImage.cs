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
    /// <summary>
    /// Aligns to the baseline of the parent element.
    /// </summary>
    [UsedImplicitly] Baseline,
    /// <summary>
    /// Lowers the image to a subscript-like position.
    /// </summary>
    [UsedImplicitly] Sub,
    /// <summary>
    /// Raises the image to a superscript-like position.
    /// </summary>
    [UsedImplicitly] Super,
    /// <summary>
    /// Aligns to the top of the parent element's text.
    /// </summary>
    [UsedImplicitly] TextTop,
    /// <summary>
    /// Aligns to the bottom of the parent element's text.
    /// </summary>
    [UsedImplicitly] TextBottom,
    /// <summary>
    /// Centers relative to the parent baseline plus half the x-height.
    /// </summary>
    [UsedImplicitly] Middle,
    /// <summary>
    /// Aligns to the top of the line box.
    /// </summary>
    [UsedImplicitly] Top,
    /// <summary>
    /// Aligns to the bottom of the line box.
    /// </summary>
    [UsedImplicitly] Bottom,
    /// <summary>
    /// Uses the initial value for the property.
    /// </summary>
    [UsedImplicitly] Initial,
    /// <summary>
    /// Inherits the value from the parent element.
    /// </summary>
    [UsedImplicitly] Inherit,
    /// <summary>
    /// Reverts to the value defined by the previous cascade origin.
    /// </summary>
    [UsedImplicitly] Revert,
    /// <summary>
    /// Reverts to the value from the previous cascade layer.
    /// </summary>
    [UsedImplicitly] RevertLayer,
    /// <summary>
    /// Resets to inherited or initial depending on whether the property is inheritable.
    /// </summary>
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
