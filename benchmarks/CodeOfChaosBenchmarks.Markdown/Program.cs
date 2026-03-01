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
        BenchmarkSwitcher
            .FromTypes([typeof(MarkdownBenchmark), typeof(IndividualMarkdownBenchmarks)])
            .Run(args);
    }
}
