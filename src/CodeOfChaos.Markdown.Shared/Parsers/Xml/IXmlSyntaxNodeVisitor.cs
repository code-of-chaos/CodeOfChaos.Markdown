// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Serializes and deserializes a syntax node type to and from XML.
/// </summary>
public interface IXmlSyntaxNodeVisitor {
    /// <summary>
    /// Writes a node to XML.
    /// </summary>
    /// <param name="writer">The target writer.</param>
    /// <param name="node">The node to write.</param>
    /// <param name="writeChildren">Delegate for writing child nodes.</param>
    void WriteToXml(
        XmlWriter writer,
        IMdSyntaxNode node,
        Action<IMdSyntaxNode> writeChildren
    );
    /// <summary>
    /// Writes a node to XML asynchronously.
    /// </summary>
    /// <param name="writer">The target writer.</param>
    /// <param name="node">The node to write.</param>
    /// <param name="writeChildren">Delegate for writing child nodes.</param>
    /// <param name="ct">The cancellation token.</param>
    ValueTask WriteToXmlAsync(
        XmlWriter writer,
        IMdSyntaxNode node,
        Func<IMdSyntaxNode, CancellationToken, ValueTask> writeChildren,
        CancellationToken ct = default
    );
    /// <summary>
    /// Reads the start element and creates a node under the provided parent.
    /// </summary>
    /// <param name="tree">The target syntax tree.</param>
    /// <param name="reader">The source XML reader.</param>
    /// <param name="parentNode">The parent node.</param>
    /// <returns>The created syntax node.</returns>
    IMdSyntaxNode ReadStartElement(IMdSyntaxTree tree, XmlReader reader, IMdSyntaxNode parentNode);
    /// <summary>
    /// Reads element text content into a node.
    /// </summary>
    /// <param name="node">The target node.</param>
    /// <param name="content">The element text content.</param>
    void ReadTextContent(IMdSyntaxNode node, string content);
    /// <summary>
    /// Tries to consume a visitor-specific special child element.
    /// </summary>
    /// <param name="node">The current node.</param>
    /// <param name="reader">The source reader.</param>
    /// <returns><see langword="true" /> when the element is consumed; otherwise <see langword="false" />.</returns>
    bool TryReadSpecialChildElement(IMdSyntaxNode node, XmlReader reader);
}

// ReSharper disable once UnusedTypeParameter
public interface IXmlSyntaxNodeVisitor<TSyntaxNode> : IXmlSyntaxNodeVisitor where TSyntaxNode : class, IMdSyntaxNode;
