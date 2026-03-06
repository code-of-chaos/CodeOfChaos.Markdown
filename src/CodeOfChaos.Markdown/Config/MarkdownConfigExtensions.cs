// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Syntax.Nodes;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class MarkdownConfigExtensions {
    extension(MarkdownConfig config) {
        public MarkdownConfig AddDefaultNodeVisitors() {
            // SingleLine Structures
            config.WithSyntaxNode<EscapedCharacterMdSyntaxNode>()
                .WithBlazorNodeVisitor<EscapedCharacterBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<EscapedCharacterJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<EscapedCharacterMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<EscapedCharacterXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<BoldMdSyntaxNode>()
                .WithBlazorNodeVisitor<BoldBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<BoldMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<BoldMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<BoldMdSyntaxNode>>();
            
            config.WithSyntaxNode<ItalicMdSyntaxNode>()
                .WithBlazorNodeVisitor<ItalicBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<ItalicMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<ItalicMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<ItalicMdSyntaxNode>>();
            
            config.WithSyntaxNode<SuperScriptMdSyntaxNode>()
                .WithBlazorNodeVisitor<SuperScriptBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<SuperScriptMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<SuperScriptMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<SuperScriptMdSyntaxNode>>();
            
            config.WithSyntaxNode<SubScriptMdSyntaxNode>()
                .WithBlazorNodeVisitor<SubScriptBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<SubScriptMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<SubScriptMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<SubScriptMdSyntaxNode>>();
            
            config.WithSyntaxNode<CodeInlineMdSyntaxNode>()
                .WithBlazorNodeVisitor<CodeInlineBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<CodeInlineJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<CodeInlineMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<CodeInlineXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<StrikeMdSyntaxNode>()
                .WithBlazorNodeVisitor<StrikeBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<StrikeMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<StrikeMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<StrikeMdSyntaxNode>>();
            
            config.WithSyntaxNode<UnderlineMdSyntaxNode>()
                .WithBlazorNodeVisitor<UnderlineBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<UnderlineMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<UnderlineMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<UnderlineMdSyntaxNode>>();
            
            config.WithSyntaxNode<HighlightMdSyntaxNode>()
                .WithBlazorNodeVisitor<HighlightBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<HighlightMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<HighlightMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<HighlightMdSyntaxNode>>();
            
            config.WithSyntaxNode<EmoteMdSyntaxNode>()
                .WithBlazorNodeVisitor<EmoteBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<EmoteJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<EmoteMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<EmoteXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<WikiLinkMdSyntaxNode>()
                .WithBlazorNodeVisitor<WikiLinkBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<WikiLinkJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<WikiLinkMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<WikiLinkXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<TemplatingLiteralStatementMdSyntaxNode>()
                .WithBlazorNodeVisitor<TemplatingLiteralStatementBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<TemplatingLiteralStatementMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<TemplatingLiteralStatementMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<TemplatingLiteralStatementMdSyntaxNode>>();
            
            config.WithSyntaxNode<ImageMdSyntaxNode>()
                .WithBlazorNodeVisitor<ImageBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<ImageJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<ImageMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<ImageXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<LinkMdSyntaxNode>()
                .WithBlazorNodeVisitor<LinkBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<LinkJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<LinkMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<LinkXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<TagMdSyntaxNode>()
                .WithBlazorNodeVisitor<TagBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<TagJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<TagMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<TagXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<UserMdSyntaxNode>()
                .WithBlazorNodeVisitor<UserBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<UserJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<UserMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<UserXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<FootnoteReferenceMdSyntaxNode>()
                .WithBlazorNodeVisitor<FootnoteReferenceBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<FootnoteReferenceJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<FootnoteReferenceMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<FootnoteReferenceXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<WrapperMdSyntaxNode>()
                .WithBlazorNodeVisitor<WrapperBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<WrapperMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<WrapperMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<WrapperMdSyntaxNode>>();
            
            config.WithSyntaxNode<BreakMdSyntaxNode>()
                .WithBlazorNodeVisitor<BreakBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<BreakMdSyntaxNode>>()
                .WithMarkdownSingleLineNodeVisitor<BreakMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<BreakMdSyntaxNode>>();
            
            // MultiLine Structures
            config.WithSyntaxNode<TemplatingIfStatementMdSyntaxNode>()
                .WithBlazorNodeVisitor<TemplatingIfStatementBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<TemplatingIfStatementMdSyntaxNode>>()
                .WithMarkdownMultiLineNodeVisitor<TemplatingIfStatementMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<TemplatingIfStatementMdSyntaxNode>>();
            
            config.WithSyntaxNode<HeadingMdSyntaxNode>()
                .WithBlazorNodeVisitor<HeadingBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<HeadingJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<HeadingMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<HeadingXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<CodeBlockMdSyntaxNode>()
                .WithBlazorNodeVisitor<CodeBlockBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<CodeBlockJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<CodeBlockMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<CodeBlockXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<HeadingSimpleMdSyntaxNode>()
                .WithBlazorNodeVisitor<HeadingSimpleBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<HeadingSimpleJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<HeadingSimpleMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<HeadingSimpleXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<ListOrderedMdSyntaxNode>()
                .WithBlazorNodeVisitor<ListOrderedBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<ListOrderedJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<ListOrderedMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<ListOrderedXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<ListUnorderedMdSyntaxNode>()
                .WithBlazorNodeVisitor<ListUnorderedBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<ListUnorderedJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<ListUnorderedMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<ListUnorderedXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<TableMdSyntaxNode>()
                .WithBlazorNodeVisitor<TableBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<TableJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<TableMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<TableXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<CalloutMdSyntaxNode>()
                .WithBlazorNodeVisitor<CalloutBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<CalloutJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<CalloutMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<CalloutXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<BlockQuoteMdSyntaxNode>()
                .WithBlazorNodeVisitor<BlockQuoteBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<BlockQuoteJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<BlockQuoteMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<BlockQuoteXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<FootnoteDescriptionMdSyntaxNode>()
                .WithBlazorNodeVisitor<FootnoteDescriptionBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<FootnoteDescriptionJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<FootnoteDescriptionMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<FootnoteDescriptionXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<HtmlSpanMdSyntaxNode>()
                .WithBlazorNodeVisitor<HtmlSpanBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<HtmlSpanJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<HtmlSpanMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<HtmlSpanXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<HtmlMdSyntaxNode>()
                .WithBlazorNodeVisitor<HtmlBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<HtmlJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<HtmlMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<HtmlXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<HorizontalRuleMdSyntaxNode>()
                .WithBlazorNodeVisitor<HorizontalRuleBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<HorizontalRuleJsonSyntaxNodeVisitor>()
                .WithMarkdownMultiLineNodeVisitor<HorizontalRuleMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<HorizontalRuleXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<ParagraphMdSyntaxNode>()
                .WithBlazorNodeVisitor<ParagraphBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<ParagraphMdSyntaxNode>>()
                .WithMarkdownMultiLineNodeVisitor<ParagraphMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<ParagraphMdSyntaxNode>>();
            
            config.WithSyntaxNode<NewLineMdSyntaxNode>()
                .WithBlazorNodeVisitor<NewLineBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<NewLineMdSyntaxNode>>()
                .WithMarkdownMultiLineNodeVisitor<NewLineMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<NewLineMdSyntaxNode>>();
            
            // Special Structures (Order does not matter)
            config.WithSyntaxNode<FrontMatterMdSyntaxNode>()
                .WithBlazorNodeVisitor<FrontMatterBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<FrontMatterJsonSyntaxNodeVisitor>()
                .WithMarkdownFrontMatterNodeVisitor<FrontMatterMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<FrontMatterXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<ListItemMdSyntaxNode>()
                .WithBlazorNodeVisitor<ListItemBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<ListItemJsonSyntaxNodeVisitor>()
                // .WithMarkdownMultiLineNodeVisitor<ListItemMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<ListItemXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<CalloutTitleMdSyntaxNode>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<CalloutTitleMdSyntaxNode>>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<CalloutTitleMdSyntaxNode>>();
            
            config.WithSyntaxNode<CalloutBodyMdSyntaxNode>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<CalloutBodyMdSyntaxNode>>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<CalloutBodyMdSyntaxNode>>();
            
            config.WithSyntaxNode<TableRowMdSyntaxNode>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<TableRowMdSyntaxNode>>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<TableRowMdSyntaxNode>>();
            
            config.WithSyntaxNode<TableCellMdSyntaxNode>()
                .WithJsonNodeVisitor<JsonSyntaxNodeVisitor<TableCellMdSyntaxNode>>()
                .WithXmlNodeVisitor<XmlSyntaxNodeVisitor<TableCellMdSyntaxNode>>();
            
            config.WithSyntaxNode<TextMdSyntaxNode>()
                .WithBlazorNodeVisitor<TextBlazorSyntaxNodeVisitor>()
                .WithJsonNodeVisitor<TextJsonSyntaxNodeVisitor>()
                .WithMarkdownSingleLineNodeVisitor<TextMarkdownSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<TextXmlSyntaxNodeVisitor>();
            
            config.WithSyntaxNode<TemplateExpressionMdSyntaxNode>()
                .WithJsonNodeVisitor<TemplateExpressionJsonSyntaxNodeVisitor>()
                .WithXmlNodeVisitor<TemplateExpressionXmlSyntaxNodeVisitor>();
            
            config.SkipBlazorRenderingOnComponent<FootnoteDescriptionMdSyntaxNode>();
            
            return config;
        }
    }
}
