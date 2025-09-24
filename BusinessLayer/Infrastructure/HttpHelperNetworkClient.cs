using BusinessLayer.Contracts;
using BusinessLayer.Domain;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Infrastructure;
public sealed class HttpHelperNetworkClient : INetworkClient {
    private readonly IhttpsClientHelperFactory _factory;
    private readonly IAfterRequestContextFactory _ctxFactory;
    private readonly IEnumerable<IHttpClientStep>? _steps;    // NEW: passi configurazione
    private readonly IEnumerable<IRequestAction> _actions;

    public HttpHelperNetworkClient(IhttpsClientHelperFactory factory, IAfterRequestContextFactory ctxFactory, IEnumerable<IHttpClientStep>? steps = null, IEnumerable<IRequestAction>? actions = null) {
        _factory = factory;
        _ctxFactory = ctxFactory;
        _actions = actions ?? Enumerable.Empty<IRequestAction>();
        _steps = steps ?? Enumerable.Empty<IHttpClientStep>();
        ;
    }
    public async Task<OpResult<HttpResponseSpec>> SendAsync(string HttpOptionName, HttpRequestSpec request, IContentBuilder contentBuilder, CancellationToken ct = default) {
        try {
            var client = _factory.CreateOrGet(HttpOptionName);
            foreach (var step in _steps)
                client = step.Apply(client, request);

            client = client.AddRequestAction(async (req, res, retry, elapsed) => {
                var ctx = _ctxFactory.Create(request, req, res, retry, elapsed);
                foreach (var a in _actions)
                    await a.OnAfterRequestAsync(ctx);
            });

            var response = await client.SendAsync(
                request.Url,
                request.Method switch {
                    "POST" => HttpMethod.Post,
                    "PUT" => HttpMethod.Put,
                    "DELETE" => HttpMethod.Delete,
                    "OPTIONS" => HttpMethod.Options,
                    _ => HttpMethod.Get
                },
                request.Body,
                contentBuilder);

            return OpResult<HttpResponseSpec>.Ok(new HttpResponseSpec {
                StatusCode = (int)response.StatusCode,
                Body = await response.Content.ReadAsStringAsync(ct),
                Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(",", h.Value))
            }, request.IdTransaction);
        } catch (Exception ex) {
            return OpResult<HttpResponseSpec>.Fail(ex.Message, request.IdTransaction);
        }
    }
}