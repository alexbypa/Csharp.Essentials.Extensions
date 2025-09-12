namespace BusinessLayer.Domain;
public sealed class HttpAuthSpec {
    public string? BearerToken { get; init; }
    public string? BasicUsername { get; init; }
    public string? BasicPassword { get; init; }
}
public sealed class HttpRequestSpec {
    public required string Url { get; init; }
    public string Method { get; init; } = "GET";
    public Dictionary<string, string>? Headers { get; init; }
    public string? Body { get; init; }
    public TimeSpan? Timeout { get; init; }
    public HttpAuthSpec? Auth { get; init; }
}