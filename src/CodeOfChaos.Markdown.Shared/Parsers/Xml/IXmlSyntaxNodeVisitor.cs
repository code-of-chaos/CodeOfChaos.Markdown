// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlSyntaxNodeVisitor {
    ValueTask WriteToXmlAsync(
        XmlWriter writer,
        IMdSyntaxNode node,
        Func<IMdSyntaxNode, CancellationToken, ValueTask> writeChildren,
        CancellationToken ct = default
    );
    IMdSyntaxNode ReadStartElement(IMdSyntaxTree tree, XmlReader reader, IMdSyntaxNode parentNode);
    void ReadTextContent(IMdSyntaxNode node, string content);
    bool TryReadSpecialChildElement(IMdSyntaxNode node, XmlReader reader);
}

// ReSharper disable once UnusedTypeParameter
public interface IXmlSyntaxNodeVisitor<TSyntaxNode> : IXmlSyntaxNodeVisitor where TSyntaxNode : class, IMdSyntaxNode;
