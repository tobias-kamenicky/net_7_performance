using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class LoopHoisting
{
    [Benchmark]
    public void Compute()
    {
        var sum = 0;
        for (var thousands = 0; thousands < 10; thousands++)
        {
            for (var hundreds = 0; hundreds < 10; hundreds++)
            {
                for (var tens = 0; tens < 10; tens++)
                {
                    for (var ones = 0; ones < 10; ones++)
                    {
                        sum += ComputeNumber(thousands, hundreds, tens, ones);
                        Process(sum);
                    }
                }
            }
        }
    }
    static int ComputeNumber(int thousands, int hundreds, int tens, int ones) =>
        (thousands * 1000) +
        (hundreds * 100) +
        (tens * 10) +
        ones;

    [MethodImpl(MethodImplOptions.NoInlining)]
    static void Process(int n) { }
}