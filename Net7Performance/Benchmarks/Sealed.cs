using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Sealed
{
    public class BaseClass
    {
        public virtual void VoidMethod() {}
        public virtual int IntMethod() => 0;
    }

    public class OpenClass : BaseClass
    {
        public override void VoidMethod() { }
        public override int IntMethod() => 42;
    }
    
    public sealed class SealedClass : BaseClass
    {
        public override void VoidMethod() { }
        public override int IntMethod() => 24;
    }

    private readonly BaseClass _base = new();
    private readonly OpenClass _open = new();
    private readonly SealedClass _sealed = new();
    private readonly OpenClass[] _openClasses = new OpenClass[1];
    private readonly SealedClass[] _sealedClasses = new SealedClass[1];
    
    [Benchmark] public void VoidMethod_OpenClass() => _open.VoidMethod();
    [Benchmark] public void VoidMethod_SealedClass() => _sealed.VoidMethod();
    
    [Benchmark] public int IntMethod_OpenClass() => _open.IntMethod() + 1234;
    [Benchmark] public int IntMethod_SealedClass() => _sealed.IntMethod() + 1234;
    
    [Benchmark] public bool Is_OpenClass() => _base is OpenClass;
    [Benchmark] public bool Is_SealedClass() => _base is SealedClass;
    
    [Benchmark] public void Array_OpenClass() => _openClasses[0] = _open;
    [Benchmark] public void Array_SealedClass() => _sealedClasses[0] = _sealed;
    
    [Benchmark] public Span<OpenClass> Span_OpenClass() => _openClasses;
    [Benchmark] public Span<SealedClass> Span_SealedClass() => _sealedClasses;
}