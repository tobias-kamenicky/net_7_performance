using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class RegularExpressions
{
    private static readonly string s_haystack = new HttpClient().GetStringAsync("https://www.gutenberg.org/files/1661/1661-0.txt").Result;
    
    // Uri pattern
    private Regex _regexUrl = new(@"[\w]+://[^/\s?#]+[^\s?#]+(?:\?[^\s#]*)?(?:#[^\s]*)?", RegexOptions.Compiled);
    [Benchmark]
    public bool IsMatch_Url() => _regexUrl.IsMatch(s_haystack);
    
    private Regex _regexAnchors = new(@"^abc|^def", RegexOptions.Compiled);
    [Benchmark]
    public bool IsMatch_Anchors() => _regexAnchors.IsMatch(s_haystack);
    
    private Regex _regexEnding = new(@"looking|feeling", RegexOptions.Compiled);
    [Benchmark]
    public int Count_ByEnding() => _regexEnding.Matches(s_haystack).Count; 
    
    private Regex _regexLinesWithWord = new(@"^.*elementary.*$", RegexOptions.Compiled | RegexOptions.Multiline);
    // backtracking in compiled and source generated engines has been improved, takes advantage of IndexOf('\n') and LastIndexOf('abc') to find next viable locations
    [Benchmark]
    public int Count_LinesWithWord() => _regexLinesWithWord.Matches(s_haystack).Count;
    //
    private static readonly string s_haystack_a = new('a', 1_000_000);
    
    private Regex _regex = new(@"([\s\S]*)", RegexOptions.Compiled);
    [Benchmark]
    public Match MatchAnything() => _regex.Match(s_haystack_a);
}