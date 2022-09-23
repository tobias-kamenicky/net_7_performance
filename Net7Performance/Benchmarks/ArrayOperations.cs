using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

// For span and arrays of chars/strings/bytes
public class ArrayOperations
{
    private static readonly string s_haystack = new HttpClient().GetStringAsync("https://www.gutenberg.org/files/1661/1661-0.txt").Result;

    [Benchmark]
    [Arguments("Sherlock")]
    [Arguments("elementary")]
    public int Count(string needle)
    {
        ReadOnlySpan<char> haystack = s_haystack;
        int count = 0, pos;
        while ((pos = haystack.IndexOf(needle)) >= 0)
        {
            haystack = haystack[(pos + needle.Length)..];
            count++;
        }

        return count;
    }

    [Benchmark]
    // Vectorized for all types and numbers of args
    public int LastIndexOfAny() => s_haystack.LastIndexOfAny(new[] {'z', 'q'});
    
    private byte[] _dataContains = new byte[95];
    
    [Benchmark]
    // Leftover elements are vectorized as well, even if duplicates some work already done
    public bool Contains() => _dataContains.AsSpan().Contains((byte)1);
    
    private int[] _dataIndex = new int[1000];
    
    [Benchmark]
    // Now vectorized for 4 and 8 byte sized types
    public int IndexOf() => _dataIndex.AsSpan().IndexOf(42);
}