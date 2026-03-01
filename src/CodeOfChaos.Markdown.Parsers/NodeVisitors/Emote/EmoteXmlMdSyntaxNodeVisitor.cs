// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class EmoteXmlMdSyntaxNodeVisitor : XmlSyntaxNodeVisitor<EmoteMdSyntaxNode> {
    private const string EmoteKey = nameof(EmoteMdSyntaxNode.EmoteKey);
    private const string OriginalEmote = nameof(EmoteMdSyntaxNode.OriginalEmote);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(EmoteMdSyntaxNode node, XElement targetElement) {
        base.DeserializeDetails(node, targetElement);
        targetElement.SetAttributeValue(EmoteKey, node.EmoteKey);
        targetElement.SetAttributeValue(OriginalEmote, node.OriginalEmote);
    }

    protected override void SerializeDetails(IMdSyntaxTree tree, XElement element, EmoteMdSyntaxNode targetNode) {
        base.SerializeDetails(tree, element, targetNode);
        targetNode.WithEmoteKey(element.Attribute(EmoteKey)?.Value ?? string.Empty);
        targetNode.WithOriginalEmote(element.Attribute(OriginalEmote)?.Value ?? string.Empty);
    }
}
