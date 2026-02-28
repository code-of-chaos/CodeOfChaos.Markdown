// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    
    // Used for the standalone version of the library
    [UsedImplicitly]
    public static IServiceCollection AddCodeOfChaosMarkdown(this IServiceCollection serviceCollection, Action<CodeOfChaosMarkdownConfig>? configure = null) {
        var markdownConfig = new CodeOfChaosMarkdownConfig(serviceCollection);
        
        configure?.Invoke(markdownConfig);
        
        return serviceCollection;
    }
}

