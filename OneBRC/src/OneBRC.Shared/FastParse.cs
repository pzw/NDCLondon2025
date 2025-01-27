using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBRC.Shared;

public static class FastParse
{
    public static int FloatAsInt(ReadOnlySpan<byte> span)
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
