using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using CSharpEssentials.HttpHelper;

namespace BusinessLayer.Processors;
public interface IResponseHandler<TRequest> {
    Task SetNext(IResponseHandler<TRequest> handler);
    Task<OpResult<HttpResponseSpec>> HandleResponse(string httpOptionName, IhttpsClientHelperFactory httpFactory, ResponseContext responseContext, HttpRequestSpec httpRequestSpec);
}