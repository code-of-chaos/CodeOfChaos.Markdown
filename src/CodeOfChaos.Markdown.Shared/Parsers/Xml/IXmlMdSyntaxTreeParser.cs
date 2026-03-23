// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Converts between markdown syntax trees and XML representations.
/// </summary>
public interface IXmlMdSyntaxTreeParser {
    /// <summary>
    /// Parses XML text into a syntax tree.
    /// </summary>
    /// <param name="input">The XML input.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeStringToSyntaxTree(string input);
    
    /// <summary>
    /// Parses an XML element into a syntax tree.
    /// </summary>
    /// <param name="element">The XML element.</param>
    /// <returns>The parsed syntax tree.</returns>
    IMdSyntaxTree SerializeToSyntaxTree(XElement element);
    /// <summary>
    /// Parses XML from a stream into a syntax tree.
    /// </summary>
    /// <param name="stream">The source stream.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The parsed syntax tree.</returns>
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default);
    /// <summary>
    /// Parses XML from a file into a syntax tree.
    /// </summary>
    /// <param name="filePath">The input file path.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The parsed syntax tree.</returns>
    Task<IMdSyntaxTree> SerializeFileToSyntaxTreeAsync(string filePath, CancellationToken ct = default);
    
    /// <summary>
    /// Serializes a syntax tree to XML text.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <returns>The XML text.</returns>
    string DeserializeToString(IMdSyntaxTree tree);
    /// <summary>
    /// Serializes a syntax tree to XML text asynchronously.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The XML text.</returns>
    Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default);
    
    /// <summary>
    /// Serializes a syntax tree to an XML element.
    /// </summary>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <returns>The XML element.</returns>
    XElement DeserializeToXmlElement(IMdSyntaxTree tree);
    /// <summary>
    /// Serializes a syntax tree to XML and writes it to a stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    Task DeserializeToXmlStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default);
    /// <summary>
    /// Serializes a syntax tree to XML and writes it to a file.
    /// </summary>
    /// <param name="filePath">The output file path.</param>
    /// <param name="tree">The syntax tree to serialize.</param>
    /// <param name="ct">The cancellation token.</param>
    Task DeserializeToXmlFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default);
}
