using CSharpEssentials.HttpHelper;
using HttpMethod = System.Net.Http.HttpMethod;


namespace Web.Api.MinimalApi.Endpoints;
public class ApiHttpHelperDemo : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        IContentBuilder contentBuilder = new NoBodyContentBuilder();

        app.MapGet("Test/AddAction", async (IhttpsClientHelperFactory http) => {
            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
            var client = http.CreateOrGet("Test_No_RateLimit").addTimeout(timeSpan).AddRequestAction((req, res, retry, ts) => {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Http status response was: {res.StatusCode.ToString()}");
                Console.ResetColor();
                return Task.CompletedTask;
            });
            HttpResponseMessage responseMessage = await client.SendAsync("http://www.yoursite.com/users/get", HttpMethod.Get, null, contentBuilder);
            var json = await responseMessage.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        })
        .WithSummary("Add Action")
        .WithTags("HttpHelper");

        app.MapGet("Test/auth/check", CheckAuth)
            .WithTags("HttpHelper")
            .WithSummary("Bearer token check (demo)")
            .WithDescription("""
            Returns **200 OK** when `token` equals `super-secret`.

            Returns **401 Unauthorized** otherwise (or if omitted).
            """);

        app.MapGet("Test/retry", async (IhttpsClientHelperFactory http) => {
            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
            var client = http.CreateOrGet("Test_No_RateLimit")
            .addTimeout(timeSpan)
            .ClearRequestActions()
            .AddRequestAction((req, res, retry, ts) => {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Http status response was: {res.StatusCode.ToString()}");
                Console.ResetColor();
                return Task.CompletedTask;
            })
            .addRetryCondition((res) => res.StatusCode == System.Net.HttpStatusCode.InternalServerError, 3, 0.5);
            HttpResponseMessage responseMessage = await client.SendAsync("http://www.yousite.com/retry", HttpMethod.Get, null, contentBuilder);
            var json = await responseMessage.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        }).WithSummary("Test Retry")
          .WithTags("HttpHelper");

        app.MapGet("Test/testtimeout", async (IhttpsClientHelperFactory http) => {
            TimeSpan timeSpan = TimeSpan.FromSeconds(1);
            var client = http.CreateOrGet("Test_No_RateLimit")
            .addTimeout(timeSpan)
            .ClearRequestActions()
            .AddRequestAction((req, res, retry, ts) => {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: Http status response was: {res.StatusCode.ToString()}");
                Console.ResetColor();
                return Task.CompletedTask;
            });
            var responseMessage = await client.SendAsync("http://www.yousite.com/timeout", HttpMethod.Get, null, contentBuilder);
            var json = await responseMessage.Content.ReadAsStringAsync();
            return Results.Content(content: json, contentType: "application/json", statusCode: (int)responseMessage.StatusCode);
        }).WithSummary("Test Timeout")
          .WithTags("HttpHelper");

        app.MapGet("Test/testratelimit", async (IhttpsClientHelperFactory http) => {
            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
            var client = http.CreateOrGet("Test_With_RateLimit")
            .addTimeout(timeSpan)
            .ClearRequestActions()
            .AddRequestAction((req, res, retry, ts) => {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                var header = req.Headers.FirstOrDefault(x => x.Key == "X-RateLimit-TimeSpanElapsed");
                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: On request {req.RequestUri.AbsolutePath} Http status response was: [{res.StatusCode.ToString()}] X-RateLimit-TimeSpanElapsed: {header.Value.FirstOrDefault()}");
                Console.ResetColor();
                return Task.CompletedTask;
            });
            for (int i = 0; i < 10; i++) {
                var responseMessage = await client.SendAsync($"http://www.yousite.com/users/get/{i}", HttpMethod.Get, null, contentBuilder);
            }
            return Results.Content(content: "Page called", contentType: "application/json", statusCode: 200);
        }).WithSummary("Test Rate Limit")
          .WithTags("HttpHelper");
    }

    public async Task<IResult> CheckAuth(string token, IhttpsClientHelperFactory http) {
        IContentBuilder contentBuilder = new NoBodyContentBuilder();
        TimeSpan timeSpan = TimeSpan.FromSeconds(30);
        var client = http.CreateOrGet("Test_No_RateLimit").addTimeout(timeSpan)
        .setHeadersAndBearerAuthentication(null, new httpsClientHelper.httpClientAuthenticationBearer(token))
        .AddRequestAction((req, res, retry, ts) => {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Http status response was: {res.StatusCode.ToString()}");
            Console.ResetColor();
            return Task.CompletedTask;
        });
        HttpResponseMessage responseMessage = await client.SendAsync("http://www.yoursite.com/auth/check", HttpMethod.Get, null, contentBuilder);
        var json = await responseMessage.Content.ReadAsStringAsync();
        return Results.Content(json, "application/json", statusCode: (int)responseMessage.StatusCode);
    }
}

