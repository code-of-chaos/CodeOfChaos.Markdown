// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlSyntaxNodeVisitor {
    XElement DeserializeToXml(IMdSyntaxNode node, XElement parentElement);
    IMdSyntaxNode SerializeToNode(IMdSyntaxTree tree, XElement element, IMdSyntaxNode parentNode);
}
