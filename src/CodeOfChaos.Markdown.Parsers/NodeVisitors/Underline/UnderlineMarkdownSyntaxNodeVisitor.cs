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
public sealed partial class UnderlineMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<UnderlineMdSyntaxNode> {
    [GeneratedRegex(@"\G_(?<u>(?>[^\\_]+|\\_|__|(?<open>_)|(?<-open>_))+)(?(open)(?!))_", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['_'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int UId = RegexRule.GroupNumberFromName("u");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string underlineValue = match.Groups[UId].Value;

        UnderlineMdSyntaxNode node = MdSyntaxNodePool<UnderlineMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(underlineValue, node);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, UnderlineMdSyntaxNode node) {
        queue.Enqueue('_');
        queue.EnqueueChildren(node);
        queue.Enqueue('_');
    }
}
