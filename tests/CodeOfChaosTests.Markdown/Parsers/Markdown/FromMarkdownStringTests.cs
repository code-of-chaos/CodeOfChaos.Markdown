// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Markdown;
using CodeOfChaos.Markdown.Markdown.Syntax;
using CodeOfChaosTests.Markdown.DataSources;
using CodeOfChaosTests.Shared;

namespace CodeOfChaosTests.Markdown.Parsers.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InfiniBlazorMarkdownDIDataSource]
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
