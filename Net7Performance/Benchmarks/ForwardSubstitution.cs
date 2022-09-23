using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class ForwardSubstitution
{
    [Benchmark]
    public int Compute1() => Value + Value + Value + Value + Value;

    [Benchmark]
    public int Compute2() => SomethingElse() + Value + Value + Value + Value + Value;

    private static int Value => 16;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int SomethingElse() => 42;
}