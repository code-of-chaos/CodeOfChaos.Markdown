// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Parsers.Xml;
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaosTests.Markdown.Parsers.DataSources;
using CodeOfChaosTests.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;

#pragma warning disable CS8604// Possible null reference argument.

namespace CodeOfChaosTests.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MarkdownDiDataSource]
public class MdTestDataProviderTests(IXmlMdSyntaxTreeParser parser) {
    private static readonly string StaticTestFolder = Path.Combine(Path.GetTempPath(), "CodeOfChaosTests.Markdown", "MdTestDataProviderTests");
    private MdTestDataProvider TempTestProvider { get; } = new(
        Substitute.For<ILogger<MdTestDataProvider>>(),
        parser,
        StaticTestFolder
    );

    private const string TestWriteAsyncFileName = "test_write_async.xml";
    private const string TestWriteSyncFileName = "test_write_sync.xml";
    private const string TestAddAsyncFileName = "test_add_async.xml";
    private const string TestAddSyncFileName = "test_add_sync.xml";
    private const string TestUpdateAsyncFileName = "test_update_async.xml";
    private const string TestUpdateSyncFileName = "test_update_sync.xml";
    private const string TestDeleteAsyncFileName = "test_delete_async.xml";
    private const string TestDeleteSyncFileName = "test_delete_sync.xml";
    private const string TestDeleteNonexistentAsyncFileName = "test_delete_nonexistent_async.xml";
    private const string TestDeleteNonexistentSyncFileName = "test_delete_nonexistent_sync.xml";
    private const string TestInvalidAsyncFileName = "test_invalid_async.xml";
    private const string TestInvalidSyncFileName = "test_invalid_sync.xml";

    [After(Class)]
    public static void CleanupTestFiles() {
        string[] testFiles = [
            TestWriteAsyncFileName,
            TestWriteSyncFileName,
            TestAddAsyncFileName,
            TestAddSyncFileName,
            TestUpdateAsyncFileName,
            TestUpdateSyncFileName,
            TestDeleteAsyncFileName,
            TestDeleteSyncFileName,
            TestDeleteNonexistentAsyncFileName,
            TestDeleteNonexistentSyncFileName,
            TestInvalidAsyncFileName,
            TestInvalidSyncFileName
        ];

        foreach (string fileName in testFiles) {
            string fullPath = Path.Combine(StaticTestFolder, fileName);
            if (!File.Exists(fullPath)) continue;

            try {
                File.Delete(fullPath);
            }
            catch {
                // Ignore cleanup failures
            }
        }
    }


    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [Test]
    public async Task TryGetFileNames_ReturnsExpected() {
        // Arrange

        // Act
        string[]? fileNames = MdTestDataProvider.TestInstance.TryGetFileNames();

        // Assert
        await Assert.That(fileNames)
            .IsNotNull()
            .Count().IsGreaterThanOrEqualTo(2);

        await Assert.That(fileNames)
            .Contains(filePath => filePath == "bold.xml");
    }

