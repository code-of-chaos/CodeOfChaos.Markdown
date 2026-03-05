// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Langs.Blazor;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownConfigEntry<TSyntaxNode> : IMarkdownConfigEntry where TSyntaxNode : class, IMdSyntaxNode {
    public Type SyntaxNodeType { get; } = typeof(TSyntaxNode);
    
    public IBlazorComponentBuilderRecord? BlazorComponentBuilder { get; set; }
    
    public IJsonSyntaxNodeVisitor? JsonNodeVisitor { get; set; }
    
    public IMarkdownSyntaxNodeVisitor? MarkdownSingleLineNodeVisitor { get; set; }
    public IMarkdownSyntaxNodeVisitor? MarkdownMultiLineNodeVisitor { get; set; }
    public IMarkdownSyntaxNodeVisitor? MarkdownFrontMatterNodeVisitor { get; set; }
    
    public IXmlSyntaxNodeVisitor? XmlNodeVisitor { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public MarkdownConfigEntry<TSyntaxNode> WithBlazorNodeVisitor<TVisitor>() where TVisitor : class, IBlazorSyntaxNodeVisitor<TSyntaxNode> {
        BlazorComponentBuilder = BlazorComponentBuilderRecord.FromType<TSyntaxNode, TVisitor>();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithJsonNodeVisitor<TVisitor>() where TVisitor : class, IJsonSyntaxNodeVisitor<TSyntaxNode>, new() {
        JsonNodeVisitor = new TVisitor();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownSingleLineNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownSingleLineNodeVisitor = new TVisitor();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownMultiLineNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownMultiLineNodeVisitor = new TVisitor();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownFrontMatterNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode>, new() {
        MarkdownFrontMatterNodeVisitor = new TVisitor();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithXmlNodeVisitor<TVisitor>() where TVisitor : class, IXmlSyntaxNodeVisitor<TSyntaxNode>, new() {
        XmlNodeVisitor = new TVisitor();
        return this;
    }
}


