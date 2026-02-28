// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace CodeOfChaosBenchmarks.Markdown.Mocks;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class MockNavigationManager : NavigationManager {
    public MockNavigationManager() => Initialize("http://localhost/", "http://localhost/");
    protected override void NavigateToCore(string uri, bool forceLoad) { }
}
