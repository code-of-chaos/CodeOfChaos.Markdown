// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Langs.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Represents the context for rendering Markdown content, typically within the Blazor framework.
/// This class provides functionality to manage render settings and trigger updates when syntax nodes change.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class MdRenderContext {

    /// <summary>
    /// Gets or sets a value indicating whether the rendering context supports interactive features.
    /// </summary>
    /// <remarks>
    /// When set to <c>true</c>, the rendering context enables interactive behaviors, such as responding
    /// to syntax node changes or user inputs. If <c>false</c>, the rendering context operates in a
    /// non-interactive mode and does not support these features.
    /// </remarks>
    public bool IsInteractive { get; set; }

    /// <summary>
    /// Occurs when a syntax node within the Markdown rendering context is changed.
    /// </summary>
    /// <remarks>
    /// This event is triggered whenever a syntax node of type <see cref="IMdSyntaxNode"/> undergoes a modification
    /// during parsing or rendering. It allows subscribers to react asynchronously to such changes.
    /// </remarks>
    /// <example>
    /// This event can be used for handling updates to the syntax tree, performing custom transformations,
    /// or integrating additional logic during the parsing process.
    /// </example>
    /// <seealso cref="IMdSyntaxNode" />
    public event Func<IMdSyntaxNode, Task>? OnSyntaxNodeChanged;


    /// <summary>
    /// Represents a static property that provides an empty instance of the <see cref="MdRenderContext"/> class.
    /// This instance has default property values and is non-interactive.
    /// </summary>
    public static MdRenderContext Empty { get; set; } = new();

    /// <summary>
    /// Provides a predefined instance of <see cref="MdRenderContext"/> configured for interactive mode.
    /// </summary>
    /// <remarks>
    /// Interactive contexts have the <see cref="MdRenderContext.IsInteractive"/> property set to true,
    /// enabling user interaction behaviors during Markdown rendering.
    /// </remarks>
    public static MdRenderContext Interactive => new() { IsInteractive = true };

    /// <summary>
    /// Provides a static instance of <see cref="MdRenderContext"/> configured as non-interactive.
    /// This instance has the <see cref="MdRenderContext.IsInteractive"/> property set to <c>false</c>,
    /// indicating that the associated context does not support user interactions.
    /// </summary>
    public static MdRenderContext NonInteractive => new() { IsInteractive = false };

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Invokes the subscribed event handlers for syntax node changes.
    /// This method triggers the <see cref="OnSyntaxNodeChanged"/> event, passing the specified syntax node,
    /// and awaits its eventual asynchronous execution.
    /// </summary>
    /// <param name="node">The instance of <see cref="IMdSyntaxNode"/> representing the syntax node that has changed.</param>
    /// <returns>An asynchronous task representing completion of any triggered event handlers.</returns>
    public async Task InvokeSyntaxNodeChange(IMdSyntaxNode node) {
        if (OnSyntaxNodeChanged is null) return;

        await OnSyntaxNodeChanged(node).ConfigureAwait(false);
    }
}
