// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class HeadingJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<HeadingMdSyntaxNode> {
    private static readonly string Level = nameof(HeadingMdSyntaxNode.Level).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(HeadingMdSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteNumber(Level, node.Level);
    }

    protected override void SerializeDetails(JsonElement element, HeadingMdSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsInt32(element, Level, out int level)) {
            targetNode.WithLevel(level);
        }
    }
}
