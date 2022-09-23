using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class JsonText
{
    private JsonSerializerOptions _options = new();
    
    private MyAmazingClass _instance = new();
    [Benchmark] public string ImplicitOptions() => JsonSerializer.Serialize(_instance);
    [Benchmark] public string WithCached() => JsonSerializer.Serialize(_instance, _options);
    [Benchmark] public string WithoutCached() => JsonSerializer.Serialize(_instance, new JsonSerializerOptions());

    public class MyAmazingClass
    {
        public int Value { get; set; }
    }
}