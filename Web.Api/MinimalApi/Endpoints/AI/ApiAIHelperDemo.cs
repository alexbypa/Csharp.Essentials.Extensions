using CSharpEssentials.LoggerHelper.AI;
using CSharpEssentials.LoggerHelper.AI.Domain;

namespace Web.Api.MinimalApi.Endpoints.AI;
public class ApiAIHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        app.MapPost("/IAI/run", async (IActionOrchestrator orc, MacroContext ctx, CancellationToken ct) =>
            Results.Ok(await orc.RunAsync(ctx, ct)));
    }
}
