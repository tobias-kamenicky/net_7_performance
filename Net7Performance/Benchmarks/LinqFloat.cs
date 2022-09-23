using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class LinqFloat
{
    private static float[] CreateRandom()
    {
        var r = new Random(42);
        var results = new float[10_000];
        for (var i = 0; i < results.Length; i++)
        {
            results[i] = (float)r.NextDouble();
        }
        return results;
    }

    private IEnumerable<float> _floats = CreateRandom();

    [Benchmark]
    public float Sum() => _floats.Sum();

    [Benchmark]
    public float Average() => _floats.Average();

    [Benchmark]
    public float Min() => _floats.Min();

    [Benchmark]
    public float Max() => _floats.Max();
}