// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Pooling;
using Microsoft.Extensions.ObjectPool;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a syntax node modifier for processing Markdown syntax elements.
/// Provides mechanisms to parse attributes and flags from a given input string,
/// enabling efficient reuse via pooling to optimize memory usage.
/// </summary>
public class MdSyntaxNodeModifier : IMdSyntaxNodeModifier, IResettable {
    private Lazy<FrozenDictionary<string, Range>> InternalAttributesDictionary { get; set; } = new(static () => FrozenDictionary<string, Range>.Empty);

    /// <inheritdoc />
    public FrozenDictionary<string, Range> Attributes => InternalAttributesDictionary.Value;
    /// <inheritdoc />
    public string OriginalInput { get; private set; } = string.Empty;
    /// <inheritdoc />
    public ReadOnlySpan<char> OriginalInputSpan => OriginalInput.AsSpan();

    /// A thread-safe object pool for managing instances of the <see cref="MdSyntaxNodeModifier"/> class.
    /// This pool facilitates reusability of objects to reduce memory allocation overhead and improve performance.
    public static ObjectPool<MdSyntaxNodeModifier> Pool { get; } = PoolingHelpers.CreateResettablePool<MdSyntaxNodeModifier>(16);

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Creates a new instance of <see cref="MdSyntaxNodeModifier"/> from the provided input string.
    /// </summary>
    /// <param name="input">The input string used to initialize the <see cref="MdSyntaxNodeModifier"/>.</param>
    /// <returns>
    /// A new instance of <see cref="MdSyntaxNodeModifier"/> populated using the given input string.
    /// </returns>
    public static MdSyntaxNodeModifier FromString(string input) {
        MdSyntaxNodeModifier mod = Pool.Get();
        mod.OriginalInput = input;

        if (mod.OriginalInputSpan.IsWhiteSpace()) return mod;

        mod.InternalAttributesDictionary = new Lazy<FrozenDictionary<string, Range>>(() => GetLazyDictionary(mod.OriginalInputSpan));

        return mod;
    }

    // ReSharper disable once InvertIf
    private static FrozenDictionary<string, Range> GetLazyDictionary(scoped ReadOnlySpan<char> span) {
        ReadOnlySpan<char> spanTrimmed = span.TrimEnd(' ');
        int spanLength = spanTrimmed.Length;

        Span<char> buffer = stackalloc char[spanLength - 1];// set it to the max possible, and all mods start with a | based on the regex, so we can trim one char

        int keyStart = 0;
        int keyEnd = -1;
        int valueStart = 0;

        var dictionary = new Dictionary<string, Range>(StringComparer.OrdinalIgnoreCase);

        for (int pointer = 0; pointer < spanLength;) {
            char currentCharacter = spanTrimmed[pointer];
            if (currentCharacter is not ('|' or '=') || spanTrimmed.IsEscapedCharacterAtIndex(pointer)) {
                pointer++;
                continue;
            }

            switch (currentCharacter) {
                case '|': {
                    if (pointer > keyStart) {
                        if (keyEnd == -1) {
                            // No '=' found, this is a flag attribute
                            int length = spanTrimmed[keyStart..pointer].ToLowerInvariant(buffer);
                            string value = buffer[..length].ToString();
                            dictionary.AddOrUpdate(value, new Range(pointer, pointer));
                        }
                        else {
                            // Normal key=value attribute
                            int length = spanTrimmed[keyStart..keyEnd].ToLowerInvariant(buffer);
                            string value = buffer[..length].ToString();
                            dictionary.AddOrUpdate(value, new Range(valueStart, pointer));
                        }
                    }

                    keyStart = ++pointer;
                    keyEnd = -1;
                    continue;
                }

                case '=': {
                    keyEnd = pointer;
                    valueStart = ++pointer;
                    continue;
                }

                default: {
                    pointer++;
                    continue;
                }
            }
        }

        // Handle the last attribute if there is one
        if (spanLength > keyStart) {
            if (keyEnd == -1) {
                // The last attribute is a flag
                int length = spanTrimmed[keyStart..spanLength].ToLowerInvariant(buffer);
                string value = buffer[..length].ToString();
                dictionary.AddOrUpdate(value, new Range(spanLength, spanLength));
            }
            else if (valueStart < spanLength) {
                // The last attribute is key=value
                int length = spanTrimmed[keyStart..keyEnd].ToLowerInvariant(buffer);
                string value = buffer[..length].ToString();
                dictionary.AddOrUpdate(value, new Range(valueStart, spanLength));
            }
        }

        return dictionary.ToFrozenDictionary();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value) {
        if (!Attributes.TryGetValue(key, out Range range)) {
            value = null;
            return false;
        }

        ReadOnlySpan<char> original = OriginalInputSpan[range];
        Span<char> span = stackalloc char[original.Length];
        int destinationIndex = 0;

        int startIndex = original.Length > 0 && original[0] == '"' ? 1 : 0;
        int endIndex = original.Length;

        if (original.Length > 1 && original[^1] == '"') {
            int backslashCount = 0;
            for (int j = original.Length - 2; j >= 0 && original[j] == '\\'; j--) {
                backslashCount++;
            }

            if (backslashCount % 2 == 0) endIndex--;
        }

        for (int i = startIndex; i < endIndex; i++) {
            char c = original[i];
            if (i + 1 < endIndex && c == '\\' && original[i + 1] == '"' && original.IsNotEscapedCharacterAtIndex(i)) {
                i++;
                span[destinationIndex++] = original[i];
            }
            else {
                span[destinationIndex++] = c;
            }
        }

        value = new string(span[..destinationIndex]);
        return true;
    }

