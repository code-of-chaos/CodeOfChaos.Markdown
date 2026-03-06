// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Syntax;
using CodeOfChaos.Markdown.Syntax.Nodes;

namespace CodeOfChaosTests.Markdown.Syntax;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MdSyntaxTreeAndScriptingNodeTests {
    [Test]
    public async Task MdSyntaxTree_VisitOrdersAndCount_ShouldBeConsistent() {
        // Arrange
        var tree = new MdSyntaxTree();
        var p1 = new ParagraphMdSyntaxNode();
        var p2 = new ParagraphMdSyntaxNode();
        TextMdSyntaxNode t1 = new TextMdSyntaxNode().WithContent("a");
        TextMdSyntaxNode t2 = new TextMdSyntaxNode().WithContent("b");
        p1.WithChild(t1);
        p2.WithChild(t2);
        tree.RootNode.WithChild(p1);
        tree.RootNode.WithChild(p2);

        // Act
        IMdSyntaxNode[] topLevel = tree.VisitTopLevelNodes().ToArray();
        IMdSyntaxNode[] breadth = tree.VisitNodesBreadthFirst().ToArray();
        IMdSyntaxNode[] deepest = tree.VisitNodesDeepestFirst().ToArray();
        int count = tree.GetCount();

        // Assert
        await Assert.That(topLevel).Count().IsEqualTo(2);
        await Assert.That(breadth).Count().IsEqualTo(4);
        await Assert.That(deepest).Count().IsEqualTo(4);
        await Assert.That(count).IsEqualTo(4);
        await Assert.That(breadth[0]).IsSameReferenceAs(p1);
        await Assert.That(deepest[0]).IsSameReferenceAs(t1);
    }

    [Test]
    public async Task MdSyntaxTree_ClearCachesAndReset_ShouldSucceed() {
        // Arrange
        var tree = new MdSyntaxTree();
        tree.RootNode.WithChild(new ParagraphMdSyntaxNode());
        bool firstCacheHit = tree.TryGetCachedChildrenByType<ParagraphMdSyntaxNode>(out IEnumerable<ParagraphMdSyntaxNode>? cachedBefore);

        // Act
        tree.ClearCaches();
        bool secondCacheHit = tree.TryGetCachedChildrenByType<ParagraphMdSyntaxNode>(out IEnumerable<ParagraphMdSyntaxNode>? cachedAfter);
        bool resetResult = tree.TryReset();

        // Assert
        await Assert.That(firstCacheHit).IsTrue();
        await Assert.That(cachedBefore).IsNotNull();
        await Assert.That(secondCacheHit).IsTrue();
        await Assert.That(cachedAfter).IsNotNull();
        await Assert.That(resetResult).IsTrue();
        await Assert.That(tree.RootNode.ChildCount).IsEqualTo(0);
    }

    [Test]
    public async Task MdSyntaxTree_TryGetCachedChildrenByType_InvalidType_ShouldReturnFalse() {
        // Arrange
        var tree = new MdSyntaxTree();

        // Act
        bool result = tree.TryGetCachedChildrenByType(typeof(string), out IEnumerable<IMdSyntaxNode>? nodes);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(nodes).IsNull();
    }

    [Test]
    public async Task CodeInline_TryGetContentWithoutEscapedBackTicks_ShouldHandleEscapesAndSmallBuffers() {
        // Arrange
        CodeInlineMdSyntaxNode node = new CodeInlineMdSyntaxNode()
            .WithContent(@"a\`b")
            .WithBackTickCount(0);
        Span<char> successBuffer = stackalloc char[8];
        Span<char> failBuffer = stackalloc char[1];

        // Act
        bool success = node.TryGetContentWithoutEscapedBackTicks(successBuffer, out int successLength);
        bool fail = node.TryGetContentWithoutEscapedBackTicks(failBuffer, out int failLength);
        string cleaned = successBuffer[..successLength].ToString();

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(successLength).IsEqualTo(3);
        await Assert.That(cleaned).IsEqualTo("a`b");
        await Assert.That(fail).IsFalse();
        await Assert.That(failLength).IsEqualTo(-1);
        await Assert.That(node.BackTickCount).IsEqualTo(1);
    }
}
