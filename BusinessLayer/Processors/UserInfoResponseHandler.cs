using BusinessLayer.Application;
using BusinessLayer.Contracts;
using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using BusinessLayer.Infrastructure;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.Telemetry.Context;

namespace BusinessLayer.Processors;
public class UserInfoResponseHandler<TRequest> : ResponseBaseHandler<TRequest> where TRequest : RequestSample {
    TRequest _request;
    IhttpsClientHelperFactory _httpFactory;
    public UserInfoResponseHandler(TRequest request, IhttpsClientHelperFactory httpFactory) : base(request, httpFactory) {
        _request = request;
        _httpFactory = httpFactory;
    }
    public async override Task<OpResult<HttpResponseSpec>> HandleResponse(string httpOptionName, IhttpsClientHelperFactory httpFactory, ResponseContext responseContext, HttpRequestSpec httpRequestSpec) {

        //Try timeout request
        var newHttpRequestSpec = new HttpRequestSpec {
            Url = "http://www.yoursite.com/timeout",
            Method = httpRequestSpec.Method,
            Headers = httpRequestSpec.Headers,
            Body = httpRequestSpec.Body,
            Timeout = TimeSpan.FromSeconds(5),
            Auth = httpRequestSpec.Auth, 
            IdTransaction = httpRequestSpec.IdTransaction
        };

        using var trace = LoggerExtensionWithMetrics<TRequest>.TraceAsync(
            _request,
            Serilog.Events.LogEventLevel.Information,
            null,
            "Query command UserInfoResponseHandler UserID: {UserID}, Token: {Token}", _request.UserID ?? "Unknow", _request.Token ?? "Unknow")
                .StartActivity("GetUserDetails")
                .AddTag("EndPoint: ", newHttpRequestSpec.Url);
        try {
            var ctxFactory = new DefaultAfterRequestContextFactory();
            IHttpClientStep[] steps = [
            new TimeoutStep(),
                new HeadersAndBearerStep()
            ];
            var actions = new IRequestAction[] {
                 new InlineRequestAction(ctx => {
                    loggerExtension<TRequest>.TraceAsync(_request, Serilog.Events.LogEventLevel.Information, null, "request to {url} status Response : {HttpStatus}", ctx.Url, ctx.StatusCode);
                    return Task.CompletedTask;
                })
            };
            var netClient = new HttpHelperNetworkClient(_httpFactory, ctxFactory, steps, actions);

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, newHttpRequestSpec, new NoBodyContentBuilder());
            return result;
        } catch (Exception ex) {
            loggerExtension<TRequest>.TraceAsync(_request, Serilog.Events.LogEventLevel.Error, ex, "Error in UserInfoResponseHandler ");
        } finally {
            trace.Dispose();
        }
        return await base.HandleResponse(httpOptionName, _httpFactory, responseContext, httpRequestSpec);
    }
}