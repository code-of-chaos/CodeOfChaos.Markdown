// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CodeOfChaos.Markdown;
using CodeOfChaos.Markdown.Markdown;
using CodeOfChaos.Markdown.Markdown.Syntax;
using CodeOfChaosBenchmarks.Markdown.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace CodeOfChaosBenchmarks.Markdown.Benchmarks;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.Declared)]
public class MarkdownBenchmark {
    private string Markdown { get; set; } = string.Empty;
    
    private IMarkdownParser Parser { get; set; } = null!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    [GlobalSetup]
    public async Task Setup() {
        // Read the file content
        const string filePath = "Benchmarks/MarkdownBenchmark.md";
        Markdown = await File.ReadAllTextAsync(filePath, new UTF8Encoding(encoderShouldEmitUTF8Identifier:false));
        Markdown = Markdown.ReplaceLineEndings("\n");
        
        if (Markdown.IsNullOrEmpty()) throw new InvalidOperationException("The Markdown input should not be empty.");
        
        ServiceProvider provider = CreateProvider();
        Parser = provider.GetRequiredService<IMarkdownParser>();
    }

    private static ServiceProvider CreateProvider(Action<InfiniBlazorMarkdownConfig>? configure = null) {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<Microsoft.AspNetCore.Components.NavigationManager, MockNavigationManager>();
        serviceCollection.AddSingleton<Microsoft.JSInterop.IJSRuntime, MockJsRuntime>();
        serviceCollection.AddInfiniBlazorMarkdown(config => configure?.Invoke(config));
        serviceCollection.AddLogging();
        return serviceCollection.BuildServiceProvider();
    }
    
    
    [Benchmark(Baseline = true)]
    public IMdSyntaxTree SerializeToSyntaxTree() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        return tree;
    }

    [Benchmark]
    public async Task<string> RenderToHtmlString() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        string? output = await Parser.Html.DeserializeToStringAsync(tree);
        return output ?? throw new InvalidOperationException("The Markdown output should not be empty.");
    }

    [Benchmark]
    public string RenderToMarkdown() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        string? output = Parser.Markdown.DeserializeToString(tree);
        return output ?? throw new InvalidOperationException("The Markdown output should not be empty.");
    }

    [Benchmark]
    public async Task<string> RenderToXmlString() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        string? output = await Parser.Xml.DeserializeToStringAsync(tree);
        return output ?? throw new InvalidOperationException("The Markdown output should not be empty.");
    }

    [Benchmark]
    public async Task<string> RenderToJsonString() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        string? output = await Parser.Json.DeserializeToStringAsync(tree);
        return output ?? throw new InvalidOperationException("The Markdown output should not be empty.");
    }
}