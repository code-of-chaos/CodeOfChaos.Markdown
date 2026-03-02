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
public sealed partial class ScriptingIfStatementMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ScriptingIfStatementMdSyntaxNode> {
    [GeneratedRegex("""
        \G
        ^@if\((?<ifExpression>.+)\)\ *$\s
        (?<ifBody>(?:(?!^@elseif|^@else|^@endif)^.*$\s*?)*)
        (?<elifSection>
          (?:
            ^@elseif\(.+\)\ *$\s
            (?:(?:(?!^@elseif|^@else|^@endif)^.*$\s*?)+)?
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
        ^@elseif\(.+\)\ *$
        (?:\s(?!^@elseif|^@else|^@endif)(?:^.*$)+)+
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
        ScriptingIfStatementMdSyntaxNode statementNode = MdSyntaxNodePool<ScriptingIfStatementMdSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(statementNode);

        Group ifExpression = match.Groups[IfExpressionId];
        Group ifBody = match.Groups[IfBodyId];
        ScriptingExpressionMdSyntaxNode ifExpressionNode = MdSyntaxNodePool<ScriptingExpressionMdSyntaxNode>.Shared.Get();
        ifExpressionNode.WithExpression($"@if({ifExpression.Value})", ifExpression.Index, ifExpression.Length);
        statementNode.WithIfCondition(ifExpressionNode);

        string adjustedIfBody = LineNormalization.NormalizeBlockQuote(ifBody.ValueSpan, out int ifBodyLeadingSpaces);
        ScriptingBodyMdSyntaxNode ifBodyNode = MdSyntaxNodePool<ScriptingBodyMdSyntaxNode>.Shared.Get();
        ifBodyNode.WithLeadingSpaces(ifBodyLeadingSpaces);
        ifExpressionNode.AddChildNode(ifBodyNode);

        stack.PushMultiLineMatchesToStack(adjustedIfBody, ifBodyNode);

        // parse the elif section
        if (match.Groups[ElifSectionId] is { Success: true, ValueSpan: var span }) {
            foreach (ValueMatch valueMatch in ElifSectionRegexRule.EnumerateMatches(span)) {
                int index = valueMatch.Index;
                int length = valueMatch.Length;
                ReadOnlySpan<char> valueSpan = span[index..(index + length)];

                int endBracket = valueSpan[..valueSpan.IndexOf('\n')].LastIndexOf(')');
                ReadOnlySpan<char> elifExpression = valueSpan[8..endBracket];
                ReadOnlySpan<char> elifBody = valueSpan[(endBracket + 2)..];

                ScriptingExpressionMdSyntaxNode elifExpressionNode = MdSyntaxNodePool<ScriptingExpressionMdSyntaxNode>.Shared.Get();
                elifExpressionNode.WithExpression($"@elseif({elifExpression})", 8, endBracket - 8);
                statementNode.WithIfCondition(elifExpressionNode);

                string adjustedElifBody = LineNormalization.NormalizeBlockQuote(elifBody, out int elifBodyLeadingSpaces);
                ScriptingBodyMdSyntaxNode elifBodyNode = MdSyntaxNodePool<ScriptingBodyMdSyntaxNode>.Shared.Get();
                elifBodyNode.WithLeadingSpaces(elifBodyLeadingSpaces);
                elifExpressionNode.AddChildNode(elifBodyNode);

                stack.PushMultiLineMatchesToStack(adjustedElifBody, elifBodyNode);
            }
        }

        // parse the else section
        if (match.Groups[ElseSectionId] is { Success: true }) {
            Group elseBody = match.Groups[ElseBodyId];

            ScriptingExpressionMdSyntaxNode elseExpressionNode = MdSyntaxNodePool<ScriptingExpressionMdSyntaxNode>.Shared.Get();
            elseExpressionNode.WithExpression("@else", 5, 0);
            statementNode.WithElseCondition(elseExpressionNode);

            string adjustedElseBody = LineNormalization.NormalizeBlockQuote(elseBody.ValueSpan, out int elseBodyLeadingSpaces);
            ScriptingBodyMdSyntaxNode elseBodyNode = MdSyntaxNodePool<ScriptingBodyMdSyntaxNode>.Shared.Get();
            elseBodyNode.WithLeadingSpaces(elseBodyLeadingSpaces);
            elseExpressionNode.AddChildNode(elseBodyNode);

            stack.PushMultiLineMatchesToStack(adjustedElseBody, elseBodyNode);
        }
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ScriptingIfStatementMdSyntaxNode node) {
        queue.EnqueueChildren(node);
        queue.Enqueue("@endif");
        queue.Enqueue('\n');
    }
}
