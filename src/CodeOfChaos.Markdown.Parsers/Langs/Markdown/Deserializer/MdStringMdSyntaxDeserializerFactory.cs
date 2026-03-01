// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class MdStringMdSyntaxDeserializerFactory {
    public static IMdStringMdSyntaxDeserializer CreateDeserializer(IServiceProvider provider) {
        var instance = ActivatorUtilities.CreateInstance<MdStringMdSyntaxDeserializer>(provider);

        Dictionary<Type, IMarkdownSyntaxNodeVisitor> deserializers = new Dictionary<Type, IMarkdownSyntaxNodeVisitor>()
            .Register<BlockQuoteMdSyntaxNode, BlockQuoteMarkdownSyntaxNodeVisitor>(instance)
            .Register<BoldMdSyntaxNode, BoldMarkdownSyntaxNodeVisitor>(instance)
            .Register<BreakMdSyntaxNode, BreakMarkdownSyntaxNodeVisitor>(instance)
            // .Register<CalloutBodyMdSyntaxNode, CalloutBodyMarkdownSyntaxNodeVisitor>(instance) // Not implemented due to the CalloutMarkdownSyntaxNodeVisitor handling them directly
            .Register<CalloutMdSyntaxNode, CalloutMarkdownSyntaxNodeVisitor>(instance)
            // .Register<CalloutTitleMdSyntaxNode, CalloutTitleMarkdownSyntaxNodeVisitor>(instance) // Not implemented due to the CalloutMarkdownSyntaxNodeVisitor handling them directly
            .Register<CodeBlockMdSyntaxNode, CodeBlockMarkdownSyntaxNodeVisitor>(instance)
            .Register<CodeInlineMdSyntaxNode, CodeInlineMarkdownSyntaxNodeVisitor>(instance)
            .Register<EmoteMdSyntaxNode, EmoteMarkdownSyntaxNodeVisitor>(instance)
            .Register<EscapedCharacterMdSyntaxNode, EscapedCharacterMarkdownSyntaxNodeVisitor>(instance)
            .Register<FootnoteDescriptionMdSyntaxNode, FootnoteDescriptionMarkdownSyntaxNodeVisitor>(instance)
            .Register<FootnoteReferenceMdSyntaxNode, FootnoteReferenceMarkdownSyntaxNodeVisitor>(instance)
            .Register<FrontMatterMdSyntaxNode, FrontMatterMarkdownSyntaxNodeVisitor>(instance)
            .Register<HeadingMdSyntaxNode, HeadingMarkdownSyntaxNodeVisitor>(instance)
            .Register<HeadingSimpleMdSyntaxNode, HeadingSimpleMarkdownSyntaxNodeVisitor>(instance)
            .Register<HighlightMdSyntaxNode, HighlightMarkdownSyntaxNodeVisitor>(instance)
            .Register<HorizontalRuleMdSyntaxNode, HorizontalRuleMarkdownSyntaxNodeVisitor>(instance)
            .Register<HtmlMdSyntaxNode, HtmlMarkdownSyntaxNodeVisitor>(instance)
            .Register<HtmlSpanMdSyntaxNode, HtmlSpanMarkdownSyntaxNodeVisitor>(instance)
            .Register<ImageMdSyntaxNode, ImageMarkdownSyntaxNodeVisitor>(instance)
            .Register<ItalicMdSyntaxNode, ItalicMarkdownSyntaxNodeVisitor>(instance)
            .Register<LinkMdSyntaxNode, LinkMarkdownSyntaxNodeVisitor>(instance)
            .Register<ListItemMdSyntaxNode, ListItemMarkdownSyntaxNodeVisitor>(instance)
            .Register<ListOrderedMdSyntaxNode, ListOrderedMarkdownSyntaxNodeVisitor>(instance)
            .Register<ListUnOrderedMdSyntaxNode, ListUnorderedMarkdownSyntaxNodeVisitor>(instance)
            .Register<NewLineMdSyntaxNode, NewLineMarkdownSyntaxNodeVisitor>(instance)
            .Register<ParagraphMdSyntaxNode, ParagraphMarkdownSyntaxNodeVisitor>(instance)
            // .Register<RootMdSyntaxNode, RootMarkdownSyntaxNodeVisitor>(instance) // Is a semantic node and cannot be processed
            .Register<ScriptingBodySyntaxNode, ScriptingBodyMarkdownSyntaxNodeVisitor>(instance)
            .Register<ScriptingExpressionSyntaxNode, ScriptingExpressionMarkdownSyntaxNodeVisitor>(instance)
            .Register<ScriptingIfStatementSyntaxNode, ScriptingIfStatementMarkdownSyntaxNodeVisitor>(instance)
            .Register<StrikeMdSyntaxNode, StrikeMarkdownSyntaxNodeVisitor>(instance)
            .Register<SubScriptMdSyntaxNode, SubScriptMarkdownSyntaxNodeVisitor>(instance)
            .Register<SuperScriptMdSyntaxNode, SuperScriptMarkdownSyntaxNodeVisitor>(instance)
            // .Register<TableCellMdSyntaxNode, TableCellMarkdownSyntaxNodeVisitor>(instance) // Not implemented due to the TableMarkdownSyntaxNodeVisitor handling them directly
            .Register<TableMdSyntaxNode, TableMarkdownSyntaxNodeVisitor>(instance)
            // .Register<TableRowMdSyntaxNode, TableRowMarkdownSyntaxNodeVisitor>(instance) // Not implemented due to the TableMarkdownSyntaxNodeVisitor handling them directly
            .Register<TagMdSyntaxNode, TagMarkdownSyntaxNodeVisitor>(instance)
            .Register<TemplateMdSyntaxNode, TemplateMarkdownSyntaxNodeVisitor>(instance)
            .Register<TextMdSyntaxNode, TextMarkdownSyntaxNodeVisitor>(instance)
            .Register<UnderlineMdSyntaxNode, UnderlineMarkdownSyntaxNodeVisitor>(instance)
            .Register<UserMdSyntaxNode, UserMarkdownSyntaxNodeVisitor>(instance)
            .Register<WikiLinkMdSyntaxNode, WikiLinkMarkdownSyntaxNodeVisitor>(instance)
            .Register<WrapperMdSyntaxNode, WrapperMarkdownSyntaxNodeVisitor>(instance)
            ;

        instance.Deserializers = deserializers.ToFrozenDictionary();
        return instance;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static Dictionary<Type, IMarkdownSyntaxNodeVisitor> Register<TNode, TDeserializer>(
        this Dictionary<Type, IMarkdownSyntaxNodeVisitor> deserializers,
        IMdStringMdSyntaxDeserializer instance
    ) where TDeserializer : BaseMarkdownSyntaxNodeVisitor<TNode>, new() where TNode : IMdSyntaxNode {
        deserializers.AddOrUpdate(typeof(TNode), new TDeserializer {
            Deserializer = instance
        });
        return deserializers;
    }
}
