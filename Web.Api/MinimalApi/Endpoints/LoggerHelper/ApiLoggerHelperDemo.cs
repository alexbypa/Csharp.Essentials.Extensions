using BusinessLayer.Contracts.Context;
using CSharpEssentials.LoggerHelper;

namespace Web.Api.MinimalApi.Endpoints.LoggerHelper;
public class ApiLoggerHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {

        app.MapGet("Logger/LogDemo", () => {
            Console.WriteLine("LoggerHelper Demo");
            loggerExtension<RequestSample>.TraceSync(new RequestSample() { IdTransaction = Guid.NewGuid().ToString() }, Serilog.Events.LogEventLevel.Information, null, "Test");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("CHECK SINKS: VERIFY SINKS LOADED:::::::");
            Console.WriteLine($"CURRENTERROR: {GlobalLogger.CurrentError}");
            Console.WriteLine($"TOT ERRORS: {string.Join(",", GlobalLogger.Errors.Select(a => a.SinkName + ' ' + a.ErrorMessage).ToArray())}");
            GlobalLogger.SinksLoaded.ForEach(sink => 
                Console.WriteLine($"Sink loaded: {sink.SinkName} with Level : {string.Join(",", sink.Levels.ToArray())}")
            );
            Console.WriteLine("CHECK SINKS: STOP VERIFY:::::::");
            Console.ResetColor();
            return Results.Ok("LoggerHelper Demo");
        })
        .WithSummary("Simple Request")
        .WithTags("LoggerHelper");
    }
}