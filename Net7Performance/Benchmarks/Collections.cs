using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Collections
{
    // No real change between .NET versions but some of the lower level improvements affect these
    private Dictionary<int, int> _dictionary = Enumerable.Range(0, 10_000).ToDictionary(i => i);

    [Benchmark]
    public int Sum()
    {
        var dictionary = _dictionary;
        var sum = 0;

        for (var i = 0; i < 10_000; i++)
        {
            if (dictionary.TryGetValue(i, out var value))
            {
                sum += value;
            }
        }

        return sum;
    }
}