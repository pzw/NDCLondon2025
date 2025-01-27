using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace OneBRC.Benchmarks;

public partial class CityBenchmark
{
    public readonly struct CityKey : IEquatable<CityKey>
    {
        private readonly ulong _simple;
        private readonly Vector512<ulong> _vector;

        public CityKey(ulong simple)
        {
            _simple = simple;
            _vector = default;
        }

        public CityKey(Vector512<ulong> vector)
        {
            _simple = 0;
            _vector = vector;
        }

        public bool Equals(CityKey other)
        {
            if (_simple != 0)
            {
                return _simple == other._simple;
            }
            return _vector.Equals(other._vector);
        }

        public override bool Equals(object obj)
        {
            return obj is CityKey other && Equals(other);
        }

        public static CityKey Create(ReadOnlySpan<byte> span)
        {
            if (span.Length == 8)
            {
                return new CityKey(MemoryMarshal.Read<ulong>(span));
            }

            if (span.Length < 8)
            {
                // allocation sur le stack d'un buffer de 8 bytes
                Span<byte> buffer = stackalloc byte[8];
                span.CopyTo(buffer);
                return new CityKey(MemoryMarshal.Read<ulong>(buffer));
            }
            {
                Span<byte> buffer = stackalloc byte[64];
                // on ne copie que les 64 premiers bytes
                //span.Slice(0, 64).CopyTo(buffer);
                span.CopyTo(buffer);
                Span<ulong> vector = MemoryMarshal.Cast<byte, ulong>(buffer);
                return new CityKey(Vector512.Create<ulong>(vector));
                // return new CityKey(vector);
            }
        }
    }
}
