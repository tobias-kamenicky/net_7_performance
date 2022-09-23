using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class ThreadPool
{
    private byte[][] _nestedArrays = new byte[8][];
    private const int Iterations = 100_000;

    private static long IterateAll(byte[] arr)
    {
        long ret = default;
        foreach (var item in arr) ret += item;
        return ret;
    }

    [GlobalSetup]
    public void Setup()
    {
        _nestedArrays = new byte[8][];
    }
    
    [Benchmark(OperationsPerInvoke = Iterations)]
    public async Task MultipleSerial()
    {
        for (var i = 0; i < Iterations; i++)
        {
            for (var j = 0; j < _nestedArrays.Length; j++)
            {
                _nestedArrays[j] = ArrayPool<byte>.Shared.Rent(4096);
                _nestedArrays[j].AsSpan().Clear();
            }

            // Queue to ThreadPool
            await Task.Yield();

            for (var j = _nestedArrays.Length - 1; j >= 0; j--)
            {
                IterateAll(_nestedArrays[j]);
                ArrayPool<byte>.Shared.Return(_nestedArrays[j]);
            }
        }
    }
    
    [Benchmark(OperationsPerInvoke = Iterations)]
    public async Task MultipleSerialOther()
    {
        for (var i = 0; i < Iterations; i++)
        {
            for (var j = 0; j < _nestedArrays.Length; j++)
            {
                _nestedArrays[j] = new byte[4096];
                _nestedArrays[j].AsSpan().Fill((byte)j);
            }

            // Queue to ThreadPool
            await Task.Yield();

            for (var j = _nestedArrays.Length - 1; j >= 0; j--)
            {
                IterateAll(_nestedArrays[j]);
                _nestedArrays[j].AsSpan().Clear();
            }
        }
    }
}