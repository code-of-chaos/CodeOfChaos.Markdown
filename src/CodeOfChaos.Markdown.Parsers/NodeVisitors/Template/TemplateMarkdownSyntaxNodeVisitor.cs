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
    [GeneratedRegex(@"\G(?<!\])(?<open>\{)+(?<t>[^\s{}]+)(?<-open>\})+(?(open)(?!))", DefaultSingleLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    private static readonly char[] STriggerCharacters = ['{'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int TemplateContentId = RegexRule.GroupNumberFromName("t");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        ReadOnlySpan<char> variableContent = match.Groups[TemplateContentId].ValueSpan;
        int variableLength = match.Length;

        TemplateMdSyntaxNode node = MdSyntaxNodePool<TemplateMdSyntaxNode>.Shared.Get();
        node.WithContent(variableContent.ToString())
            .WithBracesCount((variableLength - variableContent.Length) / 2);
        parentNode.AddChildNode(node);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TemplateMdSyntaxNode node) {
        queue.Enqueue('{', Math.Max(node.BracesCount, 1));
        queue.Enqueue(node.Content);
        queue.Enqueue('}', Math.Max(node.BracesCount, 1));
    }
}
