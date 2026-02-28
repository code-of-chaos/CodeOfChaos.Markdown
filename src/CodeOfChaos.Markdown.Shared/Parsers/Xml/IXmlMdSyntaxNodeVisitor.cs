// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlMdSyntaxNodeVisitor {
    XElement DeserializeToXml(IMdSyntaxNode node, XElement parentElement);
    IMdSyntaxNode SerializeToNode(IMdSyntaxTree tree, XElement element, IMdSyntaxNode parentNode);
}
