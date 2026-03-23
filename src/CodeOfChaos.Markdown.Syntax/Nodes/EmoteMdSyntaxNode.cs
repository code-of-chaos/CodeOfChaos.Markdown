// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace CodeOfChaos.Markdown.Syntax.Nodes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class EmoteMdSyntaxNode() : MdSyntaxNode<EmoteMdSyntaxNode>(initialChildCount: 0) {

    /// <summary>
    /// Gets the key representing a specific emote. This property is used to uniquely identify an emote
    /// and typically serves as a parsed or normalized key derived from user-defined emote syntax in markdown.
    /// </summary>
    /// <remarks>
    /// The value is set through the <c>WithEmoteKey</c> method and defaults to an empty string.
    /// </remarks>
    public string EmoteKey { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the original representation of the emote used in the Markdown syntax node.
    /// </summary>
    /// <remarks>
    /// This property holds the raw or unprocessed form of the emote string as it
    /// exists in the source Markdown content. It can be utilized to preserve the
    /// original formatting or for debugging purposes during parsing or processing.
    /// </remarks>
    public string OriginalEmote { get; private set; } = string.Empty;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Sets the emote key for the current <see cref="EmoteMdSyntaxNode"/> instance.
    /// </summary>
    /// <param name="emoteKey">The emote key to assign to the current node.</param>
    /// <returns>The current instance of <see cref="EmoteMdSyntaxNode"/>.</returns>
    public EmoteMdSyntaxNode WithEmoteKey(string emoteKey) {
        EmoteKey = emoteKey;
        return this;
    }

    /// <summary>
    /// Sets the original emote associated with this <see cref="EmoteMdSyntaxNode"/>.
    /// </summary>
    /// <param name="originalEmote">
    /// The string representation of the original emote to be associated with this node.
    /// </param>
    /// <returns>
    /// Returns the current <see cref="EmoteMdSyntaxNode"/> instance with the updated original emote.
    /// </returns>
    public EmoteMdSyntaxNode WithOriginalEmote(string originalEmote) {
        OriginalEmote = originalEmote;
        return this;
    }

    /// <inheritdoc />
    public override bool TryReset() {
        EmoteKey = string.Empty;
        OriginalEmote = string.Empty;
        return base.TryReset();
    }

    /// <inheritdoc />
    protected override bool Equals(EmoteMdSyntaxNode? other)
        => base.Equals(other)
            && StringComparer.Ordinal.Equals(EmoteKey, other.EmoteKey)
            && StringComparer.Ordinal.Equals(OriginalEmote, other.OriginalEmote);
    
    /// <inheritdoc />
    public override string ToDebugString()
        => $"{base.ToDebugString()}: '{EmoteKey}'";
}
