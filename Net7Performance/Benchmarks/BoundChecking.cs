using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

/// <summary>
/// before and after DOTNET_TieredPGO=1
/// </summary>
public class BoundChecking
{
    private int[,] _square;

    [Params(1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var count = 0;
        _square = new int[Size, Size];
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                _square[i, j] = count++;
            }
        }
    }
    
    [Benchmark]
    public int Sum_KnownBounds()
    {
        var square = _square;
        var sum = 0;
        for (var i = 0; i < Size; i++)
        {
            for (var j = 0; j < Size; j++)
            {
                sum += square[i, j];
            }
        }
        return sum;
    }

    [Benchmark]
    public int Sum_UnknownBounds()
    {
        var square = _square;
        var sum = 0;
        for (var i = square.GetLowerBound(0); i < square.GetUpperBound(0); i++)
        {
            for (var j = square.GetLowerBound(1); j < square.GetUpperBound(1); j++)
            {
                sum += square[i, j];
            }
        }
        return sum;
    }
}