using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Folding
{
    [Benchmark]
    public TimeSpan FromSeconds() => TimeSpan.FromSeconds(5);
}