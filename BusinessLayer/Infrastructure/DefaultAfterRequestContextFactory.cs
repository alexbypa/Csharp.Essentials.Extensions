using BusinessLayer.Domain;
using BusinessLayer.Contracts;
using System.Net.Http.Headers;

namespace BusinessLayer.Infrastructure;
public sealed class DefaultAfterRequestContextFactory : IAfterRequestContextFactory {
    public AfterRequestContext Create(
        HttpRequestSpec spec,
        HttpRequestMessage req,
        HttpResponseMessage res,
        int retry,
        TimeSpan elapsed) {
        
        var extras = new Dictionary<string, object?> {
            ["request.headers"] = HeadersToDictionary(req.Headers),
            ["response.headers"] = HeadersToDictionary(res.Headers)
        };

        return new AfterRequestContext(
            Url: req.RequestUri?.ToString() ?? spec.Url,
            Method: req.Method?.Method ?? spec.Method,
            StatusCode: (int)res.StatusCode,
            RetryCount: retry,
            Elapsed: elapsed,
            Extras: extras
        );
    }
    private static IReadOnlyDictionary<string, string> HeadersToDictionary(HttpHeaders headers)
        => headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value));
}