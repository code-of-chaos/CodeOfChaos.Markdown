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
public sealed partial class FootnoteReferenceMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<FootnoteReferenceMdSyntaxNode> {
    [GeneratedRegex(@"\G\[\^(?<id>[\d\p{L}\p{N}]+)\]", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['['];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int FootnoteIdentifierId = RegexRule.GroupNumberFromName("id");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string footnoteId = match.Groups[FootnoteIdentifierId].Value;

        FootnoteReferenceMdSyntaxNode node = MdSyntaxNodePool<FootnoteReferenceMdSyntaxNode>.Shared.Get();
        node.WithIdentifier(footnoteId);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, FootnoteReferenceMdSyntaxNode node) {
        queue.Enqueue('[');
        queue.Enqueue('^');
        queue.Enqueue(node.Identifier);
        queue.Enqueue(']');
    }
}
