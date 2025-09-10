using CSharpEssentials.LoggerHelper;

namespace Web.Api.MinimalApi.Endpoints.LoggerHelper;
public class ApiLoggerHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {

        app.MapGet("Logger/LogDemo", () => {
            Console.WriteLine("LoggerHelper Demo");
            loggerExtension<RequestHelper>.TraceSync(new RequestHelper() { IdTransaction = Guid.NewGuid().ToString() }, Serilog.Events.LogEventLevel.Information, null, "Test");
            return Results.Ok("LoggerHelper Demo");
        })
        .WithSummary("Simple Request")
        .WithTags("LoggerHelper");
    }
}
public class RequestHelper : IRequest {
    public string IdTransaction {get;set;}

    public string Action {get;set;}

    public string ApplicationName {get;set;}
}