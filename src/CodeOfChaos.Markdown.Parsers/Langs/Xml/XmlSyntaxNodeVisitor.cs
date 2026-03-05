// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Xml;

namespace CodeOfChaos.Markdown.Parsers.Langs.Xml;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class XmlSyntaxNodeVisitor<TSyntaxNode> : IXmlSyntaxNodeVisitor<TSyntaxNode> where TSyntaxNode : MdSyntaxNode<TSyntaxNode>, new() {
    private const string Modifiers = nameof(Modifiers);
    private const string OriginalInput = nameof(OriginalInput);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public void WriteToXml(
        XmlWriter writer,
        IMdSyntaxNode node,
        Action<IMdSyntaxNode> writeChildren
    ) {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(writeChildren);

        writer.WriteStartElement(node.Type.Name);

        DeserializeDetails(Unsafe.As<TSyntaxNode>(node), writer);

        writeChildren(node);

        writer.WriteEndElement();
    }

    public async ValueTask WriteToXmlAsync(
        XmlWriter writer,
        IMdSyntaxNode node,
        Func<IMdSyntaxNode, CancellationToken, ValueTask> writeChildren,
        CancellationToken ct = default
    ) {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(node);
        ArgumentNullException.ThrowIfNull(writeChildren);

        await writer.WriteStartElementAsync(prefix: null, localName: node.Type.Name, ns: null);

        DeserializeDetails(Unsafe.As<TSyntaxNode>(node), writer);

        await writeChildren(node, ct);

        await writer.WriteEndElementAsync();
    }

    protected virtual void DeserializeDetails(TSyntaxNode node, XmlWriter writer) {
        if (node.Modifier is not {} modifier) return;

        writer.WriteStartElement(Modifiers);
        writer.WriteStartElement(OriginalInput);
        writer.WriteString(modifier.OriginalInput);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }

    protected void WriteXmlPreserveSpace(XmlWriter writer) => writer.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");

    protected void WriteElementContent(XmlWriter writer, string? content) {
        if (content.IsNotNullOrEmpty()) {
            writer.WriteString(content);
        }
    }

    public IMdSyntaxNode ReadStartElement(IMdSyntaxTree tree, XmlReader reader, IMdSyntaxNode parentNode) {
        TSyntaxNode node = MdSyntaxNodePool<TSyntaxNode>.Shared.Get();
        parentNode.AddChildNode(node);

        SerializeDetails(reader, node);
        return node;
    }

    protected virtual void SerializeDetails(XmlReader reader, TSyntaxNode targetNode) {
    }

    public virtual void ReadTextContent(IMdSyntaxNode node, string content) {
        SerializeContent(Unsafe.As<TSyntaxNode>(node), content);
    }

    protected virtual void SerializeContent(TSyntaxNode targetNode, string content) {
    }

    public virtual bool TryReadSpecialChildElement(IMdSyntaxNode node, XmlReader reader) {
        if (!reader.LocalName.Equals(Modifiers, StringComparison.Ordinal)) return false;

        if (TryReadModifiers(reader, out MdSyntaxNodeModifier? modifier)) {
            node.WithModifier(modifier);
        }

        return true;
    }

    private static bool TryReadModifiers(XmlReader reader, [NotNullWhen(true)] out MdSyntaxNodeModifier? modifier) {
        modifier = null;

        if (reader.IsEmptyElement) {
            reader.Read();
            return false;
        }

        reader.Read();
        while (!(reader.NodeType == XmlNodeType.EndElement && reader.LocalName.Equals(Modifiers, StringComparison.Ordinal))) {
            switch (reader.NodeType) {
                case XmlNodeType.Element when reader.LocalName.Equals(OriginalInput, StringComparison.Ordinal): {
                    string originalInput = reader.ReadElementContentAsString();
                    modifier = MdSyntaxNodeModifier.FromString(originalInput);
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
        return modifier is not null;
    }

    protected static bool TryGetAttributeAsInt32(XmlReader reader, string propertyName, out int value) {
        value = 0;
        string? attr = reader.GetAttribute(propertyName);
        return attr is not null && int.TryParse(attr, out value);
    }

    protected static bool TryGetAttributeAsString(XmlReader reader, string propertyName, [NotNullWhen(true)] out string? value) {
        value = reader.GetAttribute(propertyName);
        return value is not null;
    }

    protected static bool TryGetAttributeAsEnum<TEnumType>(XmlReader reader, string propertyName, out TEnumType value) where TEnumType : struct {
        value = default;
        string? attr = reader.GetAttribute(propertyName);
        return attr is not null && Enum.TryParse(attr, out value);
    }
}
