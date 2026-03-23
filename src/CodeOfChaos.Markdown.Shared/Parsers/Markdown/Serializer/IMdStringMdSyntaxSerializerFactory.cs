// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;

namespace CodeOfChaos.Markdown.Parsers.Markdown.Serializer;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Creates configured markdown string serializer instances.
/// </summary>
public interface IMdStringMdSyntaxSerializerFactory {
    /// <summary>
    /// Creates a serializer using the provided markdown configuration.
    /// </summary>
    /// <param name="config">The configuration containing markdown visitors.</param>
    /// <returns>A configured serializer instance.</returns>
    IMdStringMdSyntaxSerializer Create(IMarkdownConfig config);
}
