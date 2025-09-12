using BusinessLayer.Domain;
using BusinessLayer.Contracts;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Application;
public sealed class FetchAndLogUseCase {
    private readonly INetworkClient _network;
    private readonly IAppLogger? _logger;

    public FetchAndLogUseCase(INetworkClient network, IAppLogger logger)
        => (_network, _logger) = (network, logger);
    public FetchAndLogUseCase(INetworkClient network)
        => (_network) = (network);

    public async Task<OpResult<HttpResponseSpec>> ExecuteAsync(string httpOptionName, HttpRequestSpec spec, IContentBuilder contentBuilder, CancellationToken ct = default) {
        _logger?.Info("Starting HTTP request", new { spec.Url, spec.Method });
        var res = await _network.SendAsync(httpOptionName, spec, contentBuilder, ct);
        if (!res.Success) {
            _logger?.Error("HTTP request failed", null, new { spec.Url, spec.Method, res.Error });
            return res;
        }
        _logger?.Info("HTTP request completed", new { spec.Url, res.Value!.StatusCode });
        return res;
    }
}