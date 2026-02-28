// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Xml.Linq;

namespace CodeOfChaos.Markdown.Parsers.Xml;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IXmlMdSyntaxTreeParser {
    IMdSyntaxTree SerializeStringToSyntaxTree(string input);
    
    IMdSyntaxTree SerializeToSyntaxTree(XElement element);
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default);
    Task<IMdSyntaxTree> SerializeFileToSyntaxTreeAsync(string filePath, CancellationToken ct = default);
    
    string DeserializeToString(IMdSyntaxTree tree);
    Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default);
    
    XElement DeserializeToXmlElement(IMdSyntaxTree tree);
    Task DeserializeToXmlStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default);
    Task DeserializeToXmlFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default);
}
