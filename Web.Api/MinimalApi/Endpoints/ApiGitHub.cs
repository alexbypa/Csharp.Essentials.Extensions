using BusinessLayer.Repository.Github;
using CSharpEssentials.HttpHelper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Web.Api.MinimalApi.Endpoints;
public class ApiGitHub : IEndpointDefinition {
    public void DefineEndpoints(WebApplication app) {
        app.MapGet("/repo/get", getRepoByUser)
            .WithName("getUser")
            .Produces<IResult>(StatusCodes.Status200OK);
        app.MapGet("/repos/search", SearchRepos)
            .WithName("SearchRepos")
            .Produces<IResult>(StatusCodes.Status200OK);
        app.MapGet("/testuow", testUoW)
            .WithName("testUoW")
            .Produces<IResult>(StatusCodes.Status200OK);
    }
    public async Task<IResult> testUoW([FromServices] IGitHubOptionsRepo repo, [FromQuery] string UserName) {
        var all = await repo.GetByUserName(UserName);
        return Results.Ok(all);
    }
    public async Task<IResult> SearchRepos([FromQuery] string Pattern, [FromServices] IhttpsClientHelper httpFactory) {
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
}

public class RequestDemo : IRequest {
    public string IdTransaction { get; set; }

    public string Action { get; set; }

    public string ApplicationName { get; set; }
}

/*

 2. 📄 Ottenere il contenuto del README.md di un repo
csharp
Copia
Modifica
var url = "https://api.github.com/repos/alexbypa/recap/readme";

var response = await httpClient.GetAsync(url);
var json = await response.Content.ReadAsStringAsync();
Console.WriteLine(json);

6. 🧪 Leggere workflow runs (es. test CI/CD GitHub Actions)
csharp
Copia
Modifica
var url = "https://api.github.com/repos/alexbypa/recap/actions/runs";

var response = await httpClient.GetAsync(url);
var json = await response.Content.ReadAsStringAsync();
Console.WriteLine(json);

7. ✅ Ottenere copertura test (es. da un file coverage.json nel repo)
Se salvi un file di test coverage (es. da coverlet) in GitHub puoi usare l’API per leggerlo:

csharp
Copia
Modifica
var url = "https://api.github.com/repos/alexbypa/recap/contents/coverage/coverage.json";

var response = await httpClient.GetAsync(url);
var json = await response.Content.ReadAsStringAsync();
Console.WriteLine(json);
*/