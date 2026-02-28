// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Markdown.Parsers.Markdown.Serializer;
using CodeOfChaos.Markdown.Markdown.Syntax;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer.NodeSerializers;
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
    
    
    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public CodeOfChaosMarkdownConfig(IServiceCollection serviceCollection) {
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdown();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownEditors();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownParsers();
        
        serviceCollection.AddSingleton(TextEditorFactory.CreateTextEditor);
        serviceCollection.AddSingleton(MdStringMdSyntaxDeserializerFactory.CreateDeserializer);
        serviceCollection.AddSingleton<IMarkdownConfig>(this);
        
        serviceCollection.AddSingleton<IMdStringMdSyntaxSerializer>(static sp => {
            var fullOptions = new MarkdownSerializerOptions {
                SingleLine = [
                    new EscapedCharacterSyntaxNodeSerializer(),
                    new BoldSyntaxNodeSerializer(),
                    new ItalicSyntaxNodeSerializer(),
                    new SuperScriptSyntaxNodeSerializer(),
                    new SubScriptSyntaxNodeSerializer(),
                    new CodeInlineSyntaxNodeSerializer(),
                    new StrikeSyntaxNodeSerializer(),
                    new UnderlineSyntaxNodeSerializer(),
                    new HighlightSyntaxNodeSerializer(),
                    new EmoteSyntaxNodeSerializer(),
                    new WikiLinkSyntaxNodeSerializer(),
                    new TemplateSyntaxNodeSerializer(),
                    new LinkSyntaxNodeSerializer(),
                    new TagSyntaxNodeSerializer(),
                    new UserSyntaxNodeSerializer(),
                    new FootnoteReferenceSyntaxNodeSerializer(),
                    new WrapperSyntaxNodeSerializer(),
                    new BreakSyntaxNodeSerializer(),
                ],
                MultiLine = [
                    new ScriptingIfStatementSyntaxNodeSerializer(),
                    new HeadingSyntaxNodeSerializer(),
                    new CodeBlockSyntaxNodeSerializer(),
                    new HeadingSimpleSyntaxNodeSerializer(),
                    new ListSyntaxNodeSerializer(),
                    new TableSyntaxNodeSerializer(),
                    new CalloutSyntaxNodeSerializer(),
                    new BlockQuoteSyntaxNodeSerializer(),
                    new FootnoteDescriptionSyntaxNodeSerializer(),
                    new HtmlBlockSyntaxNodeSerializer(),
                    new HorizontalRuleSyntaxNodeSerializer(),
                    new ParagraphSyntaxNodeSerializer(),
                    new NewLineSyntaxNodeSerializer()
                ],
                FrontMatter = new FrontmatterSyntaxNodeSerializer()
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
    public CodeOfChaosMarkdownConfig RegisterMdBlazorComponent<TNode, TComponent>() where TComponent : MarkdownComponentBase<TNode> where TNode : class, IMdSyntaxNode {
        int count = ComponentRecords.Count;
        if (ComponentRecords.Capacity < count + 1) ComponentRecords.EnsureCapacity(count * 2);
        
        ComponentRecords.AddOrUpdate(typeof(TNode), MdComponentRecord.FromType<TComponent, TNode>());   
        return this;
    }
    
    public CodeOfChaosMarkdownConfig SkipBlazorRenderingOnComponent<TNode>() where TNode : class, IMdSyntaxNode {
        SkippedBlazorComponentTypes.Add(typeof(TNode));
        return this;   
    }

    public FrozenDictionary<Type, IMdComponentRecord> GetComponentRecords()
        => ComponentRecordsLazy.Value;
    
    public FrozenSet<Type> GetSkippedBlazorComponentTypes()
        => SkippedBlazorComponentTypesLazy.Value;
    
}
