// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Markdown.Config;
using CodeOfChaos.Markdown.Parsers.Markdown;
using CodeOfChaos.Markdown.Parsers.Markdown.Serializer;
using Microsoft.Extensions.Logging;
using System.Buffers;
using System.Collections.Immutable;

namespace CodeOfChaos.Markdown.Parsers.Langs.Markdown.Serializer;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<IMdStringMdSyntaxSerializerFactory>]
public class MdStringMdSyntaxSerializerFactory(ILogger<MdStringMdSyntaxSerializer> logger) : IMdStringMdSyntaxSerializerFactory {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public IMdStringMdSyntaxSerializer Create(IMarkdownConfig config) {
        // ReSharper disable twice UseCollectionExpression
        ImmutableArray<IMarkdownSyntaxNodeVisitor> singleLineSerializers = config.SingleLineMarkdownSyntaxNodeVisitors;
        ImmutableArray<IMarkdownSyntaxNodeVisitor> multiLineSerializers = config.MultiLineMarkdownSyntaxNodeVisitors;
        IMarkdownSyntaxNodeVisitor? frontMatterSerializer = config.FrontMatterMarkdownSyntaxNodeVisitor;

        (ImmutableArray<IMarkdownSyntaxNodeVisitor>[] singleAscii, ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> singleNonAscii) = BuildLookup(singleLineSerializers);
        (ImmutableArray<IMarkdownSyntaxNodeVisitor>[] multiAscii, ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> multiNonAscii) = BuildLookup(multiLineSerializers);

        Span<bool> seen = stackalloc bool[256];
        Span<char> buffer = stackalloc char[256];
        HashSet<char>? nonAscii = null;
        int count = 0;

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (IMarkdownSyntaxNodeVisitor serializer in singleLineSerializers) {
            foreach (char ch in serializer.SerializationTriggerCharacters) {
                if (ch >= 256) {
                    nonAscii ??= new HashSet<char>();
                    nonAscii.Add(ch);
                    continue;
                }
                
                if (seen[ch]) continue;

                seen[ch] = true;
                buffer[count++] = ch;
            }
        }

        ReadOnlySpan<char> bufferView = buffer[..count];
        
        SearchValues<char> singleLineTriggerSearchValues = nonAscii is null
            ? SearchValues.Create(bufferView)
            : SearchValues.Create(BuildAllArray(ref bufferView, nonAscii));

        return new MdStringMdSyntaxSerializer(logger) {
            SingleLineSerializers = singleLineSerializers,
            SingleLineLookup = singleAscii,
            SingleLineNonAsciiLookup = singleNonAscii,
            SingleLineTriggerSearchValues = singleLineTriggerSearchValues,

            MultiLineSerializers = multiLineSerializers,
            MultiLineLookup = multiAscii,
            MultiLineNonAsciiLookup = multiNonAscii,

            FrontMatterSerializer = frontMatterSerializer,
        };
    }

    private static (ImmutableArray<IMarkdownSyntaxNodeVisitor>[] ascii,
        ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>> nonAscii)
        BuildLookup(ImmutableArray<IMarkdownSyntaxNodeVisitor> serializers) {
        var buckets = new List<IMarkdownSyntaxNodeVisitor>?[256];
        var nonAsciiBuckets = new Dictionary<char, List<IMarkdownSyntaxNodeVisitor>>();
        var globals = new List<IMarkdownSyntaxNodeVisitor>();

        Span<bool> localSeen = stackalloc bool[256];
        HashSet<char>? localNonAscii = null;

        foreach (IMarkdownSyntaxNodeVisitor s in serializers) {
            ReadOnlySpan<char> triggers = s.SerializationTriggerCharacters;
            if (triggers.IsEmpty) {
                globals.Add(s);
                continue;
            }

            localSeen.Clear();
            localNonAscii?.Clear();

            foreach (char ch in triggers) {
                if (ch < 256) {
                    if (localSeen[ch]) continue;
                    localSeen[ch] = true;

                    List<IMarkdownSyntaxNodeVisitor>? list = buckets[ch];
                    if (list is null) buckets[ch] = list = new List<IMarkdownSyntaxNodeVisitor>();
                    list.Add(s);
                }
                else {
                    localNonAscii ??= new HashSet<char>();
                    if (!localNonAscii.Add(ch)) continue;

                    if (!nonAsciiBuckets.TryGetValue(ch, out var list)) {
                        list = new List<IMarkdownSyntaxNodeVisitor>();
                        nonAsciiBuckets[ch] = list;
                    }
                    list.Add(s);
                }
            }
        }

        var table = new ImmutableArray<IMarkdownSyntaxNodeVisitor>[256];
        for (int c = 0; c < 256; c++) {
            List<IMarkdownSyntaxNodeVisitor>? list = buckets[c];
            int total = (list?.Count ?? 0) + globals.Count;

            if (total == 0) {
                table[c] = ImmutableArray<IMarkdownSyntaxNodeVisitor>.Empty;
                continue;
            }

            var arr = new IMarkdownSyntaxNodeVisitor[total];
            int offset = 0;

            if (list is not null) {
                list.CopyTo(arr, 0);
                offset = list.Count;
            }

            if (globals.Count > 0) {
                globals.CopyTo(arr, offset);
            }

            table[c] = ImmutableArray.Create(arr);
        }

        ImmutableDictionary<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>>.Builder nonAscii = ImmutableDictionary.CreateBuilder<char, ImmutableArray<IMarkdownSyntaxNodeVisitor>>();
        foreach ((char ch, List<IMarkdownSyntaxNodeVisitor> list) in nonAsciiBuckets) {
            int total = list.Count + globals.Count;
            var arr = new IMarkdownSyntaxNodeVisitor[total];
            list.CopyTo(arr, 0);
            if (globals.Count > 0) globals.CopyTo(arr, list.Count);
            nonAscii[ch] = ImmutableArray.Create(arr);
        }

        return (table, nonAscii.ToImmutable());
    }

    private static char[] BuildAllArray(ref ReadOnlySpan<char> ascii, HashSet<char> nonAscii) {
        char[] all = new char[ascii.Length + nonAscii.Count];
        ascii.CopyTo(all);
        int i = ascii.Length;
        foreach (char ch in nonAscii) all[i++] = ch;
        return all;
    }
}
