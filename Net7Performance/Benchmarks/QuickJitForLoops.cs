using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class QuickJitForLoops
{
    static readonly bool _is64Bit = Environment.Is64BitProcess;

    [Benchmark]
    public int ReadonlyStaticVars()
    {
        var count = 0;
        for (var i = 0; i < 1_000_000_000; i++)
            if (Test()) count++;
        return count;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    static bool Test() => _is64Bit;
}