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
