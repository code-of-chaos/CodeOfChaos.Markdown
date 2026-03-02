// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
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
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdown();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownEditors();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownParsers();
        
        serviceCollection.AddSingleton(TextEditorFactory.CreateTextEditor);
        serviceCollection.AddSingleton(MdStringMdSyntaxDeserializerFactory.CreateDeserializer);
        
        
        var markdownConfig = new CodeOfChaosMarkdownConfig(serviceCollection);
        markdownConfig.AddDefaultNodeVisitors();
        
        configure?.Invoke(markdownConfig);
        
        return serviceCollection;
    }
}

