using BenchmarkDotNet.Attributes;

using CommunityToolkit.HighPerformance.Buffers;

using Microsoft.Diagnostics.Tracing.Parsers.IIS_Trace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OneBRC.Benchmarks;
public partial class CityBenchmark
{
    private static readonly ReadOnlyMemory<byte> ThousandTxt = GetThousandTxt();
    private StringPool _pool = new();

    [Benchmark]
    public int CountAsString()
    {
        var dict = new Dictionary<string, int>();
        var span = ThousandTxt.Span;
        foreach(var range in span.Split((byte) '\n'))
        {
            var line = span[range];
            var semicolon = line.IndexOf((byte)';');
            var utf8City = line.Slice(0, semicolon);
            var city =  Encoding.UTF8.GetString(utf8City);
            dict[city] = dict.TryGetValue(city, out var count) ? count + 1 : 1;
        }
        return dict.Count;
    }

    [Benchmark]
    public int CountAsPooledString()
    {
        var dict = new Dictionary<string, int>();
        var span = ThousandTxt.Span;
        foreach (var range in span.Split((byte)'\n'))
        {
            var line = span[range];
            var semicolon = line.IndexOf((byte)';');
            var utf8City = line.Slice(0, semicolon);
            var city = _pool.GetOrAdd(utf8City, Encoding.UTF8);
            dict[city] = dict.TryGetValue(city, out var count) ? count + 1 : 1;
        }
        return dict.Count;
    }


    [Benchmark]
    public int CountAsKeys()
    {
        var dict = new Dictionary<CityKey, int>();
        var span = ThousandTxt.Span;
        foreach (var range in span.Split((byte)'\n'))
        {
            var line = span[range];
            var semicolon = line.IndexOf((byte)';');
            var utf8City = line.Slice(0, semicolon);
            var key = CityKey.Create(utf8City);
            dict[key] = dict.TryGetValue(key, out var count) ? count + 1 : 1;
        }
        return dict.Count;
    }

    [Benchmark]
    public int CountAsHashSet()
    {
        var set = new HashSet<string>();
        var span = ThousandTxt.Span;
        foreach (var range in span.Split((byte)'\n'))
        {
            var line = span[range];
            var semicolon = line.IndexOf((byte)';');
            var utf8City = line.Slice(0, semicolon);
            var city = Encoding.UTF8.GetString(utf8City);
            set.Add(city);
        }
        return set.Count;
    }

    public static byte[] GetThousandTxt()
    {
        // pour que ça fonctionne, thousand.txt doit être considéré comme un embedded resource
        using var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("OneBRC.Benchmarks.thousand.txt");
        var buffer = new byte[s.Length];
        s.ReadExactly(buffer);
        return buffer;
    }
}
