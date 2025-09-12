using BusinessLayer.Domain;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Ports;
public interface INetworkClient {
    Task<OpResult<HttpResponseSpec>> SendAsync(string HttpOptionName, HttpRequestSpec request, IContentBuilder contentBuilder, CancellationToken ct = default);
}