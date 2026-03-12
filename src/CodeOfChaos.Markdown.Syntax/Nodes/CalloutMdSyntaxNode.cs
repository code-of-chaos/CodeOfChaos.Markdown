// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Diagnostics.CodeAnalysis;

namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class CalloutMdSyntaxNode() : MdSyntaxNode<CalloutMdSyntaxNode>(initialChildCount: 2) {
    /// <summary>
    /// Represents the index used to reference the title node within the child nodes
    /// of a <see cref="CalloutMdSyntaxNode"/>. This index is used to identify, set,
    /// or retrieve the title content of a callout node.
    /// </summary>
    /// <remarks>
    /// The value of <c>TitleNodeIndex</c> is a zero-based index corresponding to the
    /// position of the title node in the array of child nodes of a <see cref="CalloutMdSyntaxNode"/>.
    /// It is a constant value and does not change during the lifecycle of the object.
    /// </remarks>
    private const int TitleNodeIndex = 0;
    
    /// <summary>
    /// Represents the index position in the <see cref="CalloutMdSyntaxNode"/> array
    /// where the body of a callout node is stored. This constant identifies the location
    /// of the child node that contains the body content for a callout.
    /// </summary>
    private const int BodyNodeIndex = 1;

    /// <summary>
    /// Represents the type of callout in a Markdown syntax node.
    /// </summary>
    /// <remarks>
    /// The <c>CalloutType</c> property is used to define the specific type of a callout element,
    /// such as "note", "warning", or other custom callout types. Its value influences how the
    /// callout is rendered or interpreted within the Markdown document structure.
    /// The property is immutable for external access and can only be set within the context
    /// of the class using the <c>WithCalloutType</c> method. Initially, it is set to an empty string.
    /// </remarks>
    public string CalloutType { get; private set; } = string.Empty;
    
    /// <summary>
    /// Represents the collapsed state of a callout in markdown syntax.
    /// </summary>
    /// <remarks>
    /// The <c>CollapsedState</c> property indicates whether a callout is collapsed, expanded, or has no explicit state.
    /// It uses the <c>CalloutCollapseStateOptions</c> enumeration to define its possible values:
    /// <list type="bullet">
    /// <item>
    /// <description><c>None</c>: No collapsed state is explicitly set.</description>
    /// </item>
    /// <item>
    /// <description><c>Open</c>: Indicates the callout is expanded.</description>
    /// </item>
    /// <item>
    /// <description><c>Closed</c>: Indicates the callout is collapsed.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public CalloutCollapseStateOptions CollapsedState { get; private set; }
    
    /// <summary>
    /// Represents the number of leading spaces before the callout content in a markdown document.
    /// </summary>
    /// <remarks>
    /// This property is intended to account for and normalize the indentation level of a callout section.
    /// It ensures that any whitespace preceding the callout is accurately represented, and mandates that
    /// the value is non-negative. Modifications to this property happen via the <c>WithLeadingSpaces</c> method.
    /// </remarks>
    public int LeadingSpaces { get; private set; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// Sets the callout type for the current node.
    /// <param name="calloutType">The callout type to be assigned to the node.</param>
    /// <return>The current instance of <see cref="CalloutMdSyntaxNode"/> with the updated callout type.</return>
    public CalloutMdSyntaxNode WithCalloutType(string calloutType) {
        CalloutType = calloutType;
        return this;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="CalloutMdSyntaxNode"/> with the specified number of leading spaces.
    /// </summary>
    /// <param name="leadingSpaces">The number of leading spaces to set. A value less than 0 will default to 0.</param>
    /// <returns>A new instance of the <see cref="CalloutMdSyntaxNode"/> with the updated leading spaces.</returns>
    public CalloutMdSyntaxNode WithLeadingSpaces(int leadingSpaces) {
        LeadingSpaces = Math.Max(0, leadingSpaces);
        return this;
    }

    /// Sets the collapse state of the callout.
    /// <param name="collapseState">
    /// The desired collapse state of the callout. Must be one of the values specified in the <see cref="CalloutCollapseStateOptions"/> enumeration.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="CalloutMdSyntaxNode"/> with the specified collapse state applied.
    /// </returns>
    public CalloutMdSyntaxNode WithCollapseState(CalloutCollapseStateOptions collapseState) {
        CollapsedState = collapseState;
        return this;
    }

    /// Attempts to set the title node for the current instance.
    /// If a title node is already set at the specific index, the operation will return false.
    /// <param name="titleNode">The title node to set as the child node for the current instance.</param>
    /// <return>True if the title node was set successfully; otherwise, false.</return>
    public bool TrySetTitle(CalloutTitleMdSyntaxNode titleNode)
        => TryAddChildNodeAtIndex(TitleNodeIndex, titleNode);

    /// Attempts to set the callout body node at the designated index within the node's children.
    /// The body node can only be set if the index is available and not already occupied, and if the
    /// provided node is valid for insertion.
    /// <param name="bodyNode">The callout body node to set at the specified index.</param>
    /// <returns>True if the callout body node was successfully set; otherwise, false.</returns>
    public bool TrySetBody(CalloutBodyMdSyntaxNode bodyNode)
        => TryAddChildNodeAtIndex(BodyNodeIndex, bodyNode);

    /// Attempts to get the title node of the current callout node.
    /// <param name="titleNode">
    /// When this method returns, contains the title node of the current callout node if the operation succeeded;
    /// otherwise, is null. This parameter is passed uninitialized.
    /// </param>
    /// <returns>
    /// true if the title node exists and was successfully retrieved; otherwise, false.
    /// </returns>
    public bool TryGetTitleNode([NotNullWhen(true)] out CalloutTitleMdSyntaxNode? titleNode) {
        titleNode = null;
        if (ChildCount == 0) return false;

        titleNode = ChildNodes[TitleNodeIndex] as CalloutTitleMdSyntaxNode;
        return titleNode is not null;
    }

    /// Tries to retrieve the body node of the current callout syntax node if it exists.
    /// <param name="bodyNode">
    /// When this method returns, contains the body node of the current
    /// callout syntax node if it exists; otherwise, null.
    /// </param>
    /// <returns>
    /// True if the body node is successfully retrieved; otherwise, false.
    /// </returns>
    public bool TryGetBodyNode([NotNullWhen(true)] out CalloutBodyMdSyntaxNode? bodyNode) {
        bodyNode = null;
        if (ChildCount == 0) return false;

        bodyNode = ChildNodes[BodyNodeIndex] as CalloutBodyMdSyntaxNode;
        return bodyNode is not null;
    }

    /// Configures the collapsed state of the callout node based on a provided option character.
    /// The option character determines the collapse state as follows:
    /// '+' sets the state to "Open," '-' sets it to "Closed," and any other value sets it to "None."
    /// <param name="option">
    /// A read-only span of characters from which the first character is used to determine the collapsed state.
    /// </param>
    public void WithExpandOption(ReadOnlySpan<char> option) {
        if (option.IsEmpty) return;

        char c = option[0];
        CollapsedState = c switch {
            '+' => CalloutCollapseStateOptions.Open,
            '-' => CalloutCollapseStateOptions.Closed,
            _ => CalloutCollapseStateOptions.None
        };
    }

    /// <inheritdoc />
    public override bool TryReset() {
        LeadingSpaces = 0;
        CalloutType = string.Empty;
        CollapsedState = CalloutCollapseStateOptions.None;
        return base.TryReset();
    }

    /// Determines whether the specified object is equal to the current object.
    /// <param name="other">The object to compare with the current object.</param>
    /// <return>True if the specified object is equal to the current object; otherwise, false.</return>
    protected override bool Equals(CalloutMdSyntaxNode? other)
        => base.Equals(other)
            && LeadingSpaces == other.LeadingSpaces
            && CollapsedState == other.CollapsedState
            && StringComparer.Ordinal.Equals(CalloutType, other.CalloutType);

    /// Returns a debug string representation of the current instance, including its base debug string and properties such as
    /// CalloutType, CollapsedState, and LeadingSpaces.
    /// <returns>A string representing the internal state of the node for debugging purposes.</returns>
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{CalloutType}' '{CollapseStateOptionsToString(CollapsedState)}'  LS={LeadingSpaces}";
    
    private static string CollapseStateOptionsToString(CalloutCollapseStateOptions state) => state switch {
        CalloutCollapseStateOptions.None => "",
        CalloutCollapseStateOptions.Open => "+",
        CalloutCollapseStateOptions.Closed => "-",
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}

/// Represents the possible collapse states for a callout element, indicating whether it is open, closed, or in a neutral state.
public enum CalloutCollapseStateOptions {
    /// Represents the default state of a callout where no collapse state is explicitly defined.
    None = 0,
    /// Represents the state where a callout is expanded and fully visible.
    Open = 1,
    /// Represents a state where the callout's content is collapsed and not visible.
    Closed = 2
}
