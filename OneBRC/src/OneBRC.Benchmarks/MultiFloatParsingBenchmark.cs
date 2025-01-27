using BenchmarkDotNet.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBRC.Benchmarks;

public class MultiFloatParsingBenchmark
{
    private const byte Semicolon = (byte)';';
    private const byte NewLine = (byte)'\n';


    private static readonly string Floats = Enumerable.Range(0, 1000)
      .Select(_ => (Random.Shared.NextSingle() * 10).ToString("0.0000"))
        .Aggregate((a, b) => a + "\n" + b);

    private static readonly ReadOnlyMemory<byte> Utf8FLoats = Encoding.UTF8.GetBytes(Floats);


    [Benchmark(Baseline = true)]
    public float SumAsFloats()
    {
        var sum = 0f;
        var span = Utf8FLoats.Span;
        foreach (var range in span.Split(NewLine))
        {
            var line = span[range];
            sum += float.Parse(line);
        }
        return sum;
    }

    [Benchmark]
    public float SumAsInts()
    {
        // on sait que tous nos floats ont 3 décimales dans le string, donc additionne des entiers et on convertit en float à la fin
        var sum = 0;
        var span = Utf8FLoats.Span;
        foreach (var range in span.Split(NewLine))
        {
            var line = span[range];
            sum += ParseAsInt(line);
        }
        return sum / 1000f;
    }

    private static int ParseAsInt(ReadOnlySpan<byte> span)
    {
        // assume que le nombre est positif
        var value = 0;
        for (; span.Length > 0; span = span.Slice(1))
        {
            if (span[0] == (byte)'.') continue;
            value = value * 10 + (span[0] - (byte)'0');
        }
        return value;
    }
}
