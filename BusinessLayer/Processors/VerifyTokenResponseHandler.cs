using BusinessLayer.Application;
using BusinessLayer.Contracts;
using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using BusinessLayer.Infrastructure;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.Telemetry.Context;

namespace BusinessLayer.Processors;
public class VerifyTokenResponseHandler<TRequest> : ResponseBaseHandler<TRequest> where TRequest : RequestSample {
    TRequest _request;
    IhttpsClientHelperFactory _httpFactory;
    public VerifyTokenResponseHandler(TRequest request, IhttpsClientHelperFactory httpFactory) : base(request, httpFactory) {
        _request = request;
        _httpFactory = httpFactory;
    }
    public async override Task<OpResult<HttpResponseSpec>> HandleResponse(string httpOptionName, IhttpsClientHelperFactory httpFactory, ResponseContext responseContext, HttpRequestSpec httpRequestSpec) {
        using var trace = LoggerExtensionWithMetrics<TRequest>.TraceAsync(
            _request,
            Serilog.Events.LogEventLevel.Information,
            null,
            "Query command VerifyTokenResponseHandler UserID: {UserID}, Token: {Token}", _request.UserID ?? "Unknow", _request.Token ?? "Unknow")
            .StartActivity("VerifyToken")
            .AddTag("UserID", _request.UserID ?? "Unknow");
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

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, httpRequestSpec, new NoBodyContentBuilder());
            return result;
        } catch (Exception ex) {
            loggerExtension<TRequest>.TraceAsync(_request, Serilog.Events.LogEventLevel.Error, ex, "Error in VerifyTokenResponseHandler");
        } finally {
            trace.Dispose();
        }
        return await base.HandleResponse(httpOptionName, _httpFactory, responseContext, httpRequestSpec);
    }
}