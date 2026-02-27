// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Markdown.Parsers.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMdSyntaxFragmentStack {
    void PushMultiLineMatchesToStack(string input, IMdSyntaxNode parentNode, int startIndex = 0);
    void PushSingleLineMatchesToStack(string input, IMdSyntaxNode parentNode);
    
    void PushProcessedNodeToStack(IMdSyntaxNode parentNode, IMdSyntaxNode childNode);
}