    [Test]
    [Arguments("aggregate.xml", 1)]
    [Arguments("blockquote.xml", 1)]
    [Arguments("bold.xml", 1)]
    [Arguments("break.xml", 1)]
    [Arguments("callout.xml", 1)]
    [Arguments("code_block.xml", 1)]
    [Arguments("code_inline.xml", 1)]
    [Arguments("combo_bold_italic.xml", 1)]
    [Arguments("emote.xml", 1)]
    [Arguments("escaped_char.xml", 1)]
    [Arguments("footnote.xml", 1)]
    [Arguments("frontmatter.xml", 1)]
    [Arguments("heading.xml", 1)]
    [Arguments("highlight.xml", 1)]
    [Arguments("horizontal_line.xml", 1)]
    [Arguments("html.xml", 1)]
    [Arguments("image.xml", 1)]
    [Arguments("italic.xml", 1)]
    [Arguments("link.xml", 1)]
    [Arguments("list_ordered.xml", 1)]
    [Arguments("list_unordered.xml", 1)]
    [Arguments("span.xml", 1)]
    [Arguments("special_char.xml", 1)]
    [Arguments("strike.xml", 1)]
    [Arguments("subscript.xml", 1)]
    [Arguments("superscript.xml", 1)]
    [Arguments("table.xml", 1)]
    [Arguments("tag.xml", 1)]
    [Arguments("template.xml", 1)]
    [Arguments("underline.xml", 1)]
    [Arguments("user.xml", 1)]
    [Arguments("wikilink.xml", 1)]
    [Arguments("wrapper.xml", 1)]
    public async Task TryGetXmlMdTestDataAsync_ReturnsDataSet(string fileName, int minimumExpectedCount) {
        // Arrange

        // Act
        List<MdTestData>? data = await MdTestDataProvider.TestInstance.TryGetXmlMdTestDataAsync(fileName);

        // Assert
        await Assert.That(data)
            .IsNotNull()
            .Count().IsGreaterThanOrEqualTo(minimumExpectedCount);
    }

    [Test]
    [Arguments("aggregate.xml", 1)]
    [Arguments("blockquote.xml", 1)]
    [Arguments("bold.xml", 1)]
    [Arguments("break.xml", 1)]
    [Arguments("callout.xml", 1)]
    [Arguments("code_block.xml", 1)]
    [Arguments("code_inline.xml", 1)]
    [Arguments("combo_bold_italic.xml", 1)]
    [Arguments("emote.xml", 1)]
    [Arguments("escaped_char.xml", 1)]
    [Arguments("footnote.xml", 1)]
    [Arguments("frontmatter.xml", 1)]
    [Arguments("heading.xml", 1)]
    [Arguments("highlight.xml", 1)]
    [Arguments("horizontal_line.xml", 1)]
    [Arguments("html.xml", 1)]
    [Arguments("image.xml", 1)]
    [Arguments("italic.xml", 1)]
    [Arguments("link.xml", 1)]
    [Arguments("list_ordered.xml", 1)]
    [Arguments("list_unordered.xml", 1)]
    [Arguments("span.xml", 1)]
    [Arguments("special_char.xml", 1)]
    [Arguments("strike.xml", 1)]
    [Arguments("subscript.xml", 1)]
    [Arguments("superscript.xml", 1)]
    [Arguments("table.xml", 1)]
    [Arguments("tag.xml", 1)]
    [Arguments("template.xml", 1)]
    [Arguments("underline.xml", 1)]
    [Arguments("user.xml", 1)]
    [Arguments("wikilink.xml", 1)]
    [Arguments("wrapper.xml", 1)]
    public async Task TryGetXmlMdTestData_ReturnsDataSet(string fileName, int minimumExpectedCount) {
        // Arrange

        // Act
        List<MdTestData>? data = MdTestDataProvider.TestInstance.TryGetXmlMdTestData(fileName);

        // Assert
        await Assert.That(data)
            .IsNotNull()
            .Count().IsGreaterThanOrEqualTo(minimumExpectedCount);
    }

    [Test]
    [Arguments("nonexistent.xml")]
    public async Task TryGetXmlMdTestDataAsync_NonexistentFile_ReturnsNull(string fileName) {
        // Arrange

        // Act
        List<MdTestData>? data = await TempTestProvider.TryGetXmlMdTestDataAsync(fileName);

        // Assert
        await Assert.That(data).IsNull();
    }

    [Test]
    [Arguments("nonexistent.xml")]
    public async Task TryGetXmlMdTestData_NonexistentFile_ReturnsNull(string fileName) {
        // Arrange

        // Act
        List<MdTestData>? data = TempTestProvider.TryGetXmlMdTestData(fileName);

        // Assert
        await Assert.That(data).IsNull();
    }

