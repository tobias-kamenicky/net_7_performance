using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;

namespace Net7Performance.Benchmarks;

public class AsyncEnumerableParallel
{
    [Params(256)]
    public int Size { get; set; }

    private IAsyncEnumerable<int> _items;
    
    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(123);
        _items = Enumerable.Range(0, Size).Select(_ => random.Next()).ToAsyncEnumerable();
    }

    [Benchmark]
    public async Task SelectWhenAll()
    {
        await _items.SelectAwait(async x =>
        {
            await Task.Delay(1);
            return x;
        }).ToListAsync();
    }
    
    [Benchmark]
    public async Task Custom()
    {
        await _items.ParallelForEach(async x =>
        {
            await Task.Delay(1);
        }, 10);
    }

    // [Benchmark]
    // public async Task Parallel_ForEach()
    // {
    //     await Parallel.ForEachAsync(_items, async (x, ct) =>
    //     {
    //         await Task.Delay(1);
    //         return x;
    //     });
    // }
}

public static class AsyncExtensions 
{
    public static async Task ParallelForEach<T>(this IAsyncEnumerable<T> collection, Func<T, Task> doWork, int maxParallelism = 50) where T : notnull
    {
        var tasks = new List<Task>();
        await foreach (var item in collection)
        {
            tasks.Add(doWork(item));

            tasks.RemoveAll(x => x.IsCompleted || x.IsCanceled || x.IsFaulted);
            if (tasks.Count > maxParallelism)
            {
                await Task.WhenAny(tasks);
            }
        }

        await Task.WhenAll(tasks);
    }
}