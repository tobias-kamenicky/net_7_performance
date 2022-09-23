using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class ListEnumeration
{
    [Params(1024)]
    public int Size { get; set; }

    private List<int> _items;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(93);
        _items = Enumerable.Range(0, Size).Select(x => random.Next()).ToList();
    }

    [Benchmark]
    public void Foreach()
    {
        foreach (var item in _items) { }
    }
    
    [Benchmark]
    public void For()
    {
        for (var i = 0; i < _items.Count; i++) { var item = _items[i]; }
    }
    
    [Benchmark]
    public void Foreach_Linq() => _items.ForEach(i => { });

    [Benchmark]
    public void Parallel_ForEach() => Parallel.ForEach(_items, i => { });

    [Benchmark]
    public void AsParallel_ForAll() => _items.AsParallel().ForAll(i => { });

    [Benchmark]
    public void Foreach_Span()
    {
        foreach (var item in CollectionsMarshal.AsSpan(_items)) { }
    }
}