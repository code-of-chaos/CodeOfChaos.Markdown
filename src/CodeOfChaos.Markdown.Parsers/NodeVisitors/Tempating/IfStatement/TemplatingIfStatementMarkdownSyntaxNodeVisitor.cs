// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed partial class TemplatingIfStatementMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TemplatingIfStatementMdSyntaxNode> {
    [GeneratedRegex(
        """
        \G
        ^@if\((?<ifExpression>.+)\)\ *$\s
        (?<ifBody>(?:(?!^@else\ ?if|^@else|^@endif)^.*$\s*?)*)
        (?<elifSection>
          (?:
            ^@(?:elseif|else\ if|elif)\(.+\)\ *$\s
            (?:(?:(?!^@elseif|^@else\ if|^@elif|^@else|^@endif)^.*$\s*?)+)?
          )+
        )?
        (?<elseSection>
          ^@else\ *$\s
          (?<elseBody>(?:(?!^@endif)^.*$\s*?)*)
        )?
        ^@endif\ *$
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    /// <inheritdoc />
    protected override Regex Syntax => RegexRule;

    [GeneratedRegex(
        """
        ^@(?:elseif|else\ if|elif)\(.+\)\ *$
        (?:\s(?!^@elseif|^@else\ if|^@elif|^@else|^@endif)(?:^.*$)+)+
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex ElifSectionRegexRule { get; }

    private static readonly char[] STriggerCharacters = ['@'];
    /// <inheritdoc />
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int IfExpressionId = RegexRule.GroupNumberFromName("ifExpression");
    private static readonly int IfBodyId = RegexRule.GroupNumberFromName("ifBody");
    private static readonly int ElifSectionId = RegexRule.GroupNumberFromName("elifSection");
    private static readonly int ElseSectionId = RegexRule.GroupNumberFromName("elseSection");
    private static readonly int ElseBodyId = RegexRule.GroupNumberFromName("elseBody");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        TemplatingIfStatementMdSyntaxNode statementNode = MdSyntaxNodePool<TemplatingIfStatementMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(statementNode);

        Group ifExpression = match.Groups[IfExpressionId];
        Group ifBody = match.Groups[IfBodyId];
        TemplateExpressionMdSyntaxNode ifExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledIf();
        ifExpressionNode.WithExpression(ifExpression.Value);
        statementNode.AddChildNode(ifExpressionNode);

        string adjustedIfBody = LineNormalization.NormalizeLineIndentation(ifBody.ValueSpan, out int ifLeadingSpaces);
        ifExpressionNode.WithLeadingSpaces(ifLeadingSpaces);

        stack.PushMultiLineMatchesToStack(adjustedIfBody, ifExpressionNode);

        // parse the elif section
        if (match.Groups[ElifSectionId] is { Success: true, ValueSpan: var span }) {
            foreach (ValueMatch valueMatch in ElifSectionRegexRule.EnumerateMatches(span)) {
                int index = valueMatch.Index;
                int length = valueMatch.Length;
                ReadOnlySpan<char> valueSpan = span[index..(index + length)];

                int endBracket = valueSpan[..valueSpan.IndexOf('\n')].LastIndexOf(')');
                int startBracket = valueSpan[..valueSpan.IndexOf('\n')].IndexOf('(');
                ReadOnlySpan<char> elifExpression = valueSpan[(startBracket + 1)..endBracket];
                ReadOnlySpan<char> elifBody = valueSpan[(endBracket + 2)..];

                TemplateExpressionMdSyntaxNode elifExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledElseIf();
                elifExpressionNode.WithExpression(elifExpression.ToString());
                statementNode.AddChildNode(elifExpressionNode);

                string adjustedElifBody = LineNormalization.NormalizeBlockQuote(elifBody, out int elifLeadingSpaces);
                elifExpressionNode.WithLeadingSpaces(elifLeadingSpaces);

                stack.PushMultiLineMatchesToStack(adjustedElifBody, elifExpressionNode);
            }
        }

        // parse the else section
        if (match.Groups[ElseSectionId] is not { Success: true }) return;

        Group elseBody = match.Groups[ElseBodyId];

        TemplateExpressionMdSyntaxNode elseExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledElse();
        elseExpressionNode.WithExpression("@else");
        statementNode.AddChildNode(elseExpressionNode);

        string adjustedElseBody = LineNormalization.NormalizeBlockQuote(elseBody.ValueSpan, out int elseLeadingSpaces);
        elseExpressionNode.WithLeadingSpaces(elseLeadingSpaces);

        stack.PushMultiLineMatchesToStack(adjustedElseBody, elseExpressionNode);
    }

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TemplatingIfStatementMdSyntaxNode node) {
        if (node.ChildCount == 0) return;

        foreach (TemplateExpressionMdSyntaxNode expressionNode in node.GetChildrenByType<TemplateExpressionMdSyntaxNode>()) {
            switch (expressionNode.ExpressionType) {
                case TemplateExpressionType.If: {
                    queue.Enqueue("@if(");
                    queue.Enqueue(expressionNode.Expression);
                    queue.Enqueue(")\n");

                    break;
                }

                case TemplateExpressionType.ElseIf: {
                    queue.Enqueue("@else if(");
                    queue.Enqueue(expressionNode.Expression);
                    queue.Enqueue(")\n");

                    break;
                }

                case TemplateExpressionType.Else: {
                    queue.Enqueue("@else\n");
                    break;
                }

                case TemplateExpressionType.Unknown:
                case TemplateExpressionType.Literal:
                default:
                    continue;
            }

            if (expressionNode.LeadingSpaces > 0) EnqueueFormattedLines(queue, expressionNode);
            else queue.EnqueueChildren(expressionNode);
        }

        queue.Enqueue("@endif");
    }

    private static void EnqueueFormattedLines(INodeDeserializerFragmentQueue queue, TemplateExpressionMdSyntaxNode expressionNode) {
        // Skip if we dont need to add the leading spaces
        if (expressionNode.LeadingSpaces <= 0) {
            queue.EnqueueChildren(expressionNode);
            return;
        }
        
        // Process the content line by line
        string content = queue.ProcessChildrenAsStandaloneContent(expressionNode);
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(
            Math.Max(expressionNode.LeadingSpaces, 0),
            static i => new string(' ', i)
        );
        
        int count = content.Count('\n');
        int i = 0;

        SpanLineEnumerator enumerator = content.EnumerateLines();
        while (enumerator.MoveNext() && i <= count)  {
            // if we are the last item don't add the leading spaces because everything will then be an empty line
            if (i++ == count) {
                queue.Enqueue(enumerator.Current);
                break;
            }
            
            queue.Enqueue(leadingSpaces);
            queue.Enqueue(enumerator.Current);
            queue.Enqueue('\n');
        }
    }
}
