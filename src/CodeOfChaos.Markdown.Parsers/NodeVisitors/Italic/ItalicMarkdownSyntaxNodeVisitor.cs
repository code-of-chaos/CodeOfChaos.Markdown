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
/// <inheritdoc />
public sealed partial class ItalicMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ItalicMdSyntaxNode> {
    [GeneratedRegex(@"\G\*(?<i>(?>[^\\\*]+|\\\*|\*\*|(?<open>\*)|(?<-open>\*))+)(?(open)(?!))\*", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['*'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int ItalicContentId = RegexRule.GroupNumberFromName("i");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string italicValue = match.Groups[ItalicContentId].Value;

        ItalicMdSyntaxNode node = MdSyntaxNodePool<ItalicMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(italicValue, node);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ItalicMdSyntaxNode node) {
        queue.Enqueue('*');
        queue.EnqueueChildren(node);
        queue.Enqueue('*');
    }
}
