using System.Buffers;
using System.Globalization;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class PrimitiveTypes
{
    private string[] _valuesToParse;

    [GlobalSetup]
    public void Setup()
    {
        using var hc = new HttpClient();
        var text = hc.GetStringAsync("https://raw.githubusercontent.com/CarlVerret/csFastFloat/1d800237275f759b743b86fcce6680d072c1e834/Benchmark/data/canada.txt").Result;
        var lines = new List<string>();
        foreach (var line in text.AsSpan().EnumerateLines())
        {
            var trimmed = line.Trim();
            if (!trimmed.IsEmpty)
            {
                lines.Add(trimmed.ToString());
            }
        }
        _valuesToParse = lines.ToArray();
    }

    [Benchmark]
    public double ParseAll()
    {
        double total = 0;
        foreach (var s in _valuesToParse)
        {
            total += double.Parse(s, CultureInfo.InvariantCulture);
        }
        return total;
    }
    
    private Guid _guid1 = Guid.Parse("0aa2511d-251a-4764-b374-4b5e259b6d9a");
    private Guid _guid2 = Guid.Parse("0aa2511d-251a-4764-b374-4b5e259b6d9a");

    [Benchmark]
    public bool GuidEquals() => _guid1 == _guid2;
}