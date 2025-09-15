using BusinessLayer.Contracts.Context;
using BusinessLayer.Domain;
using BusinessLayer.Processors;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.Telemetry.Context;
using CSharpEssentials.LoggerHelper.Telemetry.Metrics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace Web.Api.MinimalApi.Endpoints.Telemetries;

public class ApiTelemetryDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        app.MapGet("Telemetry/Simple", getUserInfo)
           .WithSummary("Simple Request")
           .WithTags("Telemetry");

        // NEW: parallel launcher
        app.MapPost("Telemetry/Parallel", runUserInfoInParallel)
           .WithSummary("Runs N parallel getUserInfo calls")
           .WithDescription("Launches N parallel tasks calling getUserInfo; each task receives a UserID and Token (sometimes the token is the valid 'super-secret').")
           .WithTags("Telemetry");
    }

    // -------------------------
    // MODEL for the parallel run
    // -------------------------
    public record ParallelRequest(
        int N,
        double? ValidRatio // 0..1 
    );

    public record UserToken(
        string UserID,
        string Token
    );

    static class ParallelTelemetry {
        private static readonly Meter _meter = new("CSharpEssentials.Telemetry.Parallel", "1.0.0");
        private static GaugeWrapper<int>? _activeGauge;
        private static GaugeWrapper<long>? _totalGauge;
        
        private static int _active;
        private static long _totalRuns;

        static ParallelTelemetry() {
            // gauge: # di task attivi in questo momento
            _activeGauge = new GaugeWrapper<int>(
                _meter,
                name: "telemetry.parallel_active_requests",
                valueProvider: () => Volatile.Read(ref _active),
                unit: "count",
                description: "Current active parallel getUserInfo tasks");

            // gauge: totale esecuzioni lanciate dall’avvio
            _totalGauge = new GaugeWrapper<long>(
                _meter,
                name: "telemetry.parallel_total_runs",
                valueProvider: () => Interlocked.Read(ref _totalRuns),
                unit: "count",
                description: "Total parallel getUserInfo runs since app start");
        }

        public static void IncActive() => Interlocked.Increment(ref _active);
        public static void DecActive() => Interlocked.Decrement(ref _active);
        public static void IncTotal() => Interlocked.Increment(ref _totalRuns);
    }

    // -------------------------
    // BASE endpoint (already present)
    // -------------------------
    public async Task<IResult> getUserInfo(
        [FromQuery] string UserID,
        [FromQuery] string Token,
        [FromServices] IhttpsClientHelperFactory httpFactory) {
        var request = new RequestSample {
            Action = "getUserInfo",
            UserID = UserID,
            Token = Token
        };

        using var trace = LoggerExtensionWithMetrics<RequestSample>
            .TraceAsync(request, Serilog.Events.LogEventLevel.Information, null, "Chiamata a minimal api")
            .StartActivity("getUserInfo")
            .AddTag("Minimal API", "GV"); // Per gli span

        loggerExtension<RequestSample>.TraceAsync(request, Serilog.Events.LogEventLevel.Information, null, "Messaggio di prova");

        var verifyTokenResponseHandler = new VerifyTokenResponseHandler<RequestSample>(request, httpFactory);
        var userInfoResponseHandler = new UserInfoResponseHandler<RequestSample>(request, httpFactory);
        await verifyTokenResponseHandler.SetNext(userInfoResponseHandler);

        // Esegue la chiamata esterna con il token passato (talvolta 'super-secret')
        var result = await verifyTokenResponseHandler.HandleResponse(
            "Test_No_RateLimit",
            httpFactory,
            new ResponseContext(request),
            new BusinessLayer.Domain.HttpRequestSpec {
                Url = "http://www.yoursite.com/auth/check",
                Method = "POST",
                Body = "{'name':'Request','value':'Simple'}",
                Timeout = TimeSpan.FromSeconds(30),
                Headers = new Dictionary<string, string> { { "mode", "Test" } },
                Auth = new HttpAuthSpec { BearerToken = Token }
            });

        loggerExtension<RequestSample>.TraceAsync(
            request,
            Serilog.Events.LogEventLevel.Information,
            null,
            "Esecuzione completata con risposta {res}",
            JsonSerializer.Serialize(result));

        return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
    }

    // -------------------------
    // NEW: parallel launcher endpoint
    // -------------------------
    public async Task<IResult> runUserInfoInParallel(
        [FromBody] ParallelRequest body,
        [FromServices] IhttpsClientHelperFactory httpFactory,
        CancellationToken ct) {
        if (body is null || body.N <= 0)
            return Results.BadRequest("Provide N > 0.");

        var ratio = body.ValidRatio is >= 0 and <= 1 ? body.ValidRatio.Value : 0.5;

        // Genera N utenti e token (alcuni validi "super-secret", altri random)
        var items = Enumerable.Range(0, body.N).Select(i =>
        {
            var user = $"user-{i:D4}-{RandomSuffix(5)}";
            var token = ShouldUseValidToken(ratio) ? "super-secret" : RandomSuffix(16);
            return new UserToken(user, token);
        }).ToList();

        var tasks = new List<Task<(bool ok, object? value, string? error, string user, string token)>>(body.N);

        for (int i = 0; i < body.N; i++) {
            var pick = items[i]; // già N elementi
            tasks.Add(Task.Run(async () =>
            {

                var activeCtx = new {
                    MetricName = "telemetry.parallel_active_requests",
                    Value = 10, // es. Volatile.Read(...)
                    Unit = "count",
                    Description = "Current active parallel getUserInfo tasks",
                    Kind = "gauge"
                };
                loggerExtension<RequestSample>.TraceAsync(new RequestSample { Action = "ParallelRun", UserID = pick.UserID, Token = pick.Token },
                    Serilog.Events.LogEventLevel.Information, null,
                    "metric {@tcx}",
                    activeCtx);

                // 2) Counter: incremento di 1
                var totalCtx = new {
                    MetricName = "telemetry.parallel_total_runs",
                    Value = 1,
                    Unit = "count",
                    Description = "Total parallel getUserInfo runs since app start",
                    Kind = "counter"
                };

                loggerExtension<RequestSample>.TraceAsync(new RequestSample { Action = "ParallelRun", UserID = pick.UserID, Token = pick.Token },
                    Serilog.Events.LogEventLevel.Information, null,
                    "metric {@tcx}",
                    totalCtx);

                ParallelTelemetry.IncActive();
                ParallelTelemetry.IncTotal();
                try {
                    var outcome = await getUserInfo(pick.UserID, pick.Token, httpFactory);
                    var (ok, value, error) = await MaterializeIResult(outcome);
                    return (ok, value, error, pick.UserID, pick.Token);
                } catch (Exception ex) {
                    return (false, null, ex.Message, pick.UserID, pick.Token);
                } finally {
                    ParallelTelemetry.DecActive();
                }
            }, ct));
        }

        var results = await Task.WhenAll(tasks);

        var response = new {
            requested = body.N,
            validRatio = ratio,
            validTokenRuns = results.Count(r => r.token == "super-secret"),
            invalidTokenRuns = results.Count(r => r.token != "super-secret"),
            success = results.Count(r => r.ok),
            failed = results.Count(r => !r.ok),
            items = results.Select(r => new
            {
                r.user,
                token = r.token == "super-secret" ? "<valid>" : "<invalid>",
                r.ok,
                r.error
            })
        };

        return Results.Ok(response);

        // ---- helpers locali ----
        static bool ShouldUseValidToken(double r)
            => Random.Shared.NextDouble() < r;

        static string RandomSuffix(int len) {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
            Span<char> buf = stackalloc char[len];
            for (int i = 0; i < len; i++)
                buf[i] = alphabet[Random.Shared.Next(alphabet.Length)];
            return new string(buf);
        }
    }

    /// <summary>
    /// Utility: materializza un IResult in (ok, value, error) per poter aggregare le risposte.
    /// </summary>
    private static async Task<(bool ok, object? value, string? error)> MaterializeIResult(IResult result) {
        // Minimal e semplice: intercetta OkObjectResult / ProblemHttpResult.
        // Se servisse più precisione, si può migliorare con reflection sui tipi concreti.
        switch (result) {
            case Microsoft.AspNetCore.Http.HttpResults.Ok<object> okObj:
                return (true, okObj.Value, null);
            case Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult problem:
                return (false, null, problem.ProblemDetails?.Detail ?? "Problem");
            default:
                // come fallback, consideriamo "ok" se non è un Problem
                return (true, null, null);
        }
    }
    // ---- helpers locali ----
    static bool ShouldUseValidToken(double r)
        => Random.Shared.NextDouble() < r;

    static string RandomSuffix(int len) {
        const string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        Span<char> buf = stackalloc char[len];
        for (int i = 0; i < len; i++)
            buf[i] = alphabet[Random.Shared.Next(alphabet.Length)];
        return new string(buf);
    }
}
