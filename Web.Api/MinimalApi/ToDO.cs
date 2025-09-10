namespace Web.Api.MinimalApi;
//    /*
//            //on program
//            #region UnitOfWork
//                builder.Services.AddUnitOfWorkInfrastructure(builder.Configuration);
//            #endregion
//        */
//    /*
//    app.MapGet("/repo/get", getRepoByUser)
//        .WithName("getUser")
//        .Produces<IResult>(StatusCodes.Status200OK)
//        .WithTags("LoggerHelper");
//    app.MapGet("/repos/search", SearchRepos)
//        .WithName("SearchRepos")
//        .Produces<IResult>(StatusCodes.Status200OK)
//        .WithTags("LoggerHelper / PostgreSQL")
//        .WithGroupName("HttpHelper");
//    app.MapGet("/testuow", testUoW)
//        .WithName("testUoW")
//        .Produces<IResult>(StatusCodes.Status200OK)
//        .WithTags("LoggerHelper / MS SQL")
//        .WithGroupName("HttpHelper");
//    }
//     */

//    /*
//    public async Task<IResult> testUoW([FromServices] IGitHubOptionsRepo repo, [FromQuery] string UserName) {
//        var all = await repo.GetByUserName(UserName);
//        return Results.Ok(all);
//    }
//    public async Task<IResult> SearchRepos([FromQuery] string Pattern, [FromServices] IhttpsClientHelperFactory httpFactory) {
//        RequestDemo request = new RequestDemo() { IdTransaction = Guid.NewGuid().ToString(), Action = "getRepoByUser" };

//        IhttpsClientHelper httpHelper = httpFactory
//            .CreateOrGet("Test1")
//            .AddRequestAction((req, res, retry, ts) => {
//                Console.WriteLine(req);
//                return Task.CompletedTask;
//            });

//        string searchTerm = Pattern;
//        string url = $"https://api.github.com/search/repositories?q={searchTerm}+in:name&per_page=10";

//        httpHelper.setHeadersAndBearerAuthentication(new Dictionary<string, string> { { "User-Agent", "MyGitHubApp" } }, new httpsClientHelper.httpClientAuthenticationBearer("asdasd"));
//        IContentBuilder contentBuilder = new NoBodyContentBuilder();
//        HttpResponseMessage responseMessage = await httpHelper.SendAsync(url, HttpMethod.Get, null, contentBuilder);
//        var json = await responseMessage.Content.ReadAsStringAsync();
//        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
//            using var doc = JsonDocument.Parse(json);

//            var repos = doc.RootElement
//                .GetProperty("items")
//                .EnumerateArray()
//                .Select(repo => new {
//                    Name = repo.GetProperty("full_name").GetString(),
//                    Url = repo.GetProperty("html_url").GetString(),
//                    Description = repo.TryGetProperty("description", out var descProp) && descProp.ValueKind != JsonValueKind.Null
//                                  ? descProp.GetString()
//                                  : "(nessuna descrizione)"
//                })
//                .ToList();
//            return Results.Ok(repos);
//        } else {
//            return Results.StatusCode((int)responseMessage.StatusCode);
//        }

//    }
//    public async Task<IResult> getRepoByUser([FromQuery] string UserName, [FromServices] IhttpsClientHelperFactory httpFactory) {
//        RequestDemo request = new RequestDemo() { IdTransaction = Guid.NewGuid().ToString(), Action = "getRepoByUser" };
//        IhttpsClientHelper httpHelper = httpFactory
//            .CreateOrGet("Test1")
//            .AddRequestAction((req, res, retry, ts) => {
//                Console.WriteLine(req);
//                loggerExtension<RequestDemo>.TraceSync(
//                request,
//                Serilog.Events.LogEventLevel.Information,
//                null,
//                "Eseguita richiesta {req} con risposta {res} numero di retry : {retry}",
//                req, res, retry
//                );


//                return Task.CompletedTask;
//            });
//        var username = "alexbypa";
//        var url = $"https://api.github.com/users/{username}/repos";

//        httpHelper.setHeadersAndBearerAuthentication(new Dictionary<string, string> { { "User-Agent", "MyGitHubApp" } }, new httpsClientHelper.httpClientAuthenticationBearer("asdasd"));

//        IContentBuilder contentBuilder = new NoBodyContentBuilder();
//        HttpResponseMessage responseMessage = await httpHelper.SendAsync(url, HttpMethod.Get, null, contentBuilder);
//        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK) {
//            var json = await responseMessage.Content.ReadAsStringAsync();
//            var data = JsonSerializer.Deserialize<object>(json); // o un modello specifico
//            return Results.Ok(data);
//        } else {
//            return Results.StatusCode((int)responseMessage.StatusCode);
//        }
//    public class RequestDemo : IRequest {
//    public string IdTransaction { get; set; }

//    public string Action { get; set; }

//    public string ApplicationName { get; set; }
//}
//    }
//    */
