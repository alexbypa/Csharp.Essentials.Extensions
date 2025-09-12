namespace BusinessLayer.Domain;
public sealed class HttpResponseSpec {
    public required int StatusCode { get; init; }
    public required string Body { get; init; }
    public Dictionary<string, string>? Headers { get; init; }
}