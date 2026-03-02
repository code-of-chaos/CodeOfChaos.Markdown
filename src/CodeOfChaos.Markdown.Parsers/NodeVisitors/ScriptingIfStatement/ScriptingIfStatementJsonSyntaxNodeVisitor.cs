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
public sealed class ScriptingIfStatementJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<ScriptingIfStatementSyntaxNode> {
    private static readonly string ElseConditionIndex = nameof(ScriptingIfStatementSyntaxNode.ElseConditionIndex).ToCamelCase();
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(ScriptingIfStatementSyntaxNode node, Utf8JsonWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteNumber(ElseConditionIndex, node.ElseConditionIndex);
    }
    protected override void SerializeDetails(JsonElement element, ScriptingIfStatementSyntaxNode targetNode) {
        base.SerializeDetails(element, targetNode);

        if (TryGetPropertyAsInt32(element, ElseConditionIndex, out int elseConditionIndex)) {
            targetNode.WithElseConditionIndex(elseConditionIndex);
        }
    }
}
