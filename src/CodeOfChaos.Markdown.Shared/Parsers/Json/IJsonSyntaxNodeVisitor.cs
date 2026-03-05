// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJsonSyntaxNodeVisitor {
    void DeserializeToJson(IMdSyntaxNode node, Utf8JsonWriter writer);
    IMdSyntaxNode SerializeToNode(JsonElement element, IMdSyntaxNode parentNode);
}

// ReSharper disable once UnusedTypeParameter
public interface IJsonSyntaxNodeVisitor<TSyntaxNode> : IJsonSyntaxNodeVisitor where TSyntaxNode : class, IMdSyntaxNode;
