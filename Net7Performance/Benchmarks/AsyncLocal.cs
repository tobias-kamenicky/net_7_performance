using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class AsyncLocal
{
    private AsyncLocal<int> asyncLocal1 = new();
    private AsyncLocal<int> asyncLocal2 = new();
    private AsyncLocal<int> asyncLocal3 = new();
    private AsyncLocal<int> asyncLocal4 = new();

    [Benchmark(OperationsPerInvoke = 4000)]
    public void Update()
    {
        for (var i = 0; i < 1000; i++)
        {
            asyncLocal1.Value++;
            asyncLocal2.Value++;
            asyncLocal3.Value++;
            asyncLocal4.Value++;
        }
    }
}