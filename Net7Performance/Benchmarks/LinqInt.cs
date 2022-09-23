using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class LinqInt
{
    [Params(4, 1024)]
    public int Length { get; set; }
    
    [GlobalSetup]
    public void Setup() => _source = Enumerable.Range(1, Length).ToArray();
    
    private IEnumerable<int> _source;

    [Benchmark]
    public int Sum() => _source.Sum();

    [Benchmark]
    public int Min() => _source.Min();

    [Benchmark]
    public int Max() => _source.Max();
}