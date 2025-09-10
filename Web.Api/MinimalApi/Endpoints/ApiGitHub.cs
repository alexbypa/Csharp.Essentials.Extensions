using BusinessLayer.Repository.Github;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using HttpMethod = System.Net.Http.HttpMethod;


//TODO: verifica pacchetto HttpHelper se funziona tutto provare a pubblicare la versione 4.0.0 e aggiornare doc.md  
//TODO: Sistemare program.cs e appsettings.json da aggiungere su doc.md


namespace Web.Api.MinimalApi.Endpoints;
public class ApiGitHub : IEndpointDefinition {
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
            HttpResponseMessage responseMessage = await client.SendAsync("http://www.yousite.com/users/get", HttpMethod.Get, null, contentBuilder);
            var json = await responseMessage.Content.ReadAsStringAsync();
            return Results.Content(json, "application/json");
        }).WithSummary("Add Action")
          .WithTags("HttpHelper");

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


        /*
            //on program
            #region UnitOfWork
                builder.Services.AddUnitOfWorkInfrastructure(builder.Configuration);
            #endregion
        */
        /*
        app.MapGet("/repo/get", getRepoByUser)
            .WithName("getUser")
            .Produces<IResult>(StatusCodes.Status200OK)
            .WithTags("LoggerHelper");
        app.MapGet("/repos/search", SearchRepos)
            .WithName("SearchRepos")
            .Produces<IResult>(StatusCodes.Status200OK)
            .WithTags("LoggerHelper / PostgreSQL")
            .WithGroupName("HttpHelper");
        app.MapGet("/testuow", testUoW)
            .WithName("testUoW")
            .Produces<IResult>(StatusCodes.Status200OK)
            .WithTags("LoggerHelper / MS SQL")
            .WithGroupName("HttpHelper");
        */
    }
    /*
    public async Task<IResult> testUoW([FromServices] IGitHubOptionsRepo repo, [FromQuery] string UserName) {
        var all = await repo.GetByUserName(UserName);
        return Results.Ok(all);
    }
    public async Task<IResult> SearchRepos([FromQuery] string Pattern, [FromServices] IhttpsClientHelperFactory httpFactory) {
        RequestDemo request = new RequestDemo() { IdTransaction = Guid.NewGuid().ToString(), Action = "getRepoByUser" };

        IhttpsClientHelper httpHelper = httpFactory
            .CreateOrGet("Test1")
            .AddRequestAction((req, res, retry, ts) => {
                Console.WriteLine(req);
                return Task.CompletedTask;
            });

        string searchTerm = Pattern;
        string url = $"https://api.github.com/search/repositories?q={searchTerm}+in:name&per_page=10";

        httpHelper.setHeadersAndBearerAuthentication(new Dictionary<string, string> { { "User-Agent", "MyGitHubApp" } }, new httpsClientHelper.httpClientAuthenticationBearer("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"));
        IContentBuilder contentBuilder = new NoBodyContentBuilder();
        HttpResponseMessage responseMessage = await httpHelper.SendAsync(url, HttpMethod.Get, null, contentBuilder);
        var json = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
            using var doc = JsonDocument.Parse(json);

            var repos = doc.RootElement
                .GetProperty("items")
                .EnumerateArray()
                .Select(repo => new {
                    Name = repo.GetProperty("full_name").GetString(),
                    Url = repo.GetProperty("html_url").GetString(),
                    Description = repo.TryGetProperty("description", out var descProp) && descProp.ValueKind != JsonValueKind.Null
                                  ? descProp.GetString()
                                  : "(nessuna descrizione)"
                })
                .ToList();
            return Results.Ok(repos);
        } else {
            return Results.StatusCode((int)responseMessage.StatusCode);
        }

    }
    public async Task<IResult> getRepoByUser([FromQuery] string UserName, [FromServices] IhttpsClientHelperFactory httpFactory) {
        RequestDemo request = new RequestDemo() { IdTransaction = Guid.NewGuid().ToString(), Action = "getRepoByUser" };
        IhttpsClientHelper httpHelper = httpFactory
            .CreateOrGet("Test1")
            .AddRequestAction((req, res, retry, ts) => {
                Console.WriteLine(req);
                loggerExtension<RequestDemo>.TraceSync(
                request,
                Serilog.Events.LogEventLevel.Information,
                null,
                "Eseguita richiesta {req} con risposta {res} numero di retry : {retry}",
                req, res, retry
                );


                return Task.CompletedTask;
            });
        var username = "alexbypa";
        var url = $"https://api.github.com/users/{username}/repos";

        httpHelper.setHeadersAndBearerAuthentication(new Dictionary<string, string> { { "User-Agent", "MyGitHubApp" } }, new httpsClientHelper.httpClientAuthenticationBearer("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"));

        IContentBuilder contentBuilder = new NoBodyContentBuilder();
        HttpResponseMessage responseMessage = await httpHelper.SendAsync(url, HttpMethod.Get, null, contentBuilder);
        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
            var json = await responseMessage.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<object>(json); // o un modello specifico
            return Results.Ok(data);
        } else {
            return Results.StatusCode((int)responseMessage.StatusCode);
        }
    }
    */
}

public class RequestDemo : IRequest {
    public string IdTransaction { get; set; }

    public string Action { get; set; }

    public string ApplicationName { get; set; }
}