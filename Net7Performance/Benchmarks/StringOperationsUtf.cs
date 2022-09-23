using System.Text;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class StringOperationsUtf
{
#if NET7_0
    [Benchmark(Baseline = true)]
    public ReadOnlySpan<byte> WithEncoding() => Encoding.UTF8.GetBytes("test");

    [Benchmark]
    public ReadOnlySpan<byte> Withu8() => "test"u8;
#endif
}