using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;
#if NET7_0
public partial class RegexComparison
{

    private static readonly string s_haystack = new HttpClient().GetStringAsync("https://www.gutenberg.org/files/1661/1661-0.txt").Result;

    private Regex _interpreter = new(@"^.*elementary.*$", RegexOptions.Multiline);
    private Regex _nonBacktracking = new(@"^.*elementary.*$", RegexOptions.NonBacktracking | RegexOptions.Multiline);
    private Regex _compiled = new(@"^.*elementary.*$", RegexOptions.Compiled | RegexOptions.Multiline);
    [GeneratedRegex(@"^.*elementary.*$", RegexOptions.Multiline)]
    private static partial Regex SourceGenerated();

    [Benchmark(Baseline = true)] public int Interpreter() => _interpreter.Count(s_haystack);

    [Benchmark] public int Compiled() => _compiled.Count(s_haystack);
    [Benchmark] public int NonBacktracking() => _nonBacktracking.Count(s_haystack);

    [Benchmark] public int SourceGenerator() => SourceGenerated().Count(s_haystack);
}
#endif