// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaosTests.Markdown.Parsers.DataSources;
using CodeOfChaosTests.Shared;

namespace CodeOfChaosTests.Markdown.Parsers.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MarkdownDiDataSource]
public partial class FromMarkdownTests(IMarkdownParser parser) {

    [Test]
    [MethodDataSource<MdTestDataSources>(nameof(MdTestDataSources.GetBlankTest))]
    [MethodDataSource(typeof(MdTestDataSources), nameof(MdTestDataSources.GetTestDataAsync))]
    public async Task FromMarkdown_ToSyntaxTree_ShouldBeSame(MdTestData data) {
        // Arrange
        string input = data.MdString;
        IMdSyntaxTree expectedOutput = data.MdSyntaxTree;

        // Act
        IMdSyntaxTree foundTree = parser.Markdown.SerializeToSyntaxTree(input);

        // Assert
        await Assert.That(foundTree).IsNotNull();
        await Assert.That(foundTree).IsEquatableTo(expectedOutput);
    }
}
