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
            .Register<BlockQuoteMdSyntaxNode, BlockQuoteMarkdownSyntaxNodeVisitor>()
            .Register<BoldMdSyntaxNode, BoldMarkdownSyntaxNodeVisitor>()
            .Register<BreakMdSyntaxNode, BreakMarkdownSyntaxNodeVisitor>()
            // .Register<CalloutBodyMdSyntaxNode, CalloutBodyMarkdownSyntaxNodeVisitor>() // Not implemented due to the CalloutMarkdownSyntaxNodeVisitor handling them directly
            .Register<CalloutMdSyntaxNode, CalloutMarkdownSyntaxNodeVisitor>()
            // .Register<CalloutTitleMdSyntaxNode, CalloutTitleMarkdownSyntaxNodeVisitor>() // Not implemented due to the CalloutMarkdownSyntaxNodeVisitor handling them directly
            .Register<CodeBlockMdSyntaxNode, CodeBlockMarkdownSyntaxNodeVisitor>()
            .Register<CodeInlineMdSyntaxNode, CodeInlineMarkdownSyntaxNodeVisitor>()
            .Register<EmoteMdSyntaxNode, EmoteMarkdownSyntaxNodeVisitor>()
            .Register<EscapedCharacterMdSyntaxNode, EscapedCharacterMarkdownSyntaxNodeVisitor>()
            .Register<FootnoteDescriptionMdSyntaxNode, FootnoteDescriptionMarkdownSyntaxNodeVisitor>()
            .Register<FootnoteReferenceMdSyntaxNode, FootnoteReferenceMarkdownSyntaxNodeVisitor>()
            .Register<FrontMatterMdSyntaxNode, FrontMatterMarkdownSyntaxNodeVisitor>()
            .Register<HeadingMdSyntaxNode, HeadingMarkdownSyntaxNodeVisitor>()
            .Register<HeadingSimpleMdSyntaxNode, HeadingSimpleMarkdownSyntaxNodeVisitor>()
            .Register<HighlightMdSyntaxNode, HighlightMarkdownSyntaxNodeVisitor>()
            .Register<HorizontalRuleMdSyntaxNode, HorizontalRuleMarkdownSyntaxNodeVisitor>()
            .Register<HtmlMdSyntaxNode, HtmlMarkdownSyntaxNodeVisitor>()
            .Register<HtmlSpanMdSyntaxNode, HtmlSpanMarkdownSyntaxNodeVisitor>()
            .Register<ImageMdSyntaxNode, ImageMarkdownSyntaxNodeVisitor>()
            .Register<ItalicMdSyntaxNode, ItalicMarkdownSyntaxNodeVisitor>()
            .Register<LinkMdSyntaxNode, LinkMarkdownSyntaxNodeVisitor>()
            // .Register<ListItemMdSyntaxNode, ListItemMarkdownSyntaxNodeVisitor>() // Not implemented due to the ListMarkdownSyntaxNodeVisitor handling them directly
            .Register<ListOrderedMdSyntaxNode, ListOrderedMarkdownSyntaxNodeVisitor>()
            .Register<ListUnorderedMdSyntaxNode, ListUnorderedMarkdownSyntaxNodeVisitor>()
            .Register<NewLineMdSyntaxNode, NewLineMarkdownSyntaxNodeVisitor>()
            .Register<ParagraphMdSyntaxNode, ParagraphMarkdownSyntaxNodeVisitor>()
            // .Register<RootMdSyntaxNode, RootMarkdownSyntaxNodeVisitor>() // Is a semantic node and cannot be processed
            .Register<ScriptingBodyMdSyntaxNode, ScriptingBodyMarkdownSyntaxNodeVisitor>()
            .Register<ScriptingExpressionMdSyntaxNode, ScriptingExpressionMarkdownSyntaxNodeVisitor>()
            .Register<ScriptingIfStatementMdSyntaxNode, ScriptingIfStatementMarkdownSyntaxNodeVisitor>()
            .Register<StrikeMdSyntaxNode, StrikeMarkdownSyntaxNodeVisitor>()
            .Register<SubScriptMdSyntaxNode, SubScriptMarkdownSyntaxNodeVisitor>()
            .Register<SuperScriptMdSyntaxNode, SuperScriptMarkdownSyntaxNodeVisitor>()
            // .Register<TableCellMdSyntaxNode, TableCellMarkdownSyntaxNodeVisitor>() // Not implemented due to the TableMarkdownSyntaxNodeVisitor handling them directly
            .Register<TableMdSyntaxNode, TableMarkdownSyntaxNodeVisitor>()
            // .Register<TableRowMdSyntaxNode, TableRowMarkdownSyntaxNodeVisitor>() // Not implemented due to the TableMarkdownSyntaxNodeVisitor handling them directly
            .Register<TagMdSyntaxNode, TagMarkdownSyntaxNodeVisitor>()
            .Register<TemplateMdSyntaxNode, TemplateMarkdownSyntaxNodeVisitor>()
            .Register<TextMdSyntaxNode, TextMarkdownSyntaxNodeVisitor>()
            .Register<UnderlineMdSyntaxNode, UnderlineMarkdownSyntaxNodeVisitor>()
            .Register<UserMdSyntaxNode, UserMarkdownSyntaxNodeVisitor>()
            .Register<WikiLinkMdSyntaxNode, WikiLinkMarkdownSyntaxNodeVisitor>()
            .Register<WrapperMdSyntaxNode, WrapperMarkdownSyntaxNodeVisitor>()
            ;

        instance.Deserializers = deserializers.ToFrozenDictionary();
        return instance;
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Helper Methods
    // -----------------------------------------------------------------------------------------------------------------
    private static Dictionary<Type, IMarkdownSyntaxNodeVisitor> Register<TNode, TDeserializer>(
        this Dictionary<Type, IMarkdownSyntaxNodeVisitor> deserializers
    ) where TDeserializer : BaseMarkdownSyntaxNodeVisitor<TNode>, new() where TNode : MdSyntaxNode<TNode>, new() {
        deserializers.AddOrUpdate(typeof(TNode), new TDeserializer());
        return deserializers;
    }
}
