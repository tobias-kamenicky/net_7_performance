using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Inlining
{
    private bool _value = true;

    [Benchmark]
    public int BoolStringLength() => _value.ToString().Length;
}