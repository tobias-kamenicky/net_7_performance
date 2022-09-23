using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class LinqTypeCheck
{
    private IEnumerable<int> _source = (int[])(object)new uint[42];

    [Benchmark]
    public bool WithIs() => _source is int[];

    [Benchmark]
    public bool WithTypeCheck() => _source.GetType() == typeof(int[]);
}