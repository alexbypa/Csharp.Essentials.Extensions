using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using BusinessLayer.Processors;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.Telemetry.Context;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Api.MinimalApi.Endpoints.Telemetries;
public class ApiTelemetryDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        app.MapGet("Telemetry/Simple", getUserInfo)
        .WithSummary("Simple Request")
        .WithTags("Telemetry");
    }
    public async Task<IResult> getUserInfo(
    [FromQuery] string UserID,
    [FromQuery] string Token,
    [FromServices] IhttpsClientHelperFactory httpFactory) {
        RequestSample request = new RequestSample();
        request.Action = "getUserInfo";
        request.UserID = UserID;
        request.Token = Token;

        using var trace = LoggerExtensionWithMetrics<RequestSample>.TraceAsync(request, Serilog.Events.LogEventLevel.Information, null, "Chiamata a minimal api")
         .StartActivity("getUserInfo")
             .AddTag("Minimal API", "GV"); //Per gli span 
        loggerExtension<RequestSample>.TraceAsync(request, Serilog.Events.LogEventLevel.Information, null, "Messaggio di prova");
        VerifyTokenResponseHandler<RequestSample> verifyTokenResponseHandler = new VerifyTokenResponseHandler<RequestSample>(request, httpFactory);
        UserInfoResponseHandler<RequestSample> userInfoResponseHandler = new UserInfoResponseHandler<RequestSample>(request, httpFactory);
        await verifyTokenResponseHandler.SetNext(userInfoResponseHandler);
        trace.Dispose();
        
        var result = await verifyTokenResponseHandler.HandleResponse("Test_No_RateLimit", httpFactory, new ResponseContext(request), new BusinessLayer.Domain.HttpRequestSpec {
            Url = "http://www.yoursite.com/auth/check",
            Method = "POST",
            Body = "{'name':'Request','value':'Simple'}",
            Timeout = TimeSpan.FromSeconds(30),
            Headers = new Dictionary<string, string> { { "mode", "Test" } },
            Auth = new HttpAuthSpec { BearerToken = Token}
        }); 
        loggerExtension<RequestSample>.TraceAsync(request, Serilog.Events.LogEventLevel.Information, null, "Esecuzione completata con risposta {res}", JsonSerializer.Serialize(result));
        
        return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
    }
}