    // ReSharper disable once InvertIf
    /// <inheritdoc />
    public bool TryGetFlag(string key, out bool value) {
        if (!Attributes.TryGetValue(key, out Range range)) {
            value = false;
            return false;
        }

        if (range.Start.Equals(range.End)) {
            value = true;
            return true;
        }

        ReadOnlySpan<char> span = OriginalInputSpan[range];
        if (bool.TryParse(span, out value)) return true;

        // Only accept 0 or 1 as valid int values
        if (int.TryParse(span, out int intValue) && intValue is 0 or 1) {
            value = intValue != 0;
            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public void ReturnToPool() => Pool.Return(this);

    /// <inheritdoc />
    public bool TryReset() {
        InternalAttributesDictionary = new Lazy<FrozenDictionary<string, Range>>(static () => FrozenDictionary<string, Range>.Empty);
        OriginalInput = string.Empty;

        return true;
    }

    /// <inheritdoc />
    public bool Equals(IMdSyntaxNodeModifier? other) {
        // Don't need to check the attribute dictionary as it is fully dependent on the OriginalInput on creation.
        return StringComparer.Ordinal.Equals(OriginalInput, other?.OriginalInput);
    }

    #region Default Modifiers
    /// <inheritdoc />
    public bool TryGetIconName([NotNullWhen(true)] out string? iconName)
        => TryGetValue("icon", out iconName) && iconName.IsNotNullOrWhiteSpace();

    /// <inheritdoc />
    public bool TryGetTitle([NotNullWhen(true)] out string? title)
        => TryGetValue("title", out title);

    /// <inheritdoc />
    public bool TryGetSize(out (int Width, int Height) size) {
        size = (-1, -1);
        if (!Attributes.TryGetValue("size", out Range range)) return false;

        ReadOnlySpan<char> sizeValue = OriginalInputSpan[range];
        if (sizeValue.Contains('x')) {
            MemoryExtensions.SpanSplitEnumerator<char> split = sizeValue.Split('x');
            if (!split.MoveNext()) return false;

            Range firstValue = split.Current;
            if (!split.MoveNext()) return false;

            Range secondValue = split.Current;

            if (!int.TryParse(sizeValue[firstValue], out int first) || !int.TryParse(sizeValue[secondValue], out int second)) {
                return false;
            }

            size = (first, second);
            return true;
        }

        // ReSharper disable once InvertIf
        if (int.TryParse(sizeValue, out int sizeInt)) {
            size = (sizeInt, sizeInt);
            return true;
        }

        return false;
    }
    
    /// <inheritdoc />
    public bool TryGetFit(out bool state)
        => TryGetFlag("fit", out state);

    /// <inheritdoc />
    public bool TryGetAlign([NotNullWhen(true)] out string? align) {
        align = null;
        if (!TryGetValue("align", out align)) return false;

        return VerticalAlignImageUtilities.TryGetFromString(align, out _) || align.IsNotNullOrWhiteSpace();
    }

    /// <inheritdoc />
    public bool TryGetColor([NotNullWhen(true)] out string? color)
        => TryGetValue("color", out color);

    /// <inheritdoc />
    public bool TryGetStyle([NotNullWhen(true)] out string? style)
        => TryGetValue("style", out style);
    #endregion
}
