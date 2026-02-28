// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer.NodeDeserializers;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;
using BoldMarkdownSyntaxNodeVisitor = CodeOfChaos.Markdown.Parsers.NodeVisitors.BoldMarkdownSyntaxNodeVisitor;

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
            // .Register<CalloutBodyMdSyntaxNode, CalloutBodySyntaxNodeDeserializer>(instance) // Not implemented due to the CalloutSyntaxNodeDeserializer handling them directly
            // .Register<CalloutTitleMdSyntaxNode, CalloutTitleSyntaxNodeDeserializer>(instance) // Not implemented due to the CalloutSyntaxNodeDeserializer handling them directly
            .Register<CalloutMdSyntaxNode, CalloutSyntaxNodeDeserializer>(instance)
            .Register<CodeBlockMdSyntaxNode, CodeBlockSyntaxNodeDeserializer>(instance)
            .Register<CodeInlineMdSyntaxNode, CodeInlineSyntaxNodeDeserializer>(instance)
            .Register<HtmlMdSyntaxNode, HtmlSyntaxNodeDeserializer>(instance)
            .Register<TextMdSyntaxNode, TextSyntaxNodeDeserializer>(instance)
            .Register<EmoteMdSyntaxNode, EmoteSyntaxNodeDeserializer>(instance)
            .Register<NewLineMdSyntaxNode, NewLineSyntaxNodeDeserializer>(instance)
            .Register<EscapedCharacterMdSyntaxNode, EscapedCharacterSyntaxNodeDeserializer>(instance)
            .Register<HeadingMdSyntaxNode, HeadingSyntaxNodeDeserializer>(instance)
            .Register<HeadingSimpleMdSyntaxNode, HeadingSimpleSyntaxNodeDeserializer>(instance)
            .Register<HorizontalRuleMdSyntaxNode, HorizontalRuleSyntaxNodeDeserializer>(instance)
            .Register<HtmlSpanMdSyntaxNode, HtmlSpanSyntaxNodeDeserializer>(instance)
            .Register<ImageMdSyntaxNode, ImageSyntaxNodeDeserializer>(instance)
            .Register<ItalicMdSyntaxNode, ItalicSyntaxNodeDeserializer>(instance)
            .Register<LinkMdSyntaxNode, LinkSyntaxNodeDeserializer>(instance)
            .Register<ListItemMdSyntaxNode, ListItemSyntaxNodeDeserializer>(instance)
            .Register<ListOrderedMdSyntaxNode, ListOrderedSyntaxNodeDeserializer>(instance)
            .Register<ListUnOrderedMdSyntaxNode, ListUnOrderedSyntaxNodeDeserializer>(instance)
            .Register<ParagraphMdSyntaxNode, ParagraphSyntaxNodeDeserializer>(instance)
            // Register<RootMdSyntaxNode, RootSyntaxNodeDeserializer>(instance) // Is a semantic node and cannot be processed
            .Register<StrikeMdSyntaxNode, StrikeSyntaxNodeDeserializer>(instance)
            .Register<SubScriptMdSyntaxNode, SubScriptSyntaxNodeDeserializer>(instance)
            .Register<SuperScriptMdSyntaxNode, SuperScriptSyntaxNodeDeserializer>(instance)
            // .Register<TableCellMdSyntaxNode, TableCellSyntaxNodeDeserializer>(instance) // Not implemented due to the TableSyntaxNodeDeserializer handling them directly
            // .Register<TableRowMdSyntaxNode, TableRowSyntaxNodeDeserializer>(instance) // Not implemented due to the TableSyntaxNodeDeserializer handling them directly
            .Register<TableMdSyntaxNode, TableSyntaxNodeDeserializer>(instance)
            .Register<TagMdSyntaxNode, TagSyntaxNodeDeserializer>(instance)
            .Register<UnderlineMdSyntaxNode, UnderlineSyntaxNodeDeserializer>(instance)
            .Register<UserMdSyntaxNode, UserSyntaxNodeDeserializer>(instance)
            .Register<WikiLinkMdSyntaxNode, WikiLinkSyntaxNodeDeserializer>(instance)
            .Register<TemplateMdSyntaxNode, TemplateSyntaxNodeDeserializer>(instance)
            .Register<FootnoteReferenceMdSyntaxNode, FootnoteReferenceSyntaxNodeDeserializer>(instance)
            .Register<FootnoteDescriptionMdSyntaxNode, FootnoteDescriptionSyntaxNodeDeserializer>(instance)
            .Register<HighlightMdSyntaxNode, HighlightSyntaxNodeDeserializer>(instance)
            .Register<WrapperMdSyntaxNode, WrapperSyntaxNodeDeserializer>(instance)
            .Register<FrontMatterMdSyntaxNode, FrontMatterSyntaxNodeDeserializer>(instance)
            .Register<BreakMdSyntaxNode, BreakSyntaxNodeDeserializer>(instance)
            .Register<ScriptingIfStatementSyntaxNode, ScriptingIfStatementSyntaxNodeDeserializer>(instance)
            .Register<ScriptingExpressionSyntaxNode, ScriptingExpressionSyntaxNodeDeserializer>(instance)
            .Register<ScriptingBodySyntaxNode, ScriptingBodySyntaxNodeDeserializer>(instance)
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
