using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class SpanOperations
{
#if(NET7_0)
    private int[] _zeros = new int[4096];

    [Benchmark(Baseline = true)]
    public bool OpenCoded()
    {
        foreach (var b in _zeros)
        {
            if (b != 0)
            {
                return false;
            }
        }

        return true;
    }

    [Benchmark]
    public bool IndexOfAnyExcept() => _zeros.AsSpan().IndexOfAnyExcept(0) < 0;
#endif
}