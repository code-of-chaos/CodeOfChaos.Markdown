// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Langs.Xml;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.NodeVisitors;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public sealed class TableXmlSyntaxNodeVisitor : XmlSyntaxNodeVisitor<TableMdSyntaxNode> {
    private const string Alignments = nameof(TableMdSyntaxNode.Alignments);
    private const string Alignment = nameof(Alignment);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override void DeserializeDetails(TableMdSyntaxNode node, XmlWriter writer) {
        base.DeserializeDetails(node, writer);

        if (!node.HasAlignments) return;

        writer.WriteStartElement(Alignments);
        int columCount = node.GetHeaderCells().Length;
        foreach (TableAlignment alignment in node.Alignments.AsSpan(0, columCount)) {
            writer.WriteElementString(Alignment, Enum.GetName(alignment));
        }
        writer.WriteEndElement();
    }

    public override bool TryReadSpecialChildElement(IMdSyntaxNode node, XmlReader reader) {
        if (base.TryReadSpecialChildElement(node, reader)) return true;
        if (!reader.LocalName.Equals(Alignments, StringComparison.Ordinal)) return false;

        var targetNode = (TableMdSyntaxNode)node;
        var alignments = new List<TableAlignment>();

        if (reader.IsEmptyElement) {
            reader.Read();
            return true;
        }

        reader.Read();
        while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals(Alignments, StringComparison.Ordinal))) {
            switch (reader.NodeType) {
                case XmlNodeType.Element when reader.LocalName.Equals(Alignment, StringComparison.Ordinal): {
                    string value = reader.ReadElementContentAsString();
                    alignments.Add(Enum.Parse<TableAlignment>(value));
                    continue;
                }

                case XmlNodeType.Element:
                    reader.Skip();
                    break;

                case XmlNodeType.None:
                case XmlNodeType.Attribute:
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.EntityReference:
                case XmlNodeType.Entity:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.Comment:
                case XmlNodeType.Document:
                case XmlNodeType.DocumentType:
                case XmlNodeType.DocumentFragment:
                case XmlNodeType.Notation:
                case XmlNodeType.Whitespace:
                case XmlNodeType.SignificantWhitespace:
                case XmlNodeType.EndElement:
                case XmlNodeType.EndEntity:
                case XmlNodeType.XmlDeclaration:
                default:
                    reader.Read();
                    break;
            }

        }

        reader.Read();

        if (alignments.Count > 0) {
            targetNode.WithAlignments(alignments.ToArray());
        }

        return true;
    }
}
