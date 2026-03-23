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
/// <summary>
/// Represents a record that defines the mapping of a syntax-node type to a Blazor component,
/// along with the logic for rendering associated parameters in a render tree.
/// </summary>
/// <param name="SyntaxNodeType">The type of the syntax node this record is responsible for mapping.</param>
/// <param name="ComponentType">The Blazor component type that renders the specified syntax node.</param>
/// <param name="Builder">A function that constructs the render tree for the Blazor component, accepting a syntax node and other parameters.</param>
public sealed record BlazorComponentBuilderRecord(
    Type SyntaxNodeType,
    Type ComponentType,
    Func<RenderTreeBuilder, int, IMdSyntaxNode, int> Builder
) : IBlazorComponentBuilderRecord {

    /// <summary>
    /// Represents an empty default instance of <see cref="BlazorComponentBuilderRecord"/>.
    /// This property provides a predefined, immutable record configured for use with generic
    /// or unrecognized Markdown syntax nodes. It uses an
    /// <see cref="UnknownBlazorMdComponent"/> component type for rendering and a no-op
    /// builder function to process render parameters.
    /// </summary>
    public static BlazorComponentBuilderRecord Empty { get; } = new(
        typeof(IMdSyntaxNode),
        typeof(UnknownBlazorMdComponent),
        static (builder, sequence, node) => {
            builder.AddAttribute(sequence++, "SyntaxNode", node);
            return sequence;
        });

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// /// Creates a new instance of the BlazorComponentBuilderRecord for a specific syntax node type and its associated
    /// component type.
    /// This method defines a mapping between a syntax node type and a component that can render it in the Blazor
    /// framework. Additionally, it provides a static method to configure the relationship such that the RenderTreeBuilder
    /// can handle the node rendering properly.
    /// </summary>
    /// <typeparam name="TSyntaxNode">
    /// The type of the syntax node that inherits from IMdSyntaxNode.
    /// </typeparam>
    /// <typeparam name="TComponent">
    /// The type of the component responsible for rendering the specified syntax node type. It must implement
    /// IBlazorSyntaxNodeVisitor of TSyntaxNode.
    /// </typeparam>
    /// <return>
    /// A new instance of the BlazorComponentBuilderRecord with the specified syntax node type, component type, and the
    /// rendering logic.
    /// </return>
    public static BlazorComponentBuilderRecord FromType<TSyntaxNode, TComponent>()
        where TComponent : IBlazorSyntaxNodeVisitor<TSyntaxNode>
        where TSyntaxNode : class, IMdSyntaxNode {
        return new BlazorComponentBuilderRecord(
            typeof(TSyntaxNode),
            typeof(TComponent),
            static (builder, sequence, node) => {
                builder.AddAttribute(sequence++, "SyntaxNode", Unsafe.As<TSyntaxNode>(node));
                return sequence;
            }
        );
    }
}
