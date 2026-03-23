// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts between markdown syntax trees and JSON representations.
/// </summary>
public interface IJsonMdSyntaxTreeParser {
    /// <summary>
    /// Serializes a syntax tree to JSON text.
    /// </summary>
    /// <param name="input">The syntax tree to serialize.</param>
    /// <returns>The JSON text.</returns>
    string DeserializeToString(IMdSyntaxTree input);
    /// <summary>
    /// Serializes a syntax tree to JSON text asynchronously.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The JSON text.</returns>
    Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default);
    
    /// <summary>
    /// Serializes a syntax tree to a <see cref="JsonElement" />.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <returns>The JSON element.</returns>
    JsonElement DeserializeToJsonElement(IMdSyntaxTree tree);
    /// <summary>
    /// Serializes a syntax tree to JSON and writes it to a stream.
    /// </summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    Task DeserializeToJsonStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default);
    /// <summary>
    /// Serializes a syntax tree to JSON and writes it to a file.
    /// </summary>
    /// <param name="filePath">The output file path.</param>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    Task DeserializeToJsonFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default);
    
    /// <summary>
    /// Parses JSON text into a syntax tree.
    /// </summary>
    /// <param name="input">The JSON input text.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeToSyntaxTree(string input);
    /// <summary>
    /// Parses a JSON element into a syntax tree.
    /// </summary>
    /// <param name="element">The JSON element input.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeToSyntaxTree(JsonElement element);
    /// <summary>
    /// Parses JSON content from a stream into a syntax tree.
    /// </summary>
    /// <param name="stream">The JSON stream.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The parsed syntax tree.</returns>
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default);
    /// <summary>
    /// Parses JSON content from a file into a syntax tree.
    /// </summary>
    /// <param name="filePath">The JSON file path.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The parsed syntax tree.</returns>
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(string filePath, CancellationToken ct = default);
}
