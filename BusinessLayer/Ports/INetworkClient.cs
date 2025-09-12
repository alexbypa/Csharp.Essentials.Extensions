using BusinessLayer.Domain;

namespace BusinessLayer.Ports;
public interface INetworkClient {
    Task<OpResult<HttpResponseSpec>> SendAsync(HttpRequestSpec request, CancellationToken ct = default);
}