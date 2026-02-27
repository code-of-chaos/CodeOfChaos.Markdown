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
    public async Task<string> RenderMarkdown() {
        IMdSyntaxTree tree = Parser.Markdown.SerializeToSyntaxTree(Markdown);
        string? output = await Parser.Html.DeserializeToStringAsync(tree);
        return output ?? throw new InvalidOperationException("The Markdown input should not be empty.");
    }
    
    // [Benchmark()]
    // public async ValueTask<string> RenderMarkdownSanitized() {
    //     string input = Markdown;
    //     string? output = await SanitizedParser.TryParseAsync(input);
    //     if(output is null) throw new InvalidOperationException("The Markdown input should not be empty.");
    //     return output;
    // }
}