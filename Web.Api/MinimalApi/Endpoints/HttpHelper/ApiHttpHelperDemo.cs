using CSharpEssentials.HttpHelper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Api.MinimalApi.Endpoints.HttpHelper;
public class ApiHttpHelperDemo : IEndpointDefinition {
    public async Task DefineEndpointsAsync(WebApplication app) {
        IContentBuilder contentBuilder = new NoBodyContentBuilder();
        app.MapGet("Test/proxyweb", async (IhttpsClientHelperFactory httpFactory, string httpOptionName = "testAI") => {
            string url = "https://example.com/";
            var client = httpFactory.CreateOrGet(httpOptionName);

            IContentBuilder nobody = new NoBodyContentBuilder();
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            var response = await client.SendAsync(url, HttpMethod.Get, null, nobody, null, cts.Token);
            try {
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException ex) {
                return Results.Problem($"[FAIL HTTP] connection Error on Proxy/Target: {ex.Message}.");
            } catch (UriFormatException ex) {
                return Results.Problem($"[FAIL HTTP] URI fomat error on configuration: {ex.Message}.");
            }
            return Results.Ok(await response.Content.ReadAsStringAsync());
        })
        .WithTags("HTTP HELPER")
        .WithSummary("proxyweb");


        app.MapGet("Test/TimeOut", async (IhttpsClientHelperFactory httpFactory, string httpOptionName = "testAI") => {
            //Qui dobbiamo simulare il retry con Polly nel caso in cui la richiesta http richiede molto tmepo coni Mocks !!!
            await Task.Delay(10);
            return Results.Ok("TO DO !!!");
        })
        .WithTags("HTTP HELPER")
        .WithSummary("Timeout");





        /*
        app.MapGet("Test/Howcall", (IhttpsClientHelperFactory httpFactory, string httpOptionName, bool useRetry = false) => {
            string url = "http://www.yoursite.com/echo";
            var client = httpFactory.CreateOrGet(httpOptionName);
            client.ClearRequestActions();
            client.AddRequestAction((req, res, retry, elapsed) => {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[GLOBAL ACTION] Nr Retry: {retry} - {req.Method} {req.RequestUri} -> {res.StatusCode} in {elapsed.TotalMilliseconds} ms");
                Console.ResetColor();
                return Task.CompletedTask;
            });
            client.addTimeout(TimeSpan.FromSeconds(10));

            if (useRetry) {
                url = "http://www.yoursite.com/retry";
                client.addRetryCondition((res) => res.StatusCode == HttpStatusCode.InternalServerError, 3, 2);
            }

            IContentBuilder nobody = new NoBodyContentBuilder();
            var response = client.SendAsync(url, HttpMethod.Get, null,  nobody);
            return Results.Ok("See console output for how to call HttpHelper with actions.");
        })
        .WithTags("HttpHelper")
        .WithSummary("Base Request")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status502BadGateway)
        .ProducesProblem(StatusCodes.Status504GatewayTimeout)
        .WithOpenApi(op => {
            // Query parameter
            op.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter {
                Name = "httpOptionName",
                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                Required = true,
                Description = "Named HTTP profile (as configured in appsettings.json). Example: `default`.",
                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" },
                Example = new Microsoft.OpenApi.Any.OpenApiString("default")
            });

            // Response descriptions
            op.Responses["200"].Description = "OK – upstream response payload";
            op.Responses.TryAdd("400", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Bad Request – missing/invalid parameters" });
            op.Responses.TryAdd("502", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Bad Gateway – upstream error" });
            op.Responses.TryAdd("504", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Gateway Timeout – upstream timeout" });

            return op;
        });
        

        //Minimal simple example of HttpHelper usage
        app.MapGet("Test/echo", async (string httpOptionName, IhttpsClientHelperFactory httpFactory) => {
            var spec = new HttpRequestSpec {
                Url = "http://www.yoursite.com/echo",
                Method = "POST",
                Body = "{'name':'Request','value':'Simple'}"
            };

            var ctxFactory = new DefaultAfterRequestContextFactory();
            var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, null);

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
            return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithTags("HttpHelper")
        .WithSummary("Simple Request")
        .WithDescription("Calls the upstream echo endpoint using a named HttpHelper profile from appsettings.json.")
        .Produces<string>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status502BadGateway)
        .ProducesProblem(StatusCodes.Status504GatewayTimeout)
        .WithOpenApi(op => {
            // Query parameter
            op.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter {
                Name = "httpOptionName",
                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                Required = true,
                Description = "Named HTTP profile (as configured in appsettings.json). Example: `default`.",
                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" },
                Example = new Microsoft.OpenApi.Any.OpenApiString("default")
            });

            // Response descriptions
            op.Responses["200"].Description = "OK – upstream response payload";
            op.Responses.TryAdd("400", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Bad Request – missing/invalid parameters" });
            op.Responses.TryAdd("502", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Bad Gateway – upstream error" });
            op.Responses.TryAdd("504", new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Gateway Timeout – upstream timeout" });

            return op;
        });
        //Example of HttpHelper with AddRequestAction
        app.MapGet("Test/AddAction", async ([FromQuery] string httpOptionName, IhttpsClientHelperFactory httpFactory) => {
            var spec = new HttpRequestSpec {
                Url = "http://www.yoursite.com/retry",
                Method = "POST",
                Body = "{'name':'Request','value':'Simple'}"
            };

            var ctxFactory = new DefaultAfterRequestContextFactory();


            // 2) azioni "on demand"(puoi metterne 0..N)
            var actions = new IRequestAction[]{
                new ConsoleColorRequestAction(),
                new InlineRequestAction(ctx =>
                {
                    Console.WriteLine($"[INLINE] {ctx.Method} {ctx.Url} -> {ctx.StatusCode} in {ctx.Elapsed.TotalMilliseconds} ms");
                    return Task.CompletedTask;
                })
            };

            var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, null, actions);

            var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
            return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
        })
        .WithSummary("Add Action")
        .WithTags("HttpHelper")
        .WithDescription("Add a custom action to log the status code of the response and other info.");

        app.MapGet("Test/auth/check", async (
            [FromServices] IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit",
            [FromQuery] string token = "super-secret",
            [FromQuery] string Username = "Username",
            [FromQuery] string Password = "Password"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/auth/check",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(30),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } },
                    Auth = new HttpAuthSpec { BearerToken = token, BasicUsername = Username, BasicPassword = Password }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                IHttpClientStep[] steps = [
                    new TimeoutStep(),
                new HeadersAndBearerStep()
                ];

                var actions = new IRequestAction[]{
                new ConsoleColorRequestAction(),
                new InlineRequestAction(ctx =>
                {
                    Console.WriteLine($"[INLINE] {ctx.Method} {ctx.Url} -> {ctx.StatusCode} in {ctx.Elapsed.TotalMilliseconds} ms");
                    return Task.CompletedTask;
                })
            };

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps, actions);


                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            })
        .WithTags("HttpHelper")
        .WithSummary("Bearer token check (demo)")
        .WithDescription("""
            Returns **200 OK** when `token` equals `super-secret`.

            Returns **401 Unauthorized** otherwise (or if omitted).
        """);

        app.MapGet("Test/retry", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit",
            [FromQuery] int retries = 3,
            [FromQuery] double backofFactoryDelay = 2
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/retry",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(30),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                Dictionary<string, string> logretries = new Dictionary<string, string>();
                var ctxFactory = new DefaultAfterRequestContextFactory();
                var actions = new IRequestAction[]{
                new InlineRequestAction(ctx => {
                    logretries.Add($"Request at {DateTime.Now.ToLongTimeString()} on {ctx.Url}", ctx.StatusCode.ToString());
                    return Task.CompletedTask;
                })};

                IHttpClientStep[] steps = [
                    new TimeoutStep(),
                new RetryConditionStep(
                    res => res.StatusCode == HttpStatusCode.InternalServerError,
                    retries,
                    backofFactoryDelay
                )
                ];

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps, actions);

                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                result.Value?.Headers?.TryAdd("X-Retry-Attempts-Log", string.Join(" | ", logretries.Select(kv => $"{kv.Key} | HttpStatus = {kv.Value}")));
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            }).WithSummary("Test Retry")
            .WithTags("HttpHelper")
            .WithDescription("""
                Sends a POST to `/retry` with **timeout + retry condition** and logs each retry attempt.

                - **Retry condition:** triggered only when upstream returns `500 Internal Server Error`.
                - **Query parameters:**
                  - `httpOptionName` → named profile (default: `Test_No_RateLimit`)
                  - `retries` → max retry attempts (default: `3`)
                  - `backofFactoryDelay` → backoff factor between attempts (default: `2`)
                - **InlineRequestAction:** each retry is logged into an in-memory dictionary with timestamp, URL, and HTTP status.
                - **Response headers:** a custom header `X-Retry-Attempts-Log` aggregates all retry logs.

                This demonstrates how to combine retry logic (`RetryConditionStep`) with custom request actions (`InlineRequestAction`) for fine-grained observability.
                """);

        app.MapGet("Test/testtimeout", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_No_RateLimit"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = "http://www.yoursite.com/timeout",
                    Method = "POST",
                    Body = "{'name':'Request','value':'Simple'}",
                    Timeout = TimeSpan.FromSeconds(1),
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                IHttpClientStep[] steps = [
                    new TimeoutStep()
                ];

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, steps);

                var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
            }).WithSummary("Test Timeout")
              .WithTags("HttpHelper")
              .WithDescription("""
                Executes a POST request against the `/timeout` endpoint with a **1 second timeout**.

                - **Url:** `http://www.yoursite.com/timeout`  
                - **Method:** `POST`  
                - **Body:** `{'name':'Request','value':'Simple'}`  
                - **Timeout:** `1 second` (configured via `TimeoutStep`)  
                - **Headers:** `{ "mode": "Test" }`

                This sample demonstrates how to enforce a request timeout using the `TimeoutStep`
                (which applies the native `IhttpsClientHelper.addTimeout(...)` method).
                """);

        app.MapGet("Test/testratelimit", async (
            IhttpsClientHelperFactory httpFactory,
            [FromQuery] string httpOptionName = "Test_With_RateLimit"
            ) => {
                var spec = new HttpRequestSpec {
                    Url = $"http://www.yoursite.com/users/get",
                    Method = "GET",
                    Body = "{'name':'Request','value':'Simple'}",
                    Headers = new Dictionary<string, string> { { "mode", "Test" } }
                };

                var ctxFactory = new DefaultAfterRequestContextFactory();

                var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory);

                Dictionary<string, OpResult<HttpResponseSpec>> results = new Dictionary<string, OpResult<HttpResponseSpec>>();

                for (int i = 1; i <= 10; i++) {
                    var result = await new FetchAndLogUseCase(netClient).ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder());
                    results.Add($"Step {i}", result);
                }
                return Results.Ok(results);
            }).WithSummary("Test Rate Limit")
          .WithTags("HttpHelper")
          .WithDescription("""
            Executes 10 sequential HTTP GET requests against the configured profile (`httpOptionName`).
            This sample demonstrates **rate limiting** by applying the profile settings from `appsettings.json`.

            - **Url:** `http://www.yoursite.com/users/get`  
            - **Method:** `GET`  
            - **Headers:** `{ "mode": "Test" }`  
            - **Iterations:** 10 requests, each logged via `FetchAndLogUseCase`.

            Each iteration result is collected into a dictionary with keys `Step 1..Step 10`.
            """);
        */
    }
}