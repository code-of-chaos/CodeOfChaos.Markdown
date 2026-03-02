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
public sealed class ScriptingBodyMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<ScriptingBodySyntaxNode> {
    // ScriptingBody nodes don't have a regex pattern - they're created programmatically
    protected override Regex Syntax => null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) {
        // ScriptingBody nodes are not serialized from regex matches
        throw new NotSupportedException("ScriptingBody nodes are created programmatically, not from regex matches.");
    }

    protected override void Deserialize(INodeDeserializerFragmentQueue queue, ScriptingBodySyntaxNode node) {
        if (node.ChildCount == 0) return;

        // Process content line by line
        string content = queue.ProcessAsStandaloneContent(node);
        ReadOnlySpan<char> contentValue = content.AsSpan();
        int lineStart = 0;
        bool isFirstLine = true;
        string leadingSpaces = LeadingSpacesCache.GetOrAdd(Math.Max(node.LeadingSpaces, 0), static i => new string(' ', i));

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
