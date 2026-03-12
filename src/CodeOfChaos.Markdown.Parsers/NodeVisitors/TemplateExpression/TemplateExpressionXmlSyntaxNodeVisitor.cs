// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions;
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <inheritdoc />
public sealed class TemplateExpressionXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<TemplateExpressionMdSyntaxNode> {
    private static readonly string ExpressionType = nameof(TemplateExpressionMdSyntaxNode.ExpressionType).ToCamelCase();
    private static readonly string Body = nameof(TemplateExpressionMdSyntaxNode.Expression).ToCamelCase();
    private static readonly string LeadingSpaces = nameof(TemplateExpressionMdSyntaxNode.LeadingSpaces).ToCamelCase();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    protected override void DeserializeDetails(TemplateExpressionMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);

        writer.WriteAttributeString(ExpressionType, Enum.GetName(node.ExpressionType));
        writer.WriteAttributeString(Body, node.Expression);
        writer.WriteAttributeString(LeadingSpaces, node.LeadingSpaces.ToString());
    }

    /// <inheritdoc />
    protected override void SerializeDetails(XmlReader reader, TemplateExpressionMdSyntaxNode targetNode) {
        base.SerializeDetails(reader, targetNode);

        if (TryGetAttributeAsString(reader, Body, out string? content)) {
            targetNode.WithExpression(content);   
        }

        if (TryGetAttributeAsEnum(reader, ExpressionType, out TemplateExpressionType expressionType)) {
            targetNode.WithExpressionType(expressionType);  
        }
        
        if (TryGetAttributeAsInt32(reader, LeadingSpaces, out int leadingSpaces)) {
            targetNode.WithLeadingSpaces(leadingSpaces);
        }
    }

}
