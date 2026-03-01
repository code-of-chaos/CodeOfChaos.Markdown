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
public sealed partial class TagMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TagMdSyntaxNode> {
    [GeneratedRegex(@"\G\#(?<t>[\p{L}\p{N}\-_\/\.]+)", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['#'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int TextId = RegexRule.GroupNumberFromName("t");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string tagValue = match.Groups[TextId].Value;

        TagMdSyntaxNode node = MdSyntaxNodePool<TagMdSyntaxNode>.Shared.Get();
        node.WithContent(tagValue);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TagMdSyntaxNode node) {
        queue.Enqueue('#');
        queue.Enqueue(node.Content);
    }
}
