using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Reflection
{
    private MethodInfo _method;
    private AssemblyName[] _names = AppDomain.CurrentDomain.GetAssemblies().Select(a => new AssemblyName(a.FullName)).ToArray();

    [GlobalSetup]
    public void Setup() => _method = typeof(Reflection).GetMethod("MyMethod", BindingFlags.NonPublic | BindingFlags.Static);

    // Reflection emit - delegate created by JIT
    [Benchmark]
    public void MethodInfoInvoke() => _method.Invoke(null, null);
    
    // Some calls moved from native to managed - removes overhead
    [Benchmark]
    public Type GetUnderlyingType() => Enum.GetUnderlyingType(typeof(DayOfWeek));
    
    // Intrinsic
    [Benchmark]
    public bool IsByRefLike() => typeof(ReadOnlySpan<char>).IsByRefLike;

    // Stack alloc and ArrayPool<> instead of StringBuilder
    [Benchmark]
    public int Names()
    {
        var sum = 0;
        foreach (var name in _names)
        {
            sum += name.FullName.Length;
        }
        return sum;
    }
    
    private static void MyMethod() { }
}