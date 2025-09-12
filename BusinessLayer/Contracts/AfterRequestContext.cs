namespace BusinessLayer.Contracts;
public sealed record AfterRequestContext(
    string Url,
    string Method,
    int StatusCode,
    int RetryCount,
    TimeSpan Elapsed,
    IReadOnlyDictionary<string, object?> Extras // bag estendibile
);