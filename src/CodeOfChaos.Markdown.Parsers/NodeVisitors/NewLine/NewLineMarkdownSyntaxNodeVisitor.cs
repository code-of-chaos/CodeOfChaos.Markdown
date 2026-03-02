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
public sealed partial class NewLineMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<NewLineMdSyntaxNode> {
    [GeneratedRegex(@"\G\n")]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;
    
    private static readonly char[] STriggerCharacters = ['\n'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        parentNode.AddChildNode(MdSyntaxNodePool<NewLineMdSyntaxNode>.Shared.Get());
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, NewLineMdSyntaxNode node) {
        queue.Enqueue('\n');
    }
}
