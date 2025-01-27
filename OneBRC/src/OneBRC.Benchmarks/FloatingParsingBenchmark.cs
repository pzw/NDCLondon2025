using BenchmarkDotNet.Attributes;

using Microsoft.CodeAnalysis.FlowAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBRC.Benchmarks;

[MemoryDiagnoser]
public class FloatingParsingBenchmark
{
    private const byte Semicolon = (byte)';';
    private const byte NewLine = (byte)'\n';

    private static readonly string Line = "London;1.111";
    private static readonly byte[] Utf8Line = "London;1.111"u8.ToArray();
    private static readonly byte[] Lines = "London;1.11\nParis;0.222\nNew-York;555.555\n"u8.ToArray();

    private static readonly string Floats = Enumerable.Range(0, 1000)
      .Select(_ => Random.Shared.NextSingle().ToString("0.0000"))
        .Aggregate((a, b) => a + "\n" + b);

    private static readonly ReadOnlyMemory<byte> Utf8FLoats = Encoding.UTF8.GetBytes(Floats);

    //[Benchmark(Baseline = true)]
    public float Naive()
    {
        return float.Parse(Line.Split(';')[1]);
    }

   //[Benchmark]
    public float FewerAllocation()
    {
        var split = Line.IndexOf(';');
        var value = Line.Substring(split + 1);
        return float.Parse(value);
    }

    [Benchmark]
    public float ZeroAllocation()
    {
        var span = Line.AsSpan();
        var split = span.IndexOf(';');
        // pas d'allocation
        span = span.Slice(split + 1);
        // parse d'un span : pas d'allocation
        return float.Parse(span);
    }

    [Benchmark]
    public float Utf8ZeroAllocation()
    {
        var span = Utf8Line.AsSpan();
        var split = span.IndexOf(Semicolon);
        span = span.Slice(split + 1);
        return float.Parse(span);
    }

}
