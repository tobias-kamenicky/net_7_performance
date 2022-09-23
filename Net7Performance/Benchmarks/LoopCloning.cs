using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class LoopCloning
{
    private readonly int[] _values = Enumerable.Range(0, 1000).ToArray();

    [Benchmark]
    [Arguments(0, 0, 1000)]
    public int LastIndexOf(int arg, int offset, int count)
    {
        var values = _values;
        for (var i = offset + count - 1; i >= offset; i--)
            if (values[i] == arg)
                return i;
        return 0;
    }
}