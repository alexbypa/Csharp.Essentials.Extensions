# CSharp.Essentials.Extensions

## 📖 Overview
CSharp.Essentials.Extensions is a collection of helper libraries that simplify common .NET development scenarios.  
It provides ready-to-use HTTP client helpers, structured logging with multiple sinks, and useful extension methods.

---

## 📦 NuGet Packages

| Package | Description | NuGet |
|---|---|---|
| **CSharpEssentials.HttpHelper** | Simplifies `HttpClient` usage with built-in resiliency (retries/fallbacks) and rate-limiting strategies. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.HttpHelper) |
| **CSharpEssentials.Extensions** | Provides essential C# extension methods to simplify development. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.Extensions) |
| **CSharpEssentials.LoggerHelper** | Structured logging helper built on top of Serilog, with sinks for multiple providers. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper) |

---

## ⚙️ Configuration

### `appsettings.json`

The libraries support centralized configuration via `appsettings.json`.  
Below is a real example showing all available keys:

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "Default": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PWD;"
  },
  "HttpClientOptions": [
    {
      "name": "Test_No_RateLimit",
      "certificate": {
        "path": "YOUR_PATH",
        "password": "YOUR_PASSWORD"
      },
      "RateLimitOptions": {
        "AutoReplenishment": true,
        "PermitLimit": 1,
        "QueueLimit": 1,
        "Window": "00:00:15",
        "SegmentsPerWindow": 2,
        "IsEnabled": false
      },
      "UseMock": true
    },
    {
      "name": "Test_With_RateLimit",
      "certificate": {
        "path": "YOUR_PATH",
        "password": "YOUR_PASSWORD"
      },
      "RateLimitOptions": {
        "AutoReplenishment": true,
        "PermitLimit": 1,
        "QueueLimit": 100,
        "Window": "00:00:10",
        "SegmentsPerWindow": 1,
        "IsEnabled": true
      },
      "UseMock": true
    }
  ],
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
````

---

## 🚀 Usage

### Simple Request

```csharp
app.MapGet("Test/simple", async (IhttpsClientHelperFactory http) =>
{
    var bodyJson = """
        {"name":"Request","value":"Simple"}
    """;

    var contentBuilder = new JsonContentBuilder();
    contentBuilder.BuildContent(bodyJson);

    var client = http.CreateOrGet("Test_No_RateLimit");

    HttpResponseMessage responseMessage =
        await client.SendAsync("http://www.yoursite.com/echo", HttpMethod.Post, bodyJson, contentBuilder);

    var json = await responseMessage.Content.ReadAsStringAsync();
    return Results.Content(json, "application/json");
})
.WithSummary("Simple Request")
.WithTags("HttpHelper");
```

### With Rate Limiting

```csharp
var client = http.CreateOrGet("Test_With_RateLimit");
```

### Other Examples

* **Retry** request handling
* **Timeout** management
* **Bearer token check** demo endpoint
* **Rate limit** configuration

(All these examples are available in the included Demo project.)

---

## 🖥️ Demo Project with Scalar UI

The repository includes a **Demo project** that showcases all features using [Scalar](https://github.com/scalar/scalar), a modern OpenAPI UI that provides a cleaner alternative to Swagger.

### Why Scalar?

* Modern and clean interface.
* Dark/light theme support.
* Direct request testing from the browser.
* Clear visualization of query parameters and responses.

### Run the Demo

1. Clone the repository:

   ```bash
   git clone https://github.com/alexbypa/Csharp.Essentials.Extensions.git
   cd Csharp.Essentials.Extensions/Demo
   ```

2. Restore and run:

   ```bash
   dotnet restore
   dotnet run
   ```

3. Open Scalar UI:

   ```
   http://localhost:1234/scalar
   ```

### Screenshot

![Scalar Demo](docs/images/scalar-demo.png)

---

## 🔗 Related LoggerHelper Sinks

* `CSharpEssentials.LoggerHelper.Sink.Elasticsearch`
* `CSharpEssentials.LoggerHelper.Sink.MSSqlServer`
* `CSharpEssentials.LoggerHelper.Sink.File`
* `CSharpEssentials.LoggerHelper.Sink.Postgresql`
* `CSharpEssentials.LoggerHelper.Sink.Console`

---

## 🤝 Contributing

Contributions are welcome!
Please open issues and submit pull requests.

---

## 📄 License

MIT

```

---
