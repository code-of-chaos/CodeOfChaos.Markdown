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
public sealed partial class FrontMatterMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<FrontMatterMdSyntaxNode> {
    [GeneratedRegex(@"\G(?<open>^-{3,})\ *(?<lang>.+)?\n(?<body>[\s\S]*?)\n\k<open>", DefaultMultiLineRegexOptions)]
    internal static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['-'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int LangId = RegexRule.GroupNumberFromName("lang");
    private static readonly int BodyId = RegexRule.GroupNumberFromName("body");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        FrontMatterMdSyntaxNode node = MdSyntaxNodePool<FrontMatterMdSyntaxNode>.Shared.Get();
        if (match.Groups[LangId].TryGetValue(out string? lang)) node.WithLanguage(lang);
        if (match.Groups[BodyId].TryGetValue(out string? body)) node.WithContent(body);

        ReadOnlySpan<char> span = match.ValueSpan;
        int dashCount = 0;
        int spaceCount = 0;
        foreach (char t in span) {
            switch (t) {
                case '-':
                    dashCount++;
                    continue;
                case ' ':
                    spaceCount++;
                    continue;
            }
            break;
        }

        node.WithDashesCount(dashCount);
        node.WithLeadingSpaces(spaceCount);

        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, FrontMatterMdSyntaxNode node) {
        queue.Enqueue('-', node.DashesCount);
        queue.Enqueue(' ', node.LeadingSpaces);
        queue.Enqueue(node.Language);
        queue.Enqueue('\n');
        queue.Enqueue(node.Content);
        queue.Enqueue('\n');
        queue.Enqueue('-', node.DashesCount);
    }
}
