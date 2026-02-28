// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using System.Text.Json;

namespace CodeOfChaos.Markdown.Parsers.Json;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJsonMdSyntaxTreeParser {
    string DeserializeToString(IMdSyntaxTree input);
    Task<string> DeserializeToStringAsync(IMdSyntaxTree tree, CancellationToken ct = default);
    
    JsonElement DeserializeToJsonElement(IMdSyntaxTree tree);
    Task DeserializeToJsonStreamAsync(Stream stream, IMdSyntaxTree tree, CancellationToken ct = default);
    Task DeserializeToJsonFileAsync(string filePath, IMdSyntaxTree tree, CancellationToken ct = default);
    
    IMdSyntaxTree SerializeToSyntaxTree(string input);
    IMdSyntaxTree SerializeToSyntaxTree(JsonElement element);
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(Stream stream, CancellationToken ct = default);
    Task<IMdSyntaxTree> SerializeToSyntaxTreeAsync(string filePath, CancellationToken ct = default);
}
