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
public sealed partial class TemplateMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TemplateMdSyntaxNode> {
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

        TemplateMdSyntaxNode node = MdSyntaxNodePool<TemplateMdSyntaxNode>.Shared.Get();
        node.WithContent(variableContent);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TemplateMdSyntaxNode node) {
        queue.Enqueue("@{");
        queue.Enqueue(node.Content);
        queue.Enqueue('}');
    }
}
