using BenchmarkDotNet.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBRC.Benchmarks;

public class RangeVsSlice
{
    private static readonly ReadOnlyMemory<byte> Utf8Line = "London;1.111"u8.ToArray();

    [Benchmark(Baseline = true)]
    public int Slice()
    {
        var span = Utf8Line.Span;
        var index = span.IndexOf((byte)';');
        return span.Slice(index).Length;
    }

    public int Range()
    {
        var span = Utf8Line.Span;
        var index = span.IndexOf((byte)';');
        return span[index..].Length;
    }

}
