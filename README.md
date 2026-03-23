# CodeOfChaos.Markdown

[![NuGet](https://img.shields.io/nuget/v/CodeOfChaos.Markdown.svg)](https://www.nuget.org/packages/CodeOfChaos.Markdown/)
[![License](https://img.shields.io/github/license/code-of-chaos/Markdown)](LICENSE)

A comprehensive .NET library for parsing, editing, and rendering Markdown content with support for syntax trees, multiple output formats, and interactive Blazor components.

## Overview

CodeOfChaos.Markdown provides a modular, Markdown processing pipeline for .NET applications. It features:

- **Syntax Tree Representation**: Parse Markdown into an abstract syntax tree (AST) for programmatic manipulation
- **Multiple Output Formats**: Serialize to HTML, XML, JSON, or back to Markdown
- **Interactive Editing**: Text editing capabilities with modifier support for real-time Markdown manipulation
- **Blazor Integration**: Render Markdown as Blazor components for interactive web applications
- **Object Pooling**: Memory-efficient implementation using pooling patterns for high-performance scenarios
- **Extensible Architecture**: Plugin-based design for custom node types, parsers, and renderers

## Key Features

### Parsing & Syntax Trees
- Parse Markdown text into structured syntax trees
- Support for standard Markdown and extended syntax (callouts, footnotes, WikiLinks, etc.)
- Bidirectional conversion: Markdown ↔ Syntax Tree ↔ HTML/XML/JSON
- Node-based architecture for tree traversal and manipulation

### Text Editing
- Real-time text modification with built-in modifiers (bold, italic, strikethrough, etc.)
- Custom modifier support for domain-specific formatting
- Caret management and line-based operations
- Special structure handling (lists, tables, blockquotes)

### Blazor Components
- Direct rendering of Markdown syntax trees as Blazor components
- Interactive editor context with event handling
- Customizable component mapping for syntax nodes
- Support for custom data providers (emotes, tags, users, templates)

## Installation

Install the main package via NuGet:

```bash
dotnet add package CodeOfChaos.Markdown
```

For specific functionality, install individual packages:

```bash
# Core parsing and syntax tree support
dotnet add package CodeOfChaos.Markdown.Syntax

# Text editing capabilities
dotnet add package CodeOfChaos.Markdown.Editors

# Parser implementations (HTML, XML, JSON, Markdown)
dotnet add package CodeOfChaos.Markdown.Parsers

# Shared abstractions
dotnet add package CodeOfChaos.Markdown.Shared
```

## Quick Start

### Basic Parsing

```csharp
using CodeOfChaos.Markdown;
using Microsoft.Extensions.DependencyInjection;

// Configure services
var services = new ServiceCollection();
services.AddMarkdownParser(); // Extension method from the library
var provider = services.BuildServiceProvider();

// Get parser instance
var parser = provider.GetRequiredService<IMarkdownParser>();

// Parse Markdown to syntax tree
string markdown = "# Hello World\n\nThis is **bold** text.";
var syntaxTree = parser.Markdown.SerializeToSyntaxTree(markdown);

// Convert to HTML
string html = parser.Html.DeserializeFromSyntaxTree(syntaxTree);

// Convert to JSON
string json = parser.Json.DeserializeFromSyntaxTree(syntaxTree);
```

### Text Editing

```csharp
using CodeOfChaos.Markdown.Editors;
using CodeOfChaos.Markdown.TextEditor;

// Create text source and editor
var source = new TextSource("Hello World");
var editor = TextEditorFactory.CreateTextEditor(serviceProvider);

// Apply bold modifier to entire text
editor.Modify(source, "bold", new Range(0, source.Length));

// Result: "**Hello World**"
Console.WriteLine(source.Text);
```

### Blazor Rendering

```razor
@using CodeOfChaos.Markdown
@using CodeOfChaos.Markdown.Parsers.Blazor
@inject IMarkdownParser Parser
@inject IBlazorMdComponentRenderer Renderer

<div class="markdown-content">
    @Renderer.RenderRootComponents(syntaxTree.VisitTopLevelNodes())
</div>

@code {
    private IMdSyntaxTree syntaxTree = MdSyntaxTree.Empty;

    protected override void OnInitialized()
    {
        var markdown = "# Welcome\n\nThis is **rendered** as Blazor components.";
        syntaxTree = Parser.Markdown.SerializeToSyntaxTree(markdown);
    }
}
```

## Architecture Overview

### Package Structure

```
CodeOfChaos.Markdown/
├── CodeOfChaos.Markdown.Shared      # Core interfaces and abstractions
├── CodeOfChaos.Markdown.Syntax      # Syntax tree implementation and node types
├── CodeOfChaos.Markdown.Editors     # Text editing and modification logic
├── CodeOfChaos.Markdown.Parsers     # Format-specific parsers (HTML, XML, JSON, Blazor)
└── CodeOfChaos.Markdown             # Main library with configuration and DI setup
```

### Core Concepts

**Syntax Tree (`IMdSyntaxTree`)**
- Root container for parsed Markdown structure
- Provides traversal methods (breadth-first, depth-first)
- Supports caching and type-based node queries

**Syntax Nodes (`IMdSyntaxNode`)**
- Individual elements in the syntax tree (headings, paragraphs, bold text, etc.)
- Hierarchical structure with parent-child relationships
- Pooled for memory efficiency

**Text Editor (`ITextEditor`)**
- Applies modifications to text sources
- Manages text modifiers and caret position
- Handles special structures like lists and tables

**Parsers**
- `IMarkdownMdSyntaxTreeParser`: Markdown ↔ Syntax Tree
- `IHtmlMdSyntaxTreeParser`: HTML ↔ Syntax Tree
- `IXmlMdSyntaxTreeParser`: XML ↔ Syntax Tree
- `IJsonMdSyntaxTreeParser`: JSON ↔ Syntax Tree

## Advanced Usage

### Custom Text Modifiers

```csharp
public class CustomModifier : ITextModifier
{
    public string ModifierName => "custom";
    public bool IsSingleLineStructure => true;

    public void Modify(ITextSource source, Range range, ITextEditor editor)
    {
        // Custom modification logic
        int start = range.Start.GetOffset(source.Length);
        int end = range.End.GetOffset(source.Length);

        string modified = $"[CUSTOM]{source.Text[start..end]}[/CUSTOM]";
        editor.Insert(source, modified, range);
    }
}

// Register in DI
services.AddSingleton<ITextModifier, CustomModifier>();
```

### Syntax Tree Traversal

```csharp
// Visit all nodes breadth-first
foreach (var node in syntaxTree.VisitNodesBreadthFirst())
{
    Console.WriteLine($"{node.Type.Name}: {node.ToDebugString()}");
}

// Get all heading nodes
if (syntaxTree.TryGetCachedChildrenByType<HeadingMdSyntaxNode>(out var headings))
{
    foreach (var heading in headings)
    {
        Console.WriteLine($"Level {heading.Level}: {heading.Content}");
    }
}
```

### Configuration

```csharp
services.AddMarkdownParser(config =>
{
    // Configure which Blazor components to skip rendering
    config.SkipBlazorComponent<NewLineMdSyntaxNode>();

    // Enable/disable rendering unknown components
    config.RenderUnknownBlazorComponents = false;

    // Add custom node type mappings
    config.AddBlazorComponent<CustomMdSyntaxNode, CustomBlazorComponent>();
});
```

## Supported Markdown Features

- **Standard**: Headings, paragraphs, bold, italic, links, images, lists, code blocks
- **Extended**: Strikethrough, subscript, superscript, highlight, underline
- **Custom**: Callouts, footnotes, WikiLinks, front matter, HTML spans, emotes, tags, user mentions
- **Templating**: Conditional blocks, literal statements, template expressions

## Documentation

Full documentation is available at the [project documentation site](https://code-of-chaos.github.io/Markdown/).

- [Getting Started Guide](https://code-of-chaos.github.io/Markdown/articles/getting-started.html)
- [API Reference](https://code-of-chaos.github.io/Markdown/api/index.html)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Requirements

- .NET 10 SDK or later
- IDE with C# support (Visual Studio, Rider, or VS Code)

### Building the Project

```bash
git clone https://github.com/code-of-chaos/Markdown.git
cd Markdown
dotnet restore
dotnet build
dotnet test
```

### Running Tests

```bash
dotnet test --configuration Release
```

## License

This project is licensed under the terms specified in the [LICENSE](LICENSE) file.

## Acknowledgments

- Built with modern .NET performance patterns (Span<T>, ArrayPool, object pooling)
- Inspired by CommonMark and extended Markdown specifications
- Designed for extensibility and high-performance scenarios
