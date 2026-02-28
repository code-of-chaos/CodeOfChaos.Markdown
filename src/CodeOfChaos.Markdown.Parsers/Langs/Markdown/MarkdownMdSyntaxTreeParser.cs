// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown.Parsers.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IMarkdownMdSyntaxTreeParser>]
public class MarkdownMdSyntaxTreeParser(IMdStringMdSyntaxSerializer serializer, IMdStringMdSyntaxDeserializer deserializer) : IMarkdownMdSyntaxTreeParser {
    public IMdSyntaxTree SerializeToSyntaxTree(string input) => serializer.SerializeToTree(input);
    public string DeserializeToString(IMdSyntaxTree tree) => deserializer.DeserializeToString(tree);
}
