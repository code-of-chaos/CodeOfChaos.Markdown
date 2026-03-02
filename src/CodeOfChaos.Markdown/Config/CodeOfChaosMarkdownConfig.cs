// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Langs.Blazor;
using CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Parsers.NodeVisitors;
using CodeOfChaos.Markdown.Syntax.Nodes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Frozen;

namespace CodeOfChaos.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class CodeOfChaosMarkdownConfig : IMarkdownConfig {
    private Dictionary<Type, MdComponentRecord> ComponentRecords { get; } = new(32);
    private HashSet<Type> SkippedBlazorComponentTypes { get; } = [typeof(FootnoteDescriptionMdSyntaxNode)]; 
    public bool RenderUnknownBlazorComponents { get; set; }
    public Type? HtmlRendererFootnoteWrapperType { get; set; }

    private Lazy<FrozenDictionary<Type, IMdComponentRecord>> ComponentRecordsLazy { get; }
    private Lazy<FrozenSet<Type>> SkippedBlazorComponentTypesLazy { get; }
    
    internal List<ICodeOfChaosMarkdownConfigEntry> ConfigEntries { get; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public CodeOfChaosMarkdownConfig(IServiceCollection serviceCollection) {
        serviceCollection.AddSingleton<IMarkdownConfig>(this);
        
        serviceCollection.AddSingleton<IMdStringMdSyntaxSerializer>(static sp => {
            var fullOptions = new MarkdownSerializerOptions {
                SingleLine = [
                    new EscapedCharacterMarkdownSyntaxNodeVisitor(),
                    new BoldMarkdownSyntaxNodeVisitor(),
                    new ItalicMarkdownSyntaxNodeVisitor(),
                    new SuperScriptMarkdownSyntaxNodeVisitor(),
                    new SubScriptMarkdownSyntaxNodeVisitor(),
                    new CodeInlineMarkdownSyntaxNodeVisitor(),
                    new StrikeMarkdownSyntaxNodeVisitor(),
                    new UnderlineMarkdownSyntaxNodeVisitor(),
                    new HighlightMarkdownSyntaxNodeVisitor(),
                    new EmoteMarkdownSyntaxNodeVisitor(),
                    new WikiLinkMarkdownSyntaxNodeVisitor(),
                    new TemplateMarkdownSyntaxNodeVisitor(),
                    new ImageMarkdownSyntaxNodeVisitor(),
                    new LinkMarkdownSyntaxNodeVisitor(),
                    new TagMarkdownSyntaxNodeVisitor(),
                    new UserMarkdownSyntaxNodeVisitor(),
                    new FootnoteReferenceMarkdownSyntaxNodeVisitor(),
                    new WrapperMarkdownSyntaxNodeVisitor(),
                    new BreakMarkdownSyntaxNodeVisitor(),
                ],
                MultiLine = [
                    new ScriptingIfStatementMarkdownSyntaxNodeVisitor(),
                    new HeadingMarkdownSyntaxNodeVisitor(),
                    new CodeBlockMarkdownSyntaxNodeVisitor(),
                    new HeadingSimpleMarkdownSyntaxNodeVisitor(),
                    new ListOrderedMarkdownSyntaxNodeVisitor(),
                    new ListUnorderedMarkdownSyntaxNodeVisitor(),
                    new TableMarkdownSyntaxNodeVisitor(),
                    new CalloutMarkdownSyntaxNodeVisitor(),
                    new BlockQuoteMarkdownSyntaxNodeVisitor(),
                    new FootnoteDescriptionMarkdownSyntaxNodeVisitor(),
                    new HtmlSpanMarkdownSyntaxNodeVisitor(),
                    new HtmlMarkdownSyntaxNodeVisitor(),
                    new HorizontalRuleMarkdownSyntaxNodeVisitor(),
                    new ParagraphMarkdownSyntaxNodeVisitor(),
                    new NewLineMarkdownSyntaxNodeVisitor()
                ],
                FrontMatter = new FrontMatterMarkdownSyntaxNodeVisitor()
            };

            var factory = sp.GetRequiredService<IMdStringMdSyntaxSerializerFactory>();
            return factory.Create(fullOptions);
        });
        
        ComponentRecordsLazy = new Lazy<FrozenDictionary<Type, IMdComponentRecord>>(() => {
            ComponentRecords.TrimExcess();
            return ComponentRecords.ToFrozenDictionary(
                pair => pair.Key, 
                IMdComponentRecord (pair) => pair.Value);
        });
        
        SkippedBlazorComponentTypesLazy = new Lazy<FrozenSet<Type>>(() => {
            SkippedBlazorComponentTypes.TrimExcess();
            return SkippedBlazorComponentTypes.ToFrozenSet();
        });
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public CodeOfChaosMarkdownConfig SkipBlazorRenderingOnComponent<TNode>() where TNode : class, IMdSyntaxNode {
        SkippedBlazorComponentTypes.Add(typeof(TNode));
        return this;   
    }

    public FrozenDictionary<Type, IMdComponentRecord> GetComponentRecords()
        => ComponentRecordsLazy.Value;
    
    public FrozenSet<Type> GetSkippedBlazorComponentTypes()
        => SkippedBlazorComponentTypesLazy.Value;
    
}
