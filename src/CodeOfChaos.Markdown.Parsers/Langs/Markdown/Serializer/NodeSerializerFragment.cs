// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Syntax;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a queued input fragment for markdown serialization.
/// </summary>
/// <param name="ParentNode">The parent node being processed.</param>
/// <param name="ChildNode">The child node being produced.</param>
/// <param name="Match">The regex match associated with this fragment.</param>
/// <param name="NodeSerializer">The serializer that produced or should handle the fragment.</param>
public readonly record struct NodeSerializerFragment(
    IMdSyntaxNode? ParentNode,
    IMdSyntaxNode? ChildNode,
    Match? Match,
    IMarkdownSyntaxNodeVisitor? NodeSerializer
) {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static NodeSerializerFragment AsUnhandledMatch(Match match, IMdSyntaxNode node, IMarkdownSyntaxNodeVisitor nodeSerializer)
        => new(node, null, match, nodeSerializer);

    public static NodeSerializerFragment AsProcessedNode(IMdSyntaxNode parentNode, IMdSyntaxNode childNode)
        => new(parentNode, childNode, null, null);
}
