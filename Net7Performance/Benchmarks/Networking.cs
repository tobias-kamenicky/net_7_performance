using System.Net;
using System.Net.Http.Headers;
using BenchmarkDotNet.Attributes;

namespace Net7Performance.Benchmarks;

public class Networking
{
    private readonly string[] _strings = { "Access-Control-Allow-Credentials", "Access-Control-Allow-Origin", "Cache-Control", "Connection", "Date", "Server" };

    [Benchmark]
    public HttpResponseHeaders GetHeaders()
    {
        var headers = new HttpResponseMessage().Headers;
        foreach (string s in _strings)
        {
            headers.TryAddWithoutValidation(s, s);
        }
        return headers;
    }
    
    private string _encoded = WebUtility.HtmlEncode("""
    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
    Condimentum vitae sapien pellentesque habitant. Vitae auctor eu augue ut lectus. Augue lacus viverra vitae congue eu.
    Tempus quam pellentesque nec nam aliquam sem. Urna nec tincidunt praesent semper feugiat nibh sed. Amet tellus cras adipiscing
    enim eu. Duis ultricies lacus sed turpis tincidunt. Et sollicitudin ac orci phasellus egestas tellus rutrum tellus pellentesque.
    """);

    [Benchmark]
    public string HtmlDecode() => WebUtility.HtmlDecode(_encoded);
}