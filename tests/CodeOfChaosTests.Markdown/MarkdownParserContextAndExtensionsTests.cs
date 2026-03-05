// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown;
using CodeOfChaos.Markdown.Parsers.Html;
using CodeOfChaos.Markdown.Parsers.Json;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using NSubstitute;
using System.Text.Json;
using System.Xml.Linq;

namespace CodeOfChaosTests.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MarkdownParserContextAndExtensionsTests {
    [Test]
    public async Task FromMarkdownString_ToMarkdownAsync_ShouldUseMarkdownParser() {
        // Arrange
        (IMarkdownParser parser, IMarkdownMdSyntaxTreeParser markdown, _, _, _) = CreateParser();
        var tree = new MdSyntaxTree();
        markdown.SerializeToSyntaxTree("**x**").Returns(tree);
        markdown.DeserializeToString(tree).Returns("out-md");

        // Act
        string result = await parser.FromMarkdownString("**x**").ToMarkdownAsync();

        // Assert
        await Assert.That(result).IsEqualTo("out-md");
        _ = markdown.Received(1).SerializeToSyntaxTree("**x**");
        _ = markdown.Received(1).DeserializeToString(tree);
    }

    [Test]
    public async Task FromMarkdownString_ToHtmlAsync_ShouldUseHtmlParser() {
        // Arrange
        (IMarkdownParser parser, IMarkdownMdSyntaxTreeParser markdown, _, _, IHtmlMdSyntaxTreeParser html) = CreateParser();
        var tree = new MdSyntaxTree();
        markdown.SerializeToSyntaxTree("x").Returns(tree);
        html.DeserializeToStringAsync(tree, Arg.Any<CancellationToken>()).Returns("out-html");

        // Act
        string result = await parser.FromMarkdownString("x").ToHtmlAsync();

        // Assert
        await Assert.That(result).IsEqualTo("out-html");
    }

    [Test]
    public async Task FromJsonString_ToJsonStringAsync_ShouldUseJsonParser() {
        // Arrange
        (IMarkdownParser parser, _, IJsonMdSyntaxTreeParser json, _, _) = CreateParser();
        var tree = new MdSyntaxTree();
        json.SerializeToSyntaxTree("{}").Returns(tree);
        json.DeserializeToStringAsync(tree, Arg.Any<CancellationToken>()).Returns("out-json");

        // Act
        string result = await parser.FromJsonString("{}").ToJsonStringAsync();

        // Assert
        await Assert.That(result).IsEqualTo("out-json");
        _ = json.Received(1).SerializeToSyntaxTree("{}");
    }

    [Test]
    public async Task FromJsonElement_ToJsonElementAsync_ShouldUseJsonParser() {
        // Arrange
        (IMarkdownParser parser, _, IJsonMdSyntaxTreeParser json, _, _) = CreateParser();
        JsonElement input = JsonDocument.Parse("""{"a":1}""").RootElement.Clone();
        JsonElement expected = JsonDocument.Parse("""{"type":"MdSyntaxTree"}""").RootElement.Clone();
        var tree = new MdSyntaxTree();

        json.SerializeToSyntaxTree(input).Returns(tree);
        json.DeserializeToJsonElement(tree).Returns(expected);

        // Act
        JsonElement result = await parser.FromJson(input).ToJsonElementAsync();

        // Assert
        await Assert.That(result.GetProperty("type").GetString()).IsEqualTo("MdSyntaxTree");
    }

