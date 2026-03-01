// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Text.RegularExpressions;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer.NodeSerializers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed partial class EscapedCharacterSyntaxNodeSerializer : BaseMdSyntaxNodeSerializer {
    [GeneratedRegex(@"\G\\\S")]
    private static partial Regex RegexRule { get; }
    protected override Regex Syntax { get; } = RegexRule;

    public override ReadOnlySpan<char> SerializationTriggerCharacters => ['\\'];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override void Serialize(
        INodeSerializerFragmentStack stack,
        IMdSyntaxNode parentNode,
        Match match
    ) {
        char value = match.ValueSpan[1];
        EscapedCharacterMdSyntaxNode node = MdSyntaxNodePool<EscapedCharacterMdSyntaxNode>.Shared.Get();
        node.WithContent(value);
        parentNode.AddChildNode(node);
    }
}
