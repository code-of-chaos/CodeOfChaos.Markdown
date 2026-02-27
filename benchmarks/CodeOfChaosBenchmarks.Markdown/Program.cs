// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using BenchmarkDotNet.Running;
using CodeOfChaosBenchmarks.Markdown.Benchmarks;

namespace CodeOfChaosBenchmarks.Markdown;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class Program {
    public static void Main(string[] args) {
        BenchmarkRunner.Run<MarkdownBenchmark>();
        // BenchmarkRunner.Run<IndividualMarkdownBenchmarks>();
    }
}
