using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class OrderBy
{
#if NET7_0
    [GlobalSetup]
    public void Setup() => _arr = Enumerable.Range(1, 4096).Reverse().ToArray();

    private int[] _arr;
    
    [Benchmark(Baseline = true)]
    public void OrderBy_Lambda()
    {
        foreach (var _ in _arr.OrderBy(x => x)) { }
    }
    
    [Benchmark]
    public void Order()
    {
        foreach (var _ in _arr.Order()) { }
    }
#endif
}