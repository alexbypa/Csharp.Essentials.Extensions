using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Processors;
public abstract class ResponseBaseHandler<TRequest> : IResponseHandler<TRequest> {
    protected IResponseHandler<TRequest> _nextHandler;
    protected TRequest _request;
    public OpResult<HttpResponseSpec> _responseBuilder = new OpResult<HttpResponseSpec>();
    protected IhttpsClientHelperFactory _httpFactory;
    public ResponseBaseHandler(TRequest request, IhttpsClientHelperFactory httpFactory) {
        _request = request;
        _httpFactory = httpFactory;
    }
    public virtual async Task<OpResult<HttpResponseSpec>> HandleResponse(string httpOptionName, IhttpsClientHelperFactory httpFactory, ResponseContext responseContext, HttpRequestSpec httpRequestSpec) {
        if (_nextHandler != null) {
            _responseBuilder = await _nextHandler.HandleResponse(httpOptionName, _httpFactory, responseContext, httpRequestSpec);
        }
        return _responseBuilder;
    }
    public virtual async Task SetNext(IResponseHandler<TRequest> handler) {
        await Task.Delay(1);
        _nextHandler = handler;
    }
}