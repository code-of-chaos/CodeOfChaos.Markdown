// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.JSInterop;

namespace CodeOfChaosBenchmarks.Markdown.Mocks;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MockJsRuntime : IJSRuntime {
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => default;
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => default;
}
