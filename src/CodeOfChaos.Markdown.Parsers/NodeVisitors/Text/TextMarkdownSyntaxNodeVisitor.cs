// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class TextMarkdownSyntaxNodeVisitor : BaseMarkdownSyntaxNodeVisitor<TextMdSyntaxNode> {
    // Text nodes don't have a regex pattern - they're created programmatically
    /// <inheritdoc />
    protected override Regex Syntax => throw new NotSupportedException("TextMarkdown nodes are created programmatically, not from regex matches.");
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    // Text nodes are not serialized from regex matches
    /// <inheritdoc />
    public override bool TryGetSerializationMatch(string input, [NotNullWhen(true)] out Match? match, int startPosition = 0) {
        match = null;
        return false;
    }

    /// <inheritdoc />
    public override void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match) 
        => throw new NotSupportedException("Text nodes are created programmatically, not from regex matches.");

    /// <inheritdoc />
    protected override void Deserialize(INodeDeserializerFragmentQueue queue, TextMdSyntaxNode node) {
        queue.Enqueue(node.Content);
    }
}