    [Test]
    public async Task TryWriteXmlMdTestDataAsync_ValidData_ReturnsTrue() {
        // Arrange
        var testData = new List<MdTestData> {
            new() {
                Id = "testId",
                FileName = "test_write_async.xml",
                MdString = "**test markdown**",
                MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
            }
        };

        // Act
        bool result = await TempTestProvider.TryWriteXmlMdTestDataAsync(TestWriteAsyncFileName, testData);

        // Assert
        await Assert.That(result).IsTrue();

        // Cleanup - a file was created and can be read back
        List<MdTestData>? readBackData = await TempTestProvider.TryGetXmlMdTestDataAsync(TestWriteAsyncFileName);
        await Assert.That(readBackData).IsNotNull();
        await Assert.That(readBackData.First().Id).IsEqualTo("testId");
    }

    [Test]
    public async Task TryWriteXmlMdTestData_ValidData_ReturnsTrue() {
        // Arrange
        var testData = new List<MdTestData> {
            new() {
                Id = "testId",
                FileName = "test_write_sync.xml",
                MdString = "**test markdown**",
                MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
            }
        };

        // Act
        bool result = TempTestProvider.TryWriteXmlMdTestData(TestWriteSyncFileName, testData);

        // Assert
        await Assert.That(result).IsTrue();

        // Cleanup - a file was created and can be read back
        List<MdTestData>? readBackData = TempTestProvider.TryGetXmlMdTestData(TestWriteSyncFileName);
        await Assert.That(readBackData).IsNotNull();
        await Assert.That(readBackData.First().Id).IsEqualTo("testId");
    }

