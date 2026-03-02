// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownConfigEntry<TSyntaxNode> : IMarkdownConfigEntry where TSyntaxNode : class, IMdSyntaxNode {
    public Type SyntaxNodeType { get; } = typeof(TSyntaxNode);
    
    public Type? BlazorNodeVisitorType { get; set; }
    public IBlazorComponentBuilderRecord? BlazorComponentBuilderRecord { get; set; }
    
    public Type? JsonNodeVisitorType { get; set; }
    
    public Type? MarkdownSingleLineNodeVisitorType { get; set; }
    public Type? MarkdownMultiLineNodeVisitorType { get; set; }
    public Type? MarkdownFrontMatterNodeVisitorType { get; set; }
    
    public Type? XmlNodeVisitorType { get; set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public MarkdownConfigEntry<TSyntaxNode> WithBlazorNodeVisitor<TVisitor>() where TVisitor : class, IBlazorSyntaxNodeVisitor<TSyntaxNode> {
        BlazorNodeVisitorType = typeof(TVisitor);
        BlazorComponentBuilderRecord = Parsers.Langs.Blazor.BlazorComponentBuilderRecord.FromType<TSyntaxNode, TVisitor>();
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithJsonNodeVisitor<TVisitor>() where TVisitor : class, IJsonSyntaxNodeVisitor<TSyntaxNode> {
        JsonNodeVisitorType = typeof(TVisitor);
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownSingleLineNodeVisitor<TVisitor>(bool isMultiline = false) where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode> {
        MarkdownSingleLineNodeVisitorType = typeof(TVisitor);
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownMultiLineNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode> {
        MarkdownMultiLineNodeVisitorType = typeof(TVisitor);
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithMarkdownFrontMatterNodeVisitor<TVisitor>() where TVisitor : class, IMarkdownSyntaxNodeVisitor<TSyntaxNode> {
        MarkdownFrontMatterNodeVisitorType = typeof(TVisitor);
        return this;
    }
    
    public MarkdownConfigEntry<TSyntaxNode> WithXmlNodeVisitor<TVisitor>() where TVisitor : class, IXmlSyntaxNodeVisitor<TSyntaxNode> {
        XmlNodeVisitorType = typeof(TVisitor);
        return this;
    }
}


