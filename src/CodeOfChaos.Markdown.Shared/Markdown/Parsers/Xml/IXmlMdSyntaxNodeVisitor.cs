// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown.Syntax;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlMdSyntaxNodeVisitor {
    XElement DeserializeToXml(IMdSyntaxNode node, XElement parentElement);
    IMdSyntaxNode SerializeToNode(IMdSyntaxTree tree, XElement element, IMdSyntaxNode parentNode);
}