    [Test]
    public async Task FromJsonStream_ToJsonFileAsync_ShouldUseAsyncJsonFactory() {
        // Arrange
        (IMarkdownParser parser, _, IJsonMdSyntaxTreeParser json, _, _) = CreateParser();
        var tree = new MdSyntaxTree();
        using var stream = new MemoryStream([1, 2, 3]);

        json.SerializeToSyntaxTreeAsync(stream, Arg.Any<CancellationToken>()).Returns(tree);

        // Act
        await parser.FromJson(stream).ToJsonFileAsync("a.json");

        // Assert
        await json.Received(1).DeserializeToJsonFileAsync("a.json", tree, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task FromJsonFile_ToJsonStreamAsync_ShouldUseAsyncJsonFactory() {
        // Arrange
        (IMarkdownParser parser, _, IJsonMdSyntaxTreeParser json, _, _) = CreateParser();
        var tree = new MdSyntaxTree();
        using var stream = new MemoryStream();

        json.SerializeToSyntaxTreeAsync("file.json", Arg.Any<CancellationToken>()).Returns(tree);

        // Act
        await parser.FromJsonFile("file.json").ToJsonStreamAsync(stream);

        // Assert
        await json.Received(1).DeserializeToJsonStreamAsync(stream, tree, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task FromXmlString_ToXmlStringAsync_ShouldUseXmlParser() {
        // Arrange
        (IMarkdownParser parser, _, _, IXmlMdSyntaxTreeParser xml, _) = CreateParser();
        var tree = new MdSyntaxTree();
        xml.SerializeStringToSyntaxTree("<x/>").Returns(tree);
        xml.DeserializeToStringAsync(tree, Arg.Any<CancellationToken>()).Returns("<out/>");

        // Act
        string result = await parser.FromXmlString("<x/>").ToXmlStringAsync();

        // Assert
        await Assert.That(result).IsEqualTo("<out/>");
    }

    [Test]
    public async Task FromXml_ToXmlElementAsync_ShouldUseXmlParser() {
        // Arrange
        (IMarkdownParser parser, _, _, IXmlMdSyntaxTreeParser xml, _) = CreateParser();
        XElement input = XElement.Parse("<x />");
        XElement expected = XElement.Parse("<out />");
        var tree = new MdSyntaxTree();
        xml.SerializeToSyntaxTree(input).Returns(tree);
        xml.DeserializeToXmlElement(tree).Returns(expected);

        // Act
        XElement result = await parser.FromXml(input).ToXmlElementAsync();

        // Assert
        await Assert.That(result.Name.LocalName).IsEqualTo("out");
    }

    [Test]
    public async Task FromXmlStream_ToXmlFileAsync_ShouldUseAsyncXmlFactory() {
        // Arrange
        (IMarkdownParser parser, _, _, IXmlMdSyntaxTreeParser xml, _) = CreateParser();
        var tree = new MdSyntaxTree();
        using var stream = new MemoryStream([1, 2, 3]);
        xml.SerializeToSyntaxTreeAsync(stream, Arg.Any<CancellationToken>()).Returns(tree);

        // Act
        await parser.FromXml(stream).ToXmlFileAsync("a.xml");

        // Assert
        await xml.Received(1).DeserializeToXmlFileAsync("a.xml", tree, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task FromXmlFile_ToXmlStreamAsync_ShouldUseAsyncXmlFactory() {
        // Arrange
        (IMarkdownParser parser, _, _, IXmlMdSyntaxTreeParser xml, _) = CreateParser();
        var tree = new MdSyntaxTree();
        using var stream = new MemoryStream();
        xml.SerializeFileToSyntaxTreeAsync("in.xml", Arg.Any<CancellationToken>()).Returns(tree);

        // Act
        await parser.FromXmlFile("in.xml").ToXmlStreamAsync(stream);

        // Assert
        await xml.Received(1).DeserializeToXmlStreamAsync(stream, tree, Arg.Any<CancellationToken>());
    }

    private static (IMarkdownParser Parser, IMarkdownMdSyntaxTreeParser Markdown, IJsonMdSyntaxTreeParser Json, IXmlMdSyntaxTreeParser Xml, IHtmlMdSyntaxTreeParser Html) CreateParser() {
        var parser = Substitute.For<IMarkdownParser>();
        var markdown = Substitute.For<IMarkdownMdSyntaxTreeParser>();
        var json = Substitute.For<IJsonMdSyntaxTreeParser>();
        var xml = Substitute.For<IXmlMdSyntaxTreeParser>();
        var html = Substitute.For<IHtmlMdSyntaxTreeParser>();
        parser.Markdown.Returns(markdown);
        parser.Json.Returns(json);
        parser.Xml.Returns(xml);
        parser.Html.Returns(html);

        return (parser, markdown, json, xml, html);
    }
}
