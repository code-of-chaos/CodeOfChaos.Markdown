// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.Debouncers;
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.TextEditor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Parsers.Langs.Blazor;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------

/// <summary>
/// Provides context for an interactive Markdown editor, managing state and handling user interactions.
/// </summary>
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class MdEditorContext {
    /// <summary>
    /// Indicates whether the markdown editor context is in a locked state.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, the editor is considered locked, potentially preventing
    /// user interactions or modifications within the editor. This property can
    /// be used to enforce read-only behavior or restrict actions during specific
    /// operations.
    /// </remarks>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the preview of the Markdown content
    /// should be displayed in the editor context.
    /// </summary>
    public bool ShowPreview { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the input editor is displayed.
    /// </summary>
    /// <remarks>
    /// This property specifies whether the input section of the Markdown editor is visible.
    /// It can be used to toggle visibility based on user interaction or context-specific requirements.
    /// The default value is <c>true</c>, enabling the input editor by default.
    /// </remarks>
    public bool ShowInput { get; set; } = true;


    /// Gets or sets the content string of the editor.
    /// The `Content` property provides access to the textual data managed by the editor.
    /// Getting the value retrieves the current content of the internal text source,
    /// while setting the value updates the editor's internal state with the new content.
    /// This property is typically used to read or modify the markdown content within the editor context.
    public string Content {
        get => TextSource.Text;
        set => TextSource.UpdateSource(value);
    }


    /// <summary>
    /// Gets the instance of <see cref="ITextSource"/> used to represent and manipulate the text source
    /// within the Markdown editor context. This property provides access to the underlying text content
    /// and its associated metadata, such as length, line ranges, and line count. It also allows updates
    /// to the text source via its exposed methods.
    /// </summary>
    public ITextSource TextSource { get; } = new TextSource();

    /// <summary>
    /// Gets or sets a reference to the input HTML element associated with the Markdown editor.
    /// </summary>
    /// <remarks>
    /// This property allows integration with Blazor's component model by providing a reference
    /// to the underlying DOM element. It enables features such as direct DOM manipulation or
    /// access to element-specific attributes and properties.
    /// </remarks>
    public ElementReference InputElementRef { get; set; }


    /// <summary>
    /// Represents the syntax tree of a Markdown document, allowing structural representation
    /// and manipulation of its content within the editor context.
    /// </summary>
    /// <remarks>
    /// This property is used to access or modify the current syntax tree of the Markdown editor.
    /// It is of type <see cref="IMdSyntaxTree"/>, which serves as an abstraction for working
    /// with the underlying Markdown structure.
    /// The syntax tree is central to parsing and rendering Markdown content, enabling features
    /// such as visualization, node traversal, and modification of Markdown elements.
    /// </remarks>
    public IMdSyntaxTree SyntaxTree { get; set; } = MdSyntaxTree.Empty;


    /// <summary>
    /// An asynchronous event that is triggered when the underlying source content of the editor changes.
    /// This event allows subscribers to execute custom logic in response to modifications to the editor's content.
    /// </summary>
    public event Func<Task>? OnSourceChangedAsync;

    /// <summary>
    /// Event triggered asynchronously when the syntax tree changes in the Markdown editor context.
    /// </summary>
    /// <remarks>
    /// This event is invoked to notify subscribers of changes in the syntax tree within the
    /// Markdown editor. The event can be null if no subscribers are registered.
    /// </remarks>
    public event Func<Task>? OnSyntaxTreeChangedAsync;

    /// <summary>
    /// Event triggered when a key is pressed down within the input element of the Markdown editor.
    /// Allows for custom handling of keyboard input events, such as shortcuts or other key-based interactions.
    /// </summary>
    /// <remarks>
    /// This event is invoked with a <see cref="KeyboardEventArgs"/> parameter that contains information
    /// about the keyboard event, such as key, code, and modifiers. Custom event handlers can be attached
    /// to perform specific actions during key press events.
    /// </remarks>
    /// <seealso cref="MdEditorContext.InvokeInputKeyDownAsync"/>
    /// <seealso cref="Microsoft.AspNetCore.Components.Web.KeyboardEventArgs"/>
    public event Func<KeyboardEventArgs, Task>? OnInputKeyDownAsync;

    /// <summary>
    /// Event delegate that is triggered when a modifier action is invoked in the Markdown editor context.
    /// Provides the name of the modifier action as an input parameter.
    /// </summary>
    /// <remarks>
    /// This event can be used to handle specific actions related to text modifications, triggered by user input
    /// or predefined conditions. Implement this event to define custom behavior or processing logic based
    /// on the provided modifier name.
    /// </remarks>
    /// <param>The name of the modifier action being invoked.</param>
    /// <returns>Returns a <see cref="Task"/> representing the asynchronous operation.</returns>
    public event Func<string, Task>? OnModifierActionAsync;

    /// <summary>
    /// An event that is triggered when a specific action related to inserting content
    /// is performed. The delegate receives a <see cref="string"/> representing the
    /// inserted content as its parameter and returns a <see cref="Task"/> that completes
    /// when the action is fully processed.
    /// </summary>
    public event Func<string, Task>? OnInsertActionAsync;

    /// <summary>
    /// A throttled debouncer responsible for managing the execution of syntax tree change callbacks
    /// in a controlled and delayed manner to prevent excessive invocation.
    /// </summary>
    /// <remarks>
    /// This property utilizes a <see cref="ThrottledDebouncer"/> to delay and aggregate
    /// calls to syntax tree change events, thus ensuring efficient handling of changes
    /// in the editor's syntax tree state.
    /// </remarks>
    private ThrottledDebouncer SyntaxTreeChangedCallbackDebouncer { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Constructors
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Represents the context for a Markdown editor, managing state and behavior related to editing Markdown content.
    /// </summary>
    public MdEditorContext() {
        SyntaxTreeChangedCallbackDebouncer = ThrottledDebouncer.FromDelegate(async () => {
            if (OnSyntaxTreeChangedAsync is null) return;

            await OnSyntaxTreeChangedAsync().ConfigureAwait(false);
        });
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Clears the content in the markdown editor asynchronously by resetting the source value to an empty string.
    /// </summary>
    /// <returns>A task that represents the asynchronous clear operation.</returns>
    public Task ClearAsync()
        => InvokeSourceChangeAsync(string.Empty);


    /// <summary>
    /// Updates the markdown source content and triggers the source changed event asynchronously.
    /// </summary>
    /// <param name="value">The new content to update the markdown editor with.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeSourceChangeAsync(string value) {
        TextSource.UpdateSource(value);
        if (OnSourceChangedAsync is null) return;

        await OnSourceChangedAsync().ConfigureAwait(false);
    }


    /// Triggers the syntax tree change event by invoking the associated debounced callback.
    /// This method ensures that any subscribed logic for syntax tree updates is executed in a controlled manner.
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeSyntaxTreeChangeAsync()
        => await SyntaxTreeChangedCallbackDebouncer.InvokeDebouncedAsync();


    /// <summary>
    /// Invokes the input key down event by triggering the associated event handler if it is defined.
    /// </summary>
    /// <param name="e">The <see cref="KeyboardEventArgs"/> containing details about the key down event.</param>
    /// <returns>A task that represents the asynchronous operation of handling the key down event.</returns>
    public async Task InvokeInputKeyDownAsync(KeyboardEventArgs e) {
        if (OnInputKeyDownAsync is null) return;

        await OnInputKeyDownAsync(e).ConfigureAwait(false);
    }


    /// <summary>
    /// Invokes the modifier action asynchronously with the specified modifier name.
    /// </summary>
    /// <param name="modifierName">The name of the modifier to be executed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeModifierAsync(string modifierName) {
        if (OnModifierActionAsync is null) return;

        await OnModifierActionAsync(modifierName).ConfigureAwait(false);
    }


    /// <summary>
    /// Asynchronously invokes an insert action for the specified content if an insert action delegate is defined.
    /// </summary>
    /// <param name="content">The content to be inserted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeInsertAsync(string content) {
        if (OnInsertActionAsync is null) return;

        await OnInsertActionAsync(content).ConfigureAwait(false);
    }
}
