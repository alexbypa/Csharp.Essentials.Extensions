using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace Web.Api.MinimalApi.Mocks;
public class HttpMocks {
    public static HttpMessageHandler CreateHandler() {
        var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        // JSON di esempio (simula "users/dotnet/repos")
        var repoJson = """
        [
          {"name":"runtime","stargazers_count":12345},
          {"name":"aspnetcore","stargazers_count":23456}
        ]
        """;

        // Contatore per simulare transient error + retry
        var retryCount = 0;

        // GET: /users/dotnet/repos  -> 200 OK
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Get &&
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/users/get")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(repoJson, Encoding.UTF8, "application/json")
            });

        // GET: /not-found  -> 404
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/not-found")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.NotFound) {
                Content = new StringContent("""{"message":"Not Found"}""", Encoding.UTF8, "application/json")
            });

        // GET: /retry  -> 500, 500, poi 200
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/retry")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => {
                Task.Delay(500).GetAwaiter().GetResult(); // Simula un piccolo ritardo
                if (retryCount < 2) {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError) {
                        Content = new StringContent("""{"message":"Transient error"}""", Encoding.UTF8, "application/json")
                    };
                }
                retryCount = 0; 
                return new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent("""{"ok":true}""", Encoding.UTF8, "application/json")
                };
            });

        // POST: /echo  -> 
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.Method == HttpMethod.Post &&
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/echo")),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((req, _) => {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = req.Content // echo
                });
            });

        // GET: /timeout -> ritardo > TimeoutSeconds per scatenare timeout
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/timeout")),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>(async (req, ct) => {
                await Task.Delay(TimeSpan.FromSeconds(30), ct);
                ct.ThrowIfCancellationRequested();
                return new HttpResponseMessage(HttpStatusCode.OK) {
                    Content = new StringContent("""{"delayed":true}""", Encoding.UTF8, "application/json")
                };
            });

        // POST: /Test bearertoken -> 200 OK se header Authorization corretto, altrimenti 401
        mock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r =>
                    r.RequestUri != null &&
                    r.RequestUri.AbsolutePath.Contains("/auth/check")),
                ItExpr.IsAny<CancellationToken>())
            .Returns<HttpRequestMessage, CancellationToken>((req, _) =>
            {
                var token = req.Headers.Authorization?.Parameter;
                if (token == "super-secret") {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {
                        Content = new StringContent("""{"authenticated":true}""", Encoding.UTF8, "application/json")
                    });
                }

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized) {
                    Content = new StringContent("""{"error":"unauthorized"}""", Encoding.UTF8, "application/json")
                });
            });



        return mock.Object;
    }
}