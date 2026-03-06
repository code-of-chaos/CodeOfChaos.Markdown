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
public sealed partial class TemplatingLiteralStatementMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TemplatingLiteralStatementMdSyntaxNode> {
    [GeneratedRegex(@"\G@{(?<t>(?>[^\s\\{}]+|\\{|\\}|{})+)}", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['@'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int TemplateContentId = RegexRule.GroupNumberFromName("t");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        string variableContent = match.Groups[TemplateContentId].Value;

        TemplatingLiteralStatementMdSyntaxNode node = MdSyntaxNodePool<TemplatingLiteralStatementMdSyntaxNode>.Shared.Get();
        node.WithLiteralBody(variableContent);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TemplatingLiteralStatementMdSyntaxNode node) {
        if (!node.TryGetChildAt(0, out TemplateExpressionMdSyntaxNode? childNode)) return;

        queue.Enqueue("@{");
        queue.Enqueue(childNode.Expression);
        queue.Enqueue('}');
    }
}
