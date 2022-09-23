using System.Buffers;
using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class DateTimeBenchmark
{
    private char[] _dest = new char[100];
    
    private DateTime _dt = DateTime.UtcNow;

    [Benchmark] public int Day() => _dt.Day;
    [Benchmark] public int Month() => _dt.Month;
    [Benchmark] public int Year() => _dt.Year;
    [Benchmark] public bool TryFormat() => _dt.TryFormat(_dest, out _, "r");
}