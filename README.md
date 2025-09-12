# Csharp.Essentials

A **modular helper library for .NET** projects designed to make common tasks like HTTP calls, background jobs and logging easier and more robust.  
This repository contains demonstrations and extensions for the **Csharp.Essentials** NuGet packages.

Csharp.Essentials provides several packages that can be used independently or together depending on your project’s needs:

| Package | Description | NuGet |
|---|---|---|
| **CSharpEssentials.HttpHelper** | Simplifies `HttpClient` usage with built‑in resiliency (retries/fallbacks) and rate‑limiting strategies:contentReference[oaicite:0]{index=0}. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.HttpHelper) |
| **CSharpEssentials.LoggerHelper** | Provides logging helpers that leverage Serilog sinks to trace HTTP requests and responses:contentReference[oaicite:1]{index=1}. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper) |
| **CSharpEssentials.LoggerHelper.Sink.MSSqlServer** | Adds an MSSQL sink for durable log storage. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.MSSqlServer) |

These packages are intentionally lightweight. You can add only what you need, keeping your application lean and maintainable.

---

## 📑 Table of Contents <a id='table-of-contents'></a>

- [Using HttpHelper](#using-httphelper)
- [Using LoggerHelper MSSQL](#using-loggerhelper-mssql)

## Using HttpHelper<a id='using-httphelper'></a>   [🔝](#table-of-contents)

This section explains how to install **HttpHelper** and configure your project to run the demo. Detailed Minimal API examples for each HttpHelper method will be added later.

## Demo project with Scalar UI

The repository also provides a **Demo project** that you can run locally to explore the library with [Scalar](https://github.com/scalar/scalar) — a next-generation OpenAPI/Swagger UI that offers a much clearer and more modern developer experience.

### Why Scalar?
- Cleaner interface than the traditional Swagger UI.  
- Easy testing of endpoints directly from the browser.  
- Clear display of query parameters, responses, and examples.  
- Modern dark/light theme and responsive layout.

### How to run the Demo

1. Clone the repository:
```bash
   git clone https://github.com/alexbypa/Csharp.Essentials.Extensions.git
   cd Csharp.Essentials.Extensions/Demo
````

2. Restore dependencies and run:

```bash
   dotnet restore
   dotnet run
```

3. Open Scalar UI in your browser:

```bash
   http://localhost:1234/scalar
```

You will see all available demo endpoints (e.g. **Simple Request**, **Bearer token check**, **Retry**, **Timeout**, **Rate Limit**) already documented in Scalar, ready to be tested.

---

### Screenshot

![Scalar Demo](https://github.com/alexbypa/Csharp.Essentials.Extensions/blob/main/Web.Api/docs/images/scalar-demo.png)

---

> 💡 We suggest running the Demo project to get familiar with the library before integrating it into your own solution.
> Scalar is fully integrated and automatically reflects your API endpoints without additional setup.

### Installation

Install the core packages via the .NET CLI:

```bash
# HttpHelper – resilient HTTP calls with rate limiting
 dotnet add package CSharpEssentials.HttpHelper
````

### Program.cs setup

To start the demo, you only need to register the HttpHelper clients and enable the OpenAPI + Scalar documentation. 
The Minimal API endpoints themselves will be defined elsewhere in the project.

```csharp
using CSharpEssentials.HttpHelper;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register HttpHelper clients. The configuration section `HttpClientOptions` in appsettings.json
// allows you to specify certificates, rate limit settings and more content.
// If you are not using a custom HttpMessageHandler (e.g. Moq for tests), pass null as the second argument.
builder.Services.AddHttpClients(builder.Configuration, null);
//Otwerwise use this: 
//builder.Services.AddHttpClients(builder.Configuration, HttpMocks.CreateHandler());

// Add OpenAPI document generation and Scalar UI for interactive docs
builder.Services.AddOpenApi();

var app = builder.Build();

// Expose the OpenAPI JSON and the Scalar UI
app.MapOpenApi();
app.MapScalarApiReference();

// Optional: force HTTPS redirection for local development
app.UseHttpsRedirection();

app.Run();
```

This `Program.cs` sets up:

* Registration of **HttpHelper** via `AddHttpClients`, which reads options like `PermitLimit`, `QueueLimit` and certificate settings from `HttpClientOptions` in your configuration file.
* Built in OpenAPI document generation (available at `/openapi/v1.json`) and **Scalar** as an interactive UI for exploring the API.

The actual endpoints demonstrating HttpHelper usage (GET, POST with retries, logging, etc.) will be defined in subsequent sections of this repository.

---
## Configuration with `appsettings.json`

This library is designed to work with a centralized configuration stored in `appsettings.json`.  
Below you will find an explanation of all the available sections and how they affect the behavior of the extension.

---

### Example `appsettings.json`

```json
{
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
  ]
}
````

---

### Sections explained

#### `HttpClientOptions`

A list of **preconfigured HTTP clients**. Each object represents one named client.

* **`name`**
  Identifier of the client. Used when calling `http.CreateOrGet("ClientName")`.

* **`certificate`**
  Optional certificate to be attached to the client.

  * `path`: File system path to the `.pfx` or certificate file.
  * `password`: Password protecting the certificate.

* **`RateLimitOptions`**
  Controls throttling behavior for outgoing requests.

  * `AutoReplenishment`: If `true`, permits are automatically restored at the end of each window.
  * `PermitLimit`: Maximum number of requests allowed per window segment.
  * `QueueLimit`: Maximum number of requests waiting in the queue when the limit is exceeded.
  * `Window`: Time window (e.g. `"00:00:15"` = 15 seconds).
  * `SegmentsPerWindow`: Splits the window into smaller segments for more precise limiting.
  * `IsEnabled`: Enables (`true`) or disables (`false`) rate limiting.

* **`UseMock`**
  If `true`, the client uses an internal mock/handler instead of making real HTTP calls (useful for testing).

---

## 📖 Usage Examples
> This table **lists the native methods from the `HttpHelper` package** and shows **where they are orchestrated inside the `BusinessLayer`** following SOLID separation (endpoint ➜ UseCase ➜ NetworkClient ➜ Steps ➜ native helper).
> It also includes **`CreateOrGet`**, which accepts the **option name configured in `appsettings.json`** and resolves the correct HTTP profile.

| **Native method & Purpose**                                                                                                                      | **Applied by (class/step)**                                     | **Project / Folder / File**                               |
| ------------------------------------------------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------- | --------------------------------------------------------- |
| **`CreateOrGet(string optionName)`** – resolves the configured `IhttpsClientHelper` profile from `appsettings.json`.                             | `HttpHelperNetworkClient`                                       | `BusinessLayer/Infrastructure/HttpHelperNetworkClient.cs` |
| **`addTimeout(TimeSpan)`** – applies per-request timeout from `HttpRequestSpec.Timeout`.                                                         | `TimeoutStep` *(sealed, implements `IHttpClientStep`)*          | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`addRetryCondition(Func<HttpResponseMessage,bool>, int, double)`** – sets retry policy (predicate + attempts + backoff).                       | `RetryConditionStep` *(sealed, implements `IHttpClientStep`)*   | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`setHeadersWithoutAuthorization(Dictionary<string,string>)`** – sets headers when no auth is provided.                                         | `HeadersAndBearerStep` *(sealed, implements `IHttpClientStep`)* | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`setHeadersAndBearerAuthentication(Dictionary<string,string>, httpClientAuthenticationBearer)`** – sets headers + bearer token authentication. | `HeadersAndBearerStep`                                          | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`setHeadersAndBasicAuthentication(Dictionary<string,string>, httpClientAuthenticationBasic)`** – sets headers + basic authentication.          | `HeadersAndBearerStep`                                          | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`addHeaders(string key, string value)`** – adds individual headers if needed.                                                                  | `HeadersAndBearerStep`                                          | `BusinessLayer/Contracts/IHttpClientStep.cs`              |
| **`AddRequestAction(Func<HttpRequestMessage,HttpResponseMessage,int,TimeSpan,Task>)`** – registers request/response actions (logging, metrics).  | `HttpHelperNetworkClient` (+ `IRequestAction`)                  | `BusinessLayer/Infrastructure/HttpHelperNetworkClient.cs` |
| **`ClearRequestActions()`** – clears and resets registered actions.                                                                              | `HttpHelperNetworkClient`                                       | `BusinessLayer/Infrastructure/HttpHelperNetworkClient.cs` |
| **`addFormData(List<KeyValuePair<string,string>>)`** – attaches form-data (not used in Demo, extendable).                                        | *(none in Demo)*                                                | —                                                         |
| **`SendAsync(string url, HttpMethod, IDictionary<string,string>? headers, IContentBuilder)`** – performs the actual HTTP request.                | `HttpHelperNetworkClient`                                       | `BusinessLayer/Infrastructure/HttpHelperNetworkClient.cs` |

---

**Related types (quick pointers):**

* **`IHttpClientStep`** (contract used by `TimeoutStep`, `RetryConditionStep`, `HeadersAndBearerStep`) → `BusinessLayer/Contracts/IHttpClientStep.cs`
* **`HttpRequestSpec`, `HttpAuthSpec`, `HttpResponseSpec`** → `BusinessLayer/Domain/*.cs`
* **`IRequestAction` + implementations** (`ConsoleColorRequestAction`, `InlineRequestAction`) → `BusinessLayer/Contracts/IRequestAction.cs` and `BusinessLayer/Infrastructure/*.cs`
* **`HttpHelperNetworkClient`** (applies steps, adds/clears actions, performs `SendAsync`) → `BusinessLayer/Infrastructure/HttpHelperNetworkClient.cs`
* **`FetchAndLogUseCase`** (business orchestration) → `BusinessLayer/Application/FetchAndLogUseCase.cs`

> **Recap:** Minimal API endpoint ➜ **UseCase** ➜ **HttpHelperNetworkClient** ➜ **Steps (`IHttpClientStep`)** ➜ native **`IhttpsClientHelper`** methods ➜ `SendAsync`.

### 1. Simple GET request

```csharp
app.MapGet("Test/echo", async (string httpOptionName, IhttpsClientHelperFactory httpFactory) => {
    var spec = new HttpRequestSpec {          // (1)
        Url = "http://www.yoursite.com/echo", // (2)
        Method = "POST",                      // (3)
        Body = "{'name':'Request','value':'Simple'}" // (4)
    };

    var ctxFactory = new DefaultAfterRequestContextFactory(); // (5)
    var netClient = new HttpHelperNetworkClient(httpFactory, ctxFactory, null); // (6)

    var result = await new FetchAndLogUseCase(netClient)
        .ExecuteAsync(httpOptionName, spec, new NoBodyContentBuilder()); // (7)

    return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error); // (8)
});
```

**Explanation**

1. **`HttpRequestSpec`** – defines the request contract (URL, method, body). It’s the DTO passed through the whole pipeline.
2. **`Url`** – target endpoint for the call (`/echo` in this sample).
3. **`Method`** – HTTP verb to use.
4. **`Body`** – raw request payload.
5. **`DefaultAfterRequestContextFactory`** – builds the context executed after each request (used for logging, metrics, tracing).
6. **`HttpHelperNetworkClient`** – orchestrator that knows how to execute the spec using the factory and context factory.
7. **`FetchAndLogUseCase`** – application-level use case that executes the request, handles logging, and returns a clean `Result`.
8. Returns either the upstream value (`200 OK`) or a Problem detail if there’s an error.

---
## 🧩 Supported Content Builders

* `JsonContentBuilder` → for `application/json`
* `FormUrlEncodedContentBuilder` → for form data
* `XmlContentBuilder` → for `application/xml`
* `NoBodyContentBuilder` → for `GET / DELETE`

---

### 2. Request actions and retry logic

```csharp
var client = factory.CreateOrGet("WithRetry")
    .AddRequestAction((req, res, retry, ts) => {
        Console.WriteLine($"[{DateTime.Now}] Status: {res.StatusCode}");
        return Task.CompletedTask;
    })
    .addRetryCondition(
        res => res.StatusCode == HttpStatusCode.InternalServerError,
        maxRetries: 3,
        delayInSeconds: 0.5
    );

HttpResponseMessage response = await client.SendAsync(
    "https://yoursite.com/retry",
    HttpMethod.Get,
    null,
    contentBuilder);
```

---

### 3. Authentication with Bearer Token

The method `setHeadersAndBearerAuthentication` allows you to fluently attach both custom headers and a Bearer authentication token to your requests.

```csharp
var client = factory.CreateOrGet("WithAuth")
    .setHeadersAndBearerAuthentication(
        new Dictionary<string, string>
        {
            { "X-Correlation-Id", Guid.NewGuid().ToString() }
        },
        new httpsClientHelper.httpClientAuthenticationBearer("super-secret-token")
    );

HttpResponseMessage response = await client.SendAsync(
    "https://yoursite.com/secure/data",
    HttpMethod.Get,
    null,
    contentBuilder);

string result = await response.Content.ReadAsStringAsync();
Console.WriteLine(result);
```

---

## 🛠️ Built-in Features

| Feature         | Description                                |
| --------------- | ------------------------------------------ |
| Retry           | Polly-based retry with exponential backoff |
| Rate Limiting   | Sliding window limiter per client instance |
| Headers/Auth    | Bearer / Basic / Custom headers            |
| Logging Handler | Custom DelegatingHandler logs all requests |
| Retry Info      | Injects `X-Retry-Attempt` and duration     |

---

## 📂 Folder Structure

* **httpsClientHelper.cs** → main engine
* **httpsClientHelperFactory.cs** → factory + DI integration
* **HttpRequestBuilder.cs** → fluent builder pattern
* **IContentBuilder.cs** → pluggable request body strategies
* **HttpClientHandlerLogging.cs** → optional delegating handler
* **httpClientOptions.cs** → config-based client tuning

---

## 🏷️ Notes

* You can combine `setHeadersAndBearerAuthentication` with other fluent APIs like `AddRequestAction`, `addTimeout`, and `addRetryCondition`.
* The first parameter (`Dictionary<string, string>`) allows you to inject any custom headers.
* The second parameter (`httpClientAuthenticationBearer`) automatically adds the `Authorization: Bearer ...` header.


---
## Using LoggerHelper MSSQL<a id='using-loggerhelper-mssql'></a>   [🔝](#table-of-contents)
                                  
---
## 🤝 Contributing

Contributions, issues, and feature requests are welcome!
Feel free to open a [pull request](https://github.com/alexbypa/CSharpEssentials.HttpHelper/pulls) or [issue](https://github.com/alexbypa/CSharpEssentials.HttpHelper/issues).

---

## 📜 License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

