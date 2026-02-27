// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer.NodeSerializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class HorizontalRuleSyntaxNodeSerializer : BaseMdSyntaxNodeSerializer {
    [GeneratedRegex(@"\G^(?<hr>\ *?(\-{3,}?|_{3,}?)\ *?)$", DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['-', ' ', '_'];
    public override ReadOnlySpan<char> TriggerCharacters => STriggerCharacters;

    private static readonly int HrId = RegexRule.GroupNumberFromName("hr");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(
        IMdSyntaxFragmentStack stack,
        IMdSyntaxNode parentNode,
        Match match
    ) {
        string hrContent = match.Groups[HrId].Value;

        HorizontalRuleMdSyntaxNode node = MdSyntaxNodePool<HorizontalRuleMdSyntaxNode>.Shared.Get();
        node.WithIdentifier(hrContent);
        parentNode.AddChildNode(node);
    }
}
