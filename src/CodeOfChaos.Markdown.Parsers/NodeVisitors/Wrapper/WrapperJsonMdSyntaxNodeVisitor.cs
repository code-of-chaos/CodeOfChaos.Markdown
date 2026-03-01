// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Json;
using CodeOfChaos.Markdown.Syntax.Nodes;

namespace CodeOfChaos.Markdown.Parsers.Json.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class WrapperJsonSyntaxNodeVisitor : JsonSyntaxNodeVisitor<WrapperMdSyntaxNode>;
