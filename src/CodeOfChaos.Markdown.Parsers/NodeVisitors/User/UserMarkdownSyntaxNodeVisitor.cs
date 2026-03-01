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
public sealed partial class UserMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<UserMdSyntaxNode> {
    [GeneratedRegex(@"\G\@(?<u>[\p{L}\p{N}\-_\/\.]+)", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int UsernameId = RegexRule.GroupNumberFromName("u");

    private static readonly char[] STriggerCharacters = ['@'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> username = match.Groups[UsernameId].ValueSpan;

        UserMdSyntaxNode node = MdSyntaxNodePool<UserMdSyntaxNode>.Shared.Get();
        node.WithContent(username.ToString());
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, UserMdSyntaxNode node) {
        queue.Enqueue('@');
        queue.Enqueue(node.Content);
    }
}
