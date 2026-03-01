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
public sealed partial class WrapperMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<WrapperMdSyntaxNode> {
    [GeneratedRegex(@"\G<(?<mods>\|.*?)>(?<w>.*)</>", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly int WId = RegexRule.GroupNumberFromName("w");
    private static readonly int WModsId = RegexRule.GroupNumberFromName("mods");

    private static readonly char[] STriggerCharacters = ['<'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> wrapperValue = match.Groups[WId].ValueSpan;
        ReadOnlySpan<char> mods = match.Groups[WModsId].ValueSpan;// Mods are required for this match

        WrapperMdSyntaxNode node = MdSyntaxNodePool<WrapperMdSyntaxNode>.Shared.Get();
        node.WithModifier(MdSyntaxNodeModifier.FromString(mods.ToString()));
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(wrapperValue.ToString(), node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, WrapperMdSyntaxNode node) {
        queue.Enqueue('<');
        queue.Enqueue(node.Modifier!.OriginalInputSpan);
        queue.Enqueue('>');
        queue.EnqueueChildren(node);
        queue.Enqueue("</>");
    }
}
