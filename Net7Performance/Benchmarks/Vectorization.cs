using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

/// <summary>
/// Run separately for NET7.0
/// </summary>
public class Vectorization
{
    [Params(29, 60, 250, 999)]
    public int Size { get; set; }
    
    private byte[] _data;

    [GlobalSetup]
    public void Setup()
    {
        _data = Enumerable.Repeat((byte)123, Size).Append((byte)42).ToArray();
    }
    
    [Benchmark(Baseline = true)]
    public bool Find() => Contains(_data, 42);

    static bool Contains(ReadOnlySpan<byte> haystack, byte needle)
    {
        for (var i = 0; i < haystack.Length; i++)
        {
            if (haystack[i] == needle)
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool FindVectorizedGeneric() => ContainsVectorizedGeneric(_data, 42);
    static bool ContainsVectorizedGeneric(ReadOnlySpan<byte> haystack, byte needle)
    {
        if (Vector.IsHardwareAccelerated && haystack.Length >= Vector<byte>.Count)
        {
            var target = new Vector<byte>(needle);
            ref var current = ref MemoryMarshal.GetReference(haystack);
            ref var endMinusOneVector = ref Unsafe.Add(ref current, haystack.Length - Vector<byte>.Count);
            do
            {
                if (Vector.EqualsAny(target, Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.As<byte, byte>(ref current))))
                {
                    return true;
                }

                current = ref Unsafe.Add(ref current, Vector<byte>.Count);
            }
            while (Unsafe.IsAddressLessThan(ref current, ref endMinusOneVector));

            return Vector.EqualsAny(target, Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.As<byte, byte>(ref endMinusOneVector)));
        }

        return Contains(haystack, needle);
    }
    
#if NET7_0
    [Benchmark]
    public bool FindVectorized() => ContainsVectorized(_data, 42);
    
    static bool ContainsVectorized(ReadOnlySpan<byte> haystack, byte needle)
    {
        if (!Vector128.IsHardwareAccelerated || haystack.Length < Vector128<byte>.Count)
        {
            return Contains(haystack, needle);
        }

        ref var current = ref MemoryMarshal.GetReference(haystack);

        if (Vector256.IsHardwareAccelerated && haystack.Length >= Vector256<byte>.Count)
        {
            var target = Vector256.Create(needle);
            ref var endMinusOneVector = ref Unsafe.Add(ref current, haystack.Length - Vector256<byte>.Count);
            do
            {
                if (Vector256.EqualsAny(target, Vector256.LoadUnsafe(ref current)))
                {
                    return true;
                }

                current = ref Unsafe.Add(ref current, Vector256<byte>.Count);
            } while (Unsafe.IsAddressLessThan(ref current, ref endMinusOneVector));

            if (Vector256.EqualsAny(target, Vector256.LoadUnsafe(ref endMinusOneVector)))
            {
                return true;
            }
        }
        else
        {
            var target = Vector128.Create(needle);
            ref var endMinusOneVector = ref Unsafe.Add(ref current, haystack.Length - Vector128<byte>.Count);
            do
            {
                if (Vector128.EqualsAny(target, Vector128.LoadUnsafe(ref current)))
                {
                    return true;
                }

                current = ref Unsafe.Add(ref current, Vector128<byte>.Count);
            } while (Unsafe.IsAddressLessThan(ref current, ref endMinusOneVector));

            if (Vector128.EqualsAny(target, Vector128.LoadUnsafe(ref endMinusOneVector)))
            {
                return true;
            }
        }

        return false;
    }
#endif
}
