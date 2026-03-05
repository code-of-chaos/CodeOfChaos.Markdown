// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class EmoteXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<EmoteMdSyntaxNode> {
    private const string EmoteKey = nameof(EmoteMdSyntaxNode.EmoteKey);
    private const string OriginalEmote = nameof(EmoteMdSyntaxNode.OriginalEmote);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(EmoteMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);
        writer.WriteAttributeString(EmoteKey, node.EmoteKey);
        writer.WriteAttributeString(OriginalEmote, node.OriginalEmote);
    }

    protected override void SerializeDetails(XmlReader reader, EmoteMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, EmoteKey, out string? emoteKey)) {
            targetNode.WithEmoteKey(emoteKey);
        }
        
        if (TryGetAttributeAsString(reader, OriginalEmote, out string? originalEmote)) {
            targetNode.WithOriginalEmote(originalEmote);
        }
    }
}


