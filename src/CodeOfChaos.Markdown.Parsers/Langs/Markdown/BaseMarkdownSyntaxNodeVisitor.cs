// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class BaseMarkdownSyntaxNodeVisitor<TSyntaxNode> : IMarkdownSyntaxNodeVisitor<TSyntaxNode>
    where TSyntaxNode : MdSyntaxNode<TSyntaxNode>, new() {
    protected const RegexOptions DefaultSingleLineRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace;
    protected const RegexOptions DefaultMultiLineRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;
    
    protected abstract Regex Syntax { get; }
    public virtual ReadOnlySpan<char> SerializationTriggerCharacters => default;

    // ReSharper disable once StaticMemberInGenericType
    protected static ConcurrentDictionary<int, string> LeadingSpacesCache { get; } = new() {
        [0] = string.Empty,
        [1] = " ",
        [2] = "  ",
        [3] = "   ",
        [4] = "    ",
        [5] = "     ",
        [6] = "      ",
        [7] = "       ",
        [8] = "        ",
        [9] = "         "
    };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    #region Serialize
    public bool TryGetSerializationMatch(string input, [NotNullWhen(true)] out Match? match, int startPosition = 0) {
        match = null;
        if (startPosition >= input.Length) return false;
        if (input.IsNullOrEmpty()) return false;
        
        match = Syntax.Match(input, startPosition);
        return match.Success;
    }
    public abstract void Serialize(INodeSerializerFragmentStack stack, IMdSyntaxNode parentNode, Match match);
    #endregion
    
    #region Deserialize
    public void Deserialize(INodeDeserializerFragmentQueue queue, IMdSyntaxNode node) {
        if (node is not TSyntaxNode typedNode) throw new ArgumentException($"Invalid node type of {node.GetType()} did adhere to {typeof(TSyntaxNode)}");

        Deserialize(queue, typedNode);
    }

    protected abstract void Deserialize(INodeDeserializerFragmentQueue queue, TSyntaxNode node);
    #endregion
}
