// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Blazor;
using CodeOfChaos.Markdown.Syntax;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Frozen;

namespace CodeOfChaos.Markdown.Parsers.Langs.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IBlazorMdComponentRenderer>]
public class BlazorMdComponentRenderer(IMarkdownConfig config) : IBlazorMdComponentRenderer {
    private FrozenDictionary<Type, IBlazorComponentBuilderRecord> NodeToComponentMap { get; } = config.BlazorComponents;
    private FrozenSet<Type> SkippedComponentTypes { get; } = config.SkippedBlazorComponents;
    private bool RenderUnknownComponents { get; } = config.RenderUnknownBlazorComponents;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    private void RenderNodeAsComponent(RenderTreeBuilder builder, IMdSyntaxNode node, bool ignoreSkipComponents = false) {
        if (!ignoreSkipComponents && SkippedComponentTypes.Contains(node.Type)) return;

        if (!NodeToComponentMap.TryGetValue(node.Type, out IBlazorComponentBuilderRecord? data)) {
            if (!RenderUnknownComponents) return;

            data = BlazorComponentBuilderRecord.Empty;
        }

        int sequence = 0;
        builder.OpenComponent(sequence++, data.ComponentType);
        int _ = data.Builder(builder, sequence, node);// newSequence is not used so far
        builder.SetKey(node.Id);

        builder.CloseComponent();
    }

    public RenderFragment RenderComponent(IMdSyntaxNode node)
        => builder => RenderNodeAsComponent(builder, node);

    public RenderFragment RenderComponentDebug(IMdSyntaxNode node) => builder => {
        var data = BlazorComponentBuilderRecord.Empty;
        int sequence = 0;

        builder.OpenComponent(sequence++, data.ComponentType);
        data.Builder(builder, sequence, node);// newSequence is not used so far
        builder.CloseComponent();
    };

    public RenderFragment RenderChildComponents(IMdSyntaxNode node) => builder => {
        int childCount = node.ChildCount;
        if (childCount == 0) return;

        ReadOnlySpan<IMdSyntaxNode> childSpan = node.GetChildrenSpan();
        for (int i = 0; i < childCount; i++) {
            RenderNodeAsComponent(builder, childSpan[i]);
        }
    };

    public RenderFragment RenderRootComponents(IEnumerable<IMdSyntaxNode> nodes) => builder => {
        foreach (IMdSyntaxNode child in nodes) RenderNodeAsComponent(builder, child);
    };

    public RenderFragment RenderRootComponentsWithSkipped(IEnumerable<IMdSyntaxNode> nodes) => builder => {
        foreach (IMdSyntaxNode child in nodes) RenderNodeAsComponent(builder, child, true);
    };
}
