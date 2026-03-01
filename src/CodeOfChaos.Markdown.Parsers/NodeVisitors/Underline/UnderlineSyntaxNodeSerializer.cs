// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer.NodeSerializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class UnderlineSyntaxNodeSerializer : BaseMdSyntaxNodeSerializer {
    [GeneratedRegex(@"\G_(?<u>(?>[^\\_]+|\\_|__|(?<open>_)|(?<-open>_))+)(?(open)(?!))_", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['_'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int UId = RegexRule.GroupNumberFromName("u");
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(
        INodeSerializerFragmentStack stack,
        IMdSyntaxNode parentNode,
        Match match
    ) {
        string underlineValue = match.Groups[UId].Value;

        UnderlineMdSyntaxNode node = MdSyntaxNodePool<UnderlineMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);
        stack.PushSingleLineMatchesToStack(underlineValue, node);
    }
}
