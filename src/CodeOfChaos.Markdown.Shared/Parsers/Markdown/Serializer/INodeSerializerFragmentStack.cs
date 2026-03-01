// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface INodeSerializerFragmentStack {
    void PushMultiLineMatchesToStack(string input, IMdSyntaxNode parentNode, int startIndex = 0);
    void PushSingleLineMatchesToStack(string input, IMdSyntaxNode parentNode);
    
    void PushProcessedNodeToStack(IMdSyntaxNode parentNode, IMdSyntaxNode childNode);
}
