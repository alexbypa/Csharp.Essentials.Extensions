using CSharpEssentials.LoggerHelper;

namespace BusinessLayer.Contracts.Context;
public class ResponseContext {
    public IRequest Request { get; }
    public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>();
    public ResponseContext(IRequest request) {
        Request = request;
    }
}
public class RequestSample : IRequest {
    public string? Action { get; set; }
    public string? UserID { get; set; }
    public string? Token { get; set; } = Guid.NewGuid().ToString();
    public string? IdTransaction { get; set; }
    public string? ApplicationName { get; set; }
    public int secondsDelay { get; set; }
}