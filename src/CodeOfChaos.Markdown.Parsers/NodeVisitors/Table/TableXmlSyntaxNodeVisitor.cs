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
        foreach (TableMdSyntaxNode.Alignment alignment in node.Alignments.AsSpan(0, columCount)) {
            writer.WriteElementString(Alignment, Enum.GetName(alignment));
        }
        writer.WriteEndElement();
    }

    public override bool TryReadSpecialChildElement(IMdSyntaxNode node, XmlReader reader) {
        if (base.TryReadSpecialChildElement(node, reader)) return true;
        if (!reader.LocalName.Equals(Alignments, StringComparison.Ordinal)) return false;

        var targetNode = (TableMdSyntaxNode)node;
        var alignments = new List<TableMdSyntaxNode.Alignment>();

        if (reader.IsEmptyElement) {
            reader.Read();
            return true;
        }

        reader.Read();
        while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals(Alignments, StringComparison.Ordinal))) {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName.Equals(Alignment, StringComparison.Ordinal)) {
                string value = reader.ReadElementContentAsString();
                alignments.Add(Enum.Parse<TableMdSyntaxNode.Alignment>(value));
                continue;
            }

            if (reader.NodeType == XmlNodeType.Element) {
                reader.Skip();
            } else {
                reader.Read();
            }
        }

        reader.Read();

        if (alignments.Count > 0) {
            targetNode.WithAlignments(alignments.ToArray());
        }

        return true;
    }
}
