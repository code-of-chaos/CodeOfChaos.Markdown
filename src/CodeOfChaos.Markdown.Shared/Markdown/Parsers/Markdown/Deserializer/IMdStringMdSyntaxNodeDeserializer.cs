// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Syntax;
using System.Text;

namespace CodeOfChaos.Markdown.Markdown.Parsers.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IMdStringMdSyntaxNodeDeserializer{
    void Deserialize(IMdSyntaxNode node, StringBuilder builder);
}
