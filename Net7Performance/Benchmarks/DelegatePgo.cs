using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

/// <summary>
/// before and after DOTNET_TieredPGO=1
/// </summary>
public class DelegatePgo
{
    static readonly int[] _values = Enumerable.Range(0, 1_000).ToArray();

    [Benchmark]
    public int SumValues() => Sum(_values, i => i * 42);

    private static int Sum(int[] values, Func<int, int>? func)
    {
        var sum = 0;
        foreach (var value in values)
        {
            sum += func(value);
        }
        return sum;
    }
}