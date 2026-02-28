// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct MdSyntaxFragment(
    IMdSyntaxNode? ParentNode,
    IMdSyntaxNode? ChildNode,
    Match? Match,
    IMarkdownSyntaxNodeVisitor? NodeSerializer
) {

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static MdSyntaxFragment AsUnhandledMatch(Match match, IMdSyntaxNode node, IMarkdownSyntaxNodeVisitor nodeSerializer)
        => new(node, null, match, nodeSerializer);

    public static MdSyntaxFragment AsProcessedNode(IMdSyntaxNode parentNode, IMdSyntaxNode childNode)
        => new(parentNode, childNode, null, null);
}
