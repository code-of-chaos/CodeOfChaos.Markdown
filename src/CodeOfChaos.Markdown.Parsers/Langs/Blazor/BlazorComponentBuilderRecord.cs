// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.CompilerServices;

namespace CodeOfChaos.Markdown.Parsers.Langs.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed record BlazorComponentBuilderRecord(Type ComponentType, Func<RenderTreeBuilder, int, IMdSyntaxNode, int> Builder) : IBlazorComponentBuilderRecord {
    public static BlazorComponentBuilderRecord Empty { get; } = new(
        typeof(UnknownBlazorMdComponent),
        static (builder, sequence, node) => {
            builder.AddAttribute(sequence++, "SyntaxNode", node);
            return sequence;
        });

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------
    public static BlazorComponentBuilderRecord FromType<TSyntaxNode, TComponent>()
        where TComponent : IBlazorSyntaxNodeVisitor<TSyntaxNode>
        where TSyntaxNode : class, IMdSyntaxNode {
        return new BlazorComponentBuilderRecord(
            typeof(TComponent),
            static (builder, sequence, node) => {
                builder.AddAttribute(sequence++, "SyntaxNode", Unsafe.As<TSyntaxNode>(node));
                return sequence;
            }
        );
    }
}
