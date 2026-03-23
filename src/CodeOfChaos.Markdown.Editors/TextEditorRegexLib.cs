// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Editors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A static library that defines precompiled regular expressions for text editing operations.
/// </summary>
/// <remarks>
/// This class provides a set of regular expressions tailored for parsing and manipulating
/// structured text content such as lists, tables, and other markdown-like constructs.
/// The regular expressions are precompiled for performance optimization.
/// </remarks>
public static partial class TextEditorRegexLib {
    [GeneratedRegex(@"^( *(?:- |\d+\. |>[>\-\d. ]* )).*$", RegexOptions.Compiled)]
    public static partial Regex ListItemsRegex { get; }

    [GeneratedRegex(@"^(?!\s*\|?\s*-+\s*\|)(.*\|.*)$", RegexOptions.Compiled)]
    public static partial Regex IsTableLineRegex { get; }

    [GeneratedRegex(@"(?!\|)(\b[^\|\-\r\n]+\b)(?:(?=\|?))", RegexOptions.Compiled)]
    public static partial Regex TableCellsRegex { get; }

    [GeneratedRegex(@"\|[\|\-:\ ]+\|", RegexOptions.Compiled)]
    public static partial Regex IsTableHeaderLineRegex { get; }
}
