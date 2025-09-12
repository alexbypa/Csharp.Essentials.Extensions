using BusinessLayer.Domain;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Contracts;
public interface IHttpClientStep {
    IhttpsClientHelper Apply(IhttpsClientHelper client, HttpRequestSpec spec);
}
public sealed class TimeoutStep : IHttpClientStep {
    public IhttpsClientHelper Apply(IhttpsClientHelper client, HttpRequestSpec spec)
        => spec.Timeout is { } to ? client.addTimeout(to) : client;
}
public sealed class HeadersAndBearerStep : IHttpClientStep {
    public IhttpsClientHelper Apply(IhttpsClientHelper client, HttpRequestSpec spec) {
        var hasHeaders = spec.Headers is not null && spec.Headers.Count > 0;
        var hasBearer = !string.IsNullOrWhiteSpace(spec.Auth?.BearerToken);

        if (!hasHeaders && !hasBearer)
            return client;

        var bearer = hasBearer
            ? new httpsClientHelper.httpClientAuthenticationBearer(spec.Auth!.BearerToken!)
            : null;

        
        return client.setHeadersAndBearerAuthentication(spec.Headers, bearer);
    }
}
public sealed class BasicAuthStep : IHttpClientStep {
    public IhttpsClientHelper Apply(IhttpsClientHelper client, HttpRequestSpec spec) {
        var a = spec.Auth;
        var isBasic = a is not null
                      && !string.IsNullOrWhiteSpace(a.BasicUsername)
                      && !string.IsNullOrWhiteSpace(a.BasicPassword);

        if (!isBasic)
            return client;
        var basic = new httpsClientHelper.httpClientAuthenticationBasic(a!.BasicUsername!, a.BasicPassword!);

        return client.setHeadersAndBasicAuthentication(spec.Headers, basic);
    }
}

public sealed class RetryConditionStep : IHttpClientStep {
    private readonly Func<HttpResponseMessage, bool> _predicate;
    private readonly int _maxRetries;
    private readonly double _delayBackoffFactor;

    public RetryConditionStep(
        Func<HttpResponseMessage, bool> predicate,
        int maxRetries,
        double delayFactorSeconds) {
        _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        _maxRetries = maxRetries;
        _delayBackoffFactor = delayFactorSeconds;
    }

    public IhttpsClientHelper Apply(IhttpsClientHelper client, HttpRequestSpec spec)
        => client.addRetryCondition(_predicate, _maxRetries, _delayBackoffFactor);
}