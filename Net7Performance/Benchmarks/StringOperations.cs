using System.Text;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class StringOperations
{
    private string _value = "https://dot.net";

    bool IsAsciiLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    bool IsAsciiLetterFancy(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
    bool IsAsciiSmart(char c) => (uint) ((c | 0x20) - 'a') <= 'z' - 'a';
    
    [Benchmark]
    // Recognizes that the string is a constant, and compares it as a single <long>
    public bool IsHttps_Ordinal() => _value.StartsWith("https://", StringComparison.Ordinal);

    [Benchmark]
    public bool IsHttps_OrdinalIgnoreCase() => _value.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
    
    private char[] text = "Free. Cross-platform. Open source.\r\nA developer platform for building all your apps.".ToCharArray();

    [Benchmark]
    public void Reverse() => Array.Reverse(text);
    
    private byte[] _data = Encoding.UTF8.GetBytes("""
    Shall I compare thee to a summer's day?
    Thou art more lovely and more temperate:
    Rough winds do shake the darling buds of May,
    And summer's lease hath all too short a date;
    Sometime too hot the eye of heaven shines,
    And often is his gold complexion dimm'd;
    And every fair from fair sometime declines,
    By chance or nature's changing course untrimm'd;
    But thy eternal summer shall not fade,
    Nor lose possession of that fair thou ow'st;
    Nor shall death brag thou wander'st in his shade,
    When in eternal lines to time thou grow'st:
    So long as men can breathe or eyes can see,
    So long lives this, and this gives life to thee.
    """);
    private char[] _encoded = new char[1000];

    [Benchmark]
    // ? not as fast as expected?
    public bool TryToBase64Chars() => Convert.TryToBase64Chars(_data, _encoded, out _);
}