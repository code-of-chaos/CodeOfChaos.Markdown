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
public sealed partial class TemplatingIfStatementMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TemplatingIfStatementMdSyntaxNode> {
    [GeneratedRegex(
        """
        \G
        ^@if\((?<ifExpression>.+)\)\ *$\s
        (?<ifBody>(?:(?!^@else\ ?if|^@else|^@endif)^.*$\s*?)*)
        (?<elifSection>
          (?:
            ^@else\ ?if\(.+\)\ *$\s
            (?:(?:(?!^@else\ ?if|^@else|^@endif)^.*$\s*?)+)?
          )+
        )?
        (?<elseSection>
          ^@else\ *$\s
          (?<elseBody>(?:(?!^@endif)^.*$\s*?)*)
        )?
        ^@endif\ *$
        """, DefaultMultiLineRegexOptions)]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax => RegexRule;

    [GeneratedRegex("""
                    ^@else\ ?if\(.+\)\ *$
                    (?:\s(?!^@else\ ?if|^@else|^@endif)(?:^.*$)+)+
                    """, DefaultMultiLineRegexOptions)]
    private static partial Regex ElifSectionRegexRule { get; }

    private static readonly char[] STriggerCharacters = ['@'];
    public override ReadOnlySpan<char> SerializationTriggerCharacters => STriggerCharacters;

    private static readonly int IfExpressionId = RegexRule.GroupNumberFromName("ifExpression");
    private static readonly int IfBodyId = RegexRule.GroupNumberFromName("ifBody");
    private static readonly int ElifSectionId = RegexRule.GroupNumberFromName("elifSection");
    private static readonly int ElseSectionId = RegexRule.GroupNumberFromName("elseSection");
    private static readonly int ElseBodyId = RegexRule.GroupNumberFromName("elseBody");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        TemplatingIfStatementMdSyntaxNode statementNode = MdSyntaxNodePool<TemplatingIfStatementMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(statementNode);

        Group ifExpression = match.Groups[IfExpressionId];
        Group ifBody = match.Groups[IfBodyId];
        TemplateExpressionMdSyntaxNode ifExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledIf();
        ifExpressionNode.WithExpression(ifExpression.Value);
        statementNode.AddChildNode(ifExpressionNode);

        string adjustedIfBody = LineNormalization.NormalizeLineIndentation(ifBody.ValueSpan, out int ifBodyLeadingSpaces);
        ifExpressionNode.WithBodyLeadingSpaces(ifBodyLeadingSpaces);

        stack.PushMultiLineMatchesToStack(adjustedIfBody, ifExpressionNode);

        // parse the elif section
        if (match.Groups[ElifSectionId] is { Success: true, ValueSpan: var span }) {
            foreach (ValueMatch valueMatch in ElifSectionRegexRule.EnumerateMatches(span)) {
                int index = valueMatch.Index;
                int length = valueMatch.Length;
                ReadOnlySpan<char> valueSpan = span[index..(index + length)];

                int endBracket = valueSpan[..valueSpan.IndexOf('\n')].LastIndexOf(')');
                int startBracket = valueSpan[..valueSpan.IndexOf('\n')].IndexOf('(');
                ReadOnlySpan<char> elifExpression = valueSpan[(startBracket+1)..endBracket];
                ReadOnlySpan<char> elifBody = valueSpan[(endBracket + 2)..];

                TemplateExpressionMdSyntaxNode elifExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledElseIf();
                elifExpressionNode.WithExpression(elifExpression.ToString());
                statementNode.AddChildNode(elifExpressionNode);

                string adjustedElifBody = LineNormalization.NormalizeBlockQuote(elifBody, out int elifBodyLeadingSpaces);
                elifExpressionNode.WithBodyLeadingSpaces(elifBodyLeadingSpaces);

                stack.PushMultiLineMatchesToStack(adjustedElifBody, elifExpressionNode);
            }
        }

        // parse the else section
        if (match.Groups[ElseSectionId] is not { Success: true }) return;

        Group elseBody = match.Groups[ElseBodyId];

        TemplateExpressionMdSyntaxNode elseExpressionNode = TemplateExpressionMdSyntaxNode.GetPooledElse();
        elseExpressionNode.WithExpression("@else");
        statementNode.AddChildNode(elseExpressionNode);

        string adjustedElseBody = LineNormalization.NormalizeBlockQuote(elseBody.ValueSpan, out int elseBodyLeadingSpaces);
        elseExpressionNode.WithBodyLeadingSpaces(elseBodyLeadingSpaces);

        stack.PushMultiLineMatchesToStack(adjustedElseBody, elseExpressionNode);
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TemplatingIfStatementMdSyntaxNode node) {
        if (node.ChildCount == 0) return;

        foreach (TemplateExpressionMdSyntaxNode expressionNode in node.GetChildrenByType<TemplateExpressionMdSyntaxNode>()) {
            switch (expressionNode.ExpressionType) {
                case TemplateExpressionMdSyntaxNode.TemplateExpressionType.If: {
                    queue.Enqueue("@if(");
                    queue.Enqueue(expressionNode.Expression);
                    queue.Enqueue(")\n");

                    break;
                }

                case TemplateExpressionMdSyntaxNode.TemplateExpressionType.ElseIf: {
                    queue.Enqueue("@else if(");
                    queue.Enqueue(expressionNode.Expression);
                    queue.Enqueue(")\n");

                    break;
                }

                case TemplateExpressionMdSyntaxNode.TemplateExpressionType.Else: {
                    queue.Enqueue("@else\n");
                    break;
                }

                case TemplateExpressionMdSyntaxNode.TemplateExpressionType.Unknown:
                case TemplateExpressionMdSyntaxNode.TemplateExpressionType.Literal:
                default:
                    continue;
            }

            if (expressionNode.BodyLeadingSpaces > 0) EnqueueFormattedLines(queue, expressionNode);
            else queue.EnqueueChildren(expressionNode);
        }

        queue.Enqueue("@endif");
    }

    private static void EnqueueFormattedLines(INodeDeserializerFragmentQueue queue, TemplateExpressionMdSyntaxNode expressionNode) {
        // Process content line by line without creating an array

        string content = queue.ProcessChildrenAsStandaloneContent(expressionNode);
        ReadOnlySpan<char> contentValue = content.AsSpan();
        int lineStart = 0;
        bool isFirstLine = true;
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(expressionNode.BodyLeadingSpaces, 0), static i => new string(' ', i));

        for (int i = 0; i <= contentValue.Length; i++) {
            if (i != contentValue.Length && contentValue[i] != '\n') continue;

            ReadOnlySpan<char> line = contentValue.Slice(lineStart, i - lineStart);
            if (!isFirstLine) queue.Enqueue('\n');
            queue.Enqueue(leadingSpaces);

            if (line.IsEmpty) queue.Enqueue(' ');
            else queue.Enqueue(line);

            // Move to the next line
            lineStart = i + 1;
            isFirstLine = false;
        }
    }
}
