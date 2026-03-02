// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.Parsers.Langs.Markdown.Deserializer;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CodeOfChaos.Markdown;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ServiceCollectionExtensions {
    
    // Used for the standalone version of the library
    [UsedImplicitly]
    public static IServiceCollection AddCodeOfChaosMarkdown(this IServiceCollection serviceCollection, Action<MarkdownConfig>? configure = null) {
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdown();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownEditors();
        serviceCollection.RegisterServicesFromCodeOfChaosMarkdownParsers();
        
        serviceCollection.AddSingleton(TextEditorFactory.CreateTextEditor);
        serviceCollection.AddSingleton(MdStringMdSyntaxDeserializerFactory.CreateDeserializer);
        
        serviceCollection.AddSingleton<IMdStringMdSyntaxSerializer>(static sp => {
            var factory = sp.GetRequiredService<IMdStringMdSyntaxSerializerFactory>();
            var config = sp.GetRequiredService<IMarkdownConfig>();
            return factory.Create(config);
        });
        
        var markdownConfig = new MarkdownConfig();
        markdownConfig.AddDefaultNodeVisitors();
        
        configure?.Invoke(markdownConfig);
        
        serviceCollection.AddSingleton(ImmutableMarkdownConfig.From(markdownConfig));
        
        return serviceCollection;
    }
}