    [Test]
    public async Task TryAddOrUpdateAsync_NewItem_AddsSuccessfully() {
        // Arrange
        var newTestData = new MdTestData {
            Id = "newTestId",
            FileName = "test_add_async.xml",
            MdString = "*new test markdown*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // Act
        bool result = await TempTestProvider.TryAddOrUpdateAsync(TestAddAsyncFileName, newTestData);

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was added
        List<MdTestData>? data = await TempTestProvider.TryGetXmlMdTestDataAsync(TestAddAsyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Any(x => x.Id == "newTestId")).IsTrue();
    }

    [Test]
    public async Task TryAddOrUpdate_NewItem_AddsSuccessfully() {
        // Arrange
        var newTestData = new MdTestData {
            Id = "newTestId",
            FileName = "test_add_sync.xml",
            MdString = "*new test markdown*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // Act
        bool result = TempTestProvider.TryAddOrUpdate(TestAddSyncFileName, newTestData);

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was added
        List<MdTestData>? data = TempTestProvider.TryGetXmlMdTestData(TestAddSyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Any(x => x.Id == "newTestId")).IsTrue();
    }

    [Test]
    public async Task TryAddOrUpdateAsync_ExistingItem_UpdatesSuccessfully() {
        // Arrange
        var initialTestData = new MdTestData {
            Id = "updateTestId",
            FileName = "test_update_async.xml",
            MdString = "*initial markdown*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // First, add the initial item
        await TempTestProvider.TryAddOrUpdateAsync(TestUpdateAsyncFileName, initialTestData);

        var updatedTestData = new MdTestData {
            Id = "updateTestId",// Same ID
            FileName = "test_update_async.xml",
            MdString = "**updated markdown**",// Different content
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // Act
        bool result = await TempTestProvider.TryAddOrUpdateAsync(TestUpdateAsyncFileName, updatedTestData);

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was updated
        List<MdTestData>? data = await TempTestProvider.TryGetXmlMdTestDataAsync(TestUpdateAsyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Count).IsEqualTo(1);// Should still be only one item
        await Assert.That(data.First().MdString).IsEqualTo("**updated markdown**");
    }

    [Test]
    public async Task TryAddOrUpdate_ExistingItem_UpdatesSuccessfully() {
        // Arrange
        var initialTestData = new MdTestData {
            Id = "updateTestId",
            FileName = "test_update_sync.xml",
            MdString = "*initial markdown*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // First, add the initial item
        TempTestProvider.TryAddOrUpdate(TestUpdateSyncFileName, initialTestData);

        var updatedTestData = new MdTestData {
            Id = "updateTestId",// Same ID
            FileName = "test_update_sync.xml",
            MdString = "**updated markdown**",// Different content
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // Act
        bool result = TempTestProvider.TryAddOrUpdate(TestUpdateSyncFileName, updatedTestData);

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was updated
        List<MdTestData>? data = TempTestProvider.TryGetXmlMdTestData(TestUpdateSyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Count).IsEqualTo(1);// Should still be only one item
        await Assert.That(data.First().MdString).IsEqualTo("**updated markdown**");
    }

    [Test]
    public async Task TryDeleteAsync_ExistingItem_DeletesSuccessfully() {
        // Arrange
        var testData = new MdTestData {
            Id = "deleteTestId",
            FileName = "test_delete_async.xml",
            MdString = "*to be deleted*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // First, add the item
        await TempTestProvider.TryAddOrUpdateAsync(TestDeleteAsyncFileName, testData);

        // Act
        bool result = await TempTestProvider.TryDeleteAsync(TestDeleteAsyncFileName, "deleteTestId");

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was deleted
        List<MdTestData>? data = await TempTestProvider.TryGetXmlMdTestDataAsync(TestDeleteAsyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Count).IsEqualTo(0);
    }

    [Test]
    public async Task TryDelete_ExistingItem_DeletesSuccessfully() {
        // Arrange
        var testData = new MdTestData {
            Id = "deleteTestId",
            FileName = "test_delete_sync.xml",
            MdString = "*to be deleted*",
            MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
        };

        // First, add the item
        TempTestProvider.TryAddOrUpdate(TestDeleteSyncFileName, testData);

        // Act
        bool result = TempTestProvider.TryDelete(TestDeleteSyncFileName, "deleteTestId");

        // Assert
        await Assert.That(result).IsTrue();

        // Verify the item was deleted
        List<MdTestData>? data = TempTestProvider.TryGetXmlMdTestData(TestDeleteSyncFileName);
        await Assert.That(data).IsNotNull();
        await Assert.That(data.Count).IsEqualTo(0);
    }

    [Test]
    public async Task TryDeleteAsync_NonexistentItem_ReturnsFalse() {
        // Arrange

        // Act
        bool result = await TempTestProvider.TryDeleteAsync(TestDeleteNonexistentAsyncFileName, "nonexistentId");

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryDelete_NonexistentItem_ReturnsFalse() {
        // Arrange

        // Act
        bool result = TempTestProvider.TryDelete(TestDeleteNonexistentSyncFileName, "nonexistentId");

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryWriteXmlMdTestDataAsync_InvalidData_ReturnsFalse() {
        // Arrange
        var invalidTestData = new List<MdTestData> {
            new() {
                Id = "",// Invalid - empty ID
                FileName = "test_invalid_async.xml",
                MdString = "**test markdown**",
                MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
            }
        };

        // Act
        bool result = await TempTestProvider.TryWriteXmlMdTestDataAsync(TestInvalidAsyncFileName, invalidTestData);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task TryWriteXmlMdTestData_InvalidData_ReturnsFalse() {
        // Arrange
        var invalidTestData = new List<MdTestData> {
            new() {
                Id = "",// Invalid - empty ID
                FileName = "test_invalid_sync.xml",
                MdString = "**test markdown**",
                MdSyntaxTree = new MdSyntaxTree()// Don't process the Syntax tree, not needed for anything here
            }
        };

        // Act
        bool result = TempTestProvider.TryWriteXmlMdTestData(TestInvalidSyncFileName, invalidTestData);

        // Assert
        await Assert.That(result).IsFalse();
    }
}
