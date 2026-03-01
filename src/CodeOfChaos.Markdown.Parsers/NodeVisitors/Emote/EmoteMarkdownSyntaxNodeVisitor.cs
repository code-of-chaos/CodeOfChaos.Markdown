// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class EmoteMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<EmoteMdSyntaxNode> {
    [GeneratedRegex(@"\G:(?<e>[\p{L}\p{N}_]+):", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = [':'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int EmoteBodyId = RegexRule.GroupNumberFromName("e");
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string originalEmote = match.Value;
        string emoteBody = match.Groups[EmoteBodyId].Value;

        EmoteMdSyntaxNode node = MdSyntaxNodePool<EmoteMdSyntaxNode>.Shared.Get();
        node.WithOriginalEmote(originalEmote)
            .WithEmoteKey(emoteBody);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, EmoteMdSyntaxNode node) {
        queue.Enqueue(node.OriginalEmote);
    }
}
