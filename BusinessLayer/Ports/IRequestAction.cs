namespace BusinessLayer.Ports;
public sealed class RequestReport {
    public required string Url { get; init; }
    public required string Method { get; init; }
    public required int StatusCode { get; init; }
    public required int RetryCount { get; init; }
    public required TimeSpan Elapsed { get; init; }
}
public interface IRequestAction {
    Task OnAfterRequestAsync(AfterRequestContext ctx);
}