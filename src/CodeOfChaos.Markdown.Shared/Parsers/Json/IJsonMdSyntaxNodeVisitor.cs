// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJsonMdSyntaxNodeVisitor {
    void DeserializeToJson(IMdSyntaxNode node, Utf8JsonWriter writer);
    IMdSyntaxNode SerializeToNode(JsonElement element, IMdSyntaxNode parentNode);
}
