using CSharpEssentials.HttpHelper;

namespace Web.Api.MinimalApi.Endpoints.HttpHelper;
public class HttpHelperDemo {
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
