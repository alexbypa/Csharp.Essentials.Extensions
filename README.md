# 📦 Csharp.Essentials

﻿﻿[![Frameworks](https://img.shields.io/badge/.NET-6.0%20%7C%208.0%20%7C%209.0-blue)](https://dotnet.microsoft.com/en-us/download)
[![CodeQL](https://github.com/alexbypa/CSharp.Essentials/actions/workflows/codeqlLogger.yml/badge.svg)](https://github.com/alexbypa/CSharp.Essentials/actions/workflows/codeqlLogger.yml)
[![NuGet](https://img.shields.io/nuget/v/CSharpEssentials.LoggerHelper.svg)](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper)
[![Downloads](https://img.shields.io/nuget/dt/CSharpEssentials.LoggerHelper.svg)](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper)
![Last Commit](https://img.shields.io/github/last-commit/alexbypa/CSharp.Essentials?style=flat-square)

A **modular helper library for .NET** projects designed to make common tasks like HTTP calls, background jobs and logging easier and more robust.  
This repository contains demonstrations and extensions for the **Csharp.Essentials** NuGet packages.

Csharp.Essentials provides several packages that can be used independently or together depending on your project’s needs:

| Package | Description | NuGet |
|---|---|---|
| **CSharpEssentials.HttpHelper** | Simplifies `HttpClient` usage with built‑in resiliency (retries/fallbacks) and rate‑limiting strategies. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.HttpHelper) |
| **CSharpEssentials.LoggerHelper** | Provides logging helpers that leverage Serilog sinks to trace HTTP requests and responses:contentReference. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper) |
| **CSharpEssentials.LoggerHelper.Sink.Email** | Perfect for real-time critical alerts, with full HTML template customization, configurable throttling, and secure SMTP (SSL/TLS) delivery. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.Email) |
| **CSharpEssentials.LoggerHelper.Sink.xUnit** | Streamlines integration testing by forwarding application logs directly into the xUnit test output, perfect for debugging in CI/CD pipelines. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.xUnit) |
| **CSharpEssentials.LoggerHelper.Sink.Telegram** | Delivers instant log notifications to your Telegram chat or group using pure HTTP, with a clean and user-friendly message format for real-time monitoring of critical events. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.Postgresql) |
| **CSharpEssentials.LoggerHelper.Sink.Postgresql** | Stores structured logs directly into PostgreSQL with support for custom schemas, JSON fields, and automatic table creation for deep analytics and long-term storage. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.Postgresql) |
| **CSharpEssentials.LoggerHelper.Sink.MSSqlServer** | A powerful SQL Server sink for CSharpEssentials.LoggerHelper, designed to log directly into Microsoft SQL Server with automatic table creation, custom columns, and structured context properties. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.MSSqlServer) |
| **CSharpEssentials.LoggerHelper.Sink.Elasticsearch** | A high-performance Elasticsearch sink for CSharpEssentials.LoggerHelper, designed to index logs into Elasticsearch for fast search, advanced filtering, and real-time dashboards with Kibana. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.Elasticsearch) |
| **CSharpEssentials.LoggerHelper.Telemetry** | A full OpenTelemetry sink for CSharpEssentials.LoggerHelper, enabling metrics, traces, and logs with automatic database storage for end-to-end observability. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.AI) |
| **CSharpEssentials.LoggerHelper.AI** | Advanced OpenTelemetry sink and scalable gateway for LLM. Enables querying of language models using a fully configurable log context, optimizing AI insight extraction for end-to-end observability. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.AI) |
| **CSharpEssentials.LoggerHelper.Dashboard** | An embedded dashboard for CSharpEssentials.LoggerHelper, giving you real-time visibility into how sinks are loaded, which log levels are enabled, and any initialization errors — all from inside your application. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Dashboard) |

These packages are intentionally lightweight. You can add only what you need, keeping your application lean and maintainable.

---

## 📑 Table of Contents <a id='table-of-contents'></a>

- [🌐Using HttpHelper](#using-httphelper)
- [📘HttpLogger - Introduction](#introduction)
  - [🚀HttpLogger - Installation](#installation)
  - [🔧HttpLogger - Configuration](#configuration)
  - [📨 HTML Email Sink (used with System.Net.smtp)](#html-email-sink)
  - [🧪 xUnit Sink](#xunit-sink)
  - [📣 Telegram Sink (used with HttpClient)](#telegram-sink)
  - 🐘[PostgreSQL Sink](#postgresql-sink)
  - [💾 MS SQL Sink](#ms-sql-sink)
  - [🔍 ElasticSearch Sink](#elasticsearch)
  - [📊 Telemetry Sink](#telemetry)
  - [🕵️ AI](#logger-ai)
  - [📘 Dashboard](#dashboard)
  - [🔍 Extending LogEvent Properties](#customprop)
- [🧪 Demo API](#demo-api)
- [📜 Version History](#versions)

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

## 🏷️ Notes

* You can combine `setHeadersAndBearerAuthentication` with other fluent APIs like `AddRequestAction`, `addTimeout`, and `addRetryCondition`.
* The first parameter (`Dictionary<string, string>`) allows you to inject any custom headers.
* The second parameter (`httpClientAuthenticationBearer`) automatically adds the `Authorization: Bearer ...` header.                               

---
## 📘 Introduction<a id='introduction'></a>   [🔝](#table-of-contents)

🚀 **CSharpEssentials.LoggerHelper** is a flexible and modular structured logging library for .NET 6/8/9. It’s powered by Serilog for most sinks, and extended with native support for Telegram (via `HttpClient`) and Email (via `System.Net.Mail`).

⚠️ **Note**: The built-in Serilog Email Sink is currently affected by a blocking issue ([#44](https://github.com/serilog/serilog-sinks-email/issues/44)), so `CSharpEssentials.LoggerHelper` uses `System.Net.Mail` instead for full control and reliability in production.

🧩 Each sink is delivered as an independent NuGet sub-package and dynamically loaded at runtime, acting as a routing hub that writes each log event to a given sink only if the event’s level matches that sink’s configured level (see **Configuration**).

📦 Centralized and intuitive configuration via a single `appsettings.LoggerHelper.json` file with built-in placeholder validation.

🔧 Supports rich structured logs with properties like `IdTransaction`, `ApplicationName`, `MachineName`, and `Action`.

🐞 **Automatically captures both the latest error** (`CurrentError`) **and all initialization errors** in a concurrent `Errors` queue, so you can inspect the single “last” failure or enumerate the full list programmatically, expose them via HTTP headers, logs, etc.  
🔜 **Roadmap:** in the next release we’ll ship a dedicated dashboard package (`CSharpEssentials.LoggerHelper.Dashboard`) to visualize these errors (and your traces/metrics) without ever touching your code.

🔧 Designed for extensibility with plugin support, level-based sink routing, Serilog SelfLog integration, and a safe debug mode.

### 📦 Available Sink Packages


* **Console**: `CSharpEssentials.LoggerHelper.Sink.Console`
* **File**: `CSharpEssentials.LoggerHelper.Sink.File`
* **MSSqlServer**: `CSharpEssentials.LoggerHelper.Sink.MSSqlServer`
* **PostgreSQL**: `CSharpEssentials.LoggerHelper.Sink.PostgreSql`
* **ElasticSearch**: `CSharpEssentials.LoggerHelper.Sink.Elasticsearch`
* **Telegram**: `CSharpEssentials.LoggerHelper.Sink.Telegram` *Used via `HttpClient`*
* **Email**: `CSharpEssentials.LoggerHelper.Sink.Email` *Used via `System.Net.Mail`*

---

## 🚀 Installation <a id='installation'></a>    [🔝](#table-of-contents)
```bash
dotnet add package CSharpEssentials.LoggerHelper
```

```csharp
#if NET6_0
    builder.AddLoggerConfiguration();
#else
    builder.Services.AddLoggerConfiguration(builder);
#endif

// ───────────────────────────────────────────────────────────────
// 🔧 **Register your custom context enricher**:
// This tells LoggerHelper to invoke your `MyCustomEnricher` on every log call,
// so you can inject properties from the ambient context (e.g. controller action,
// HTTP request, user identity, IP address, etc.)
builder.Services.AddSingleton<IContextLogEnricher, MyCustomEnricher>();
// ───────────────────────────────────────────────────────────────

// Optionally enable HTTP middleware logging
app.UseMiddleware<RequestResponseLoggingMiddleware>();
```

## 🔧 Configuration <a id='configuration'></a>    [🔝](#table-of-contents)

### Verifying LoggerHelper Initialization in Your Minimal API Endpoint

After registering LoggerHelper in your pipeline, you can trigger sink loading and check for any initialization errors right in your endpoint handler:

> **Note:**  
> `LoggerRequest` is a custom class that **must** implement `IRequest`.  
> It provides the default log properties:
> - `IdTransaction`  
> - `Action`  
> - `ApplicationName`  
>
> You can extend it with any additional fields you need, e.g. `UserLogged`,`IpAddress`, etc. see : [✨<strong>Extending LogEvent Properties</strong>✨](#customprop)

```csharp
app.MapGet("/users/sync", async ([FromQuery] int page, IUserService service) =>
{
    // 1) Trigger sink loading and log startup event
    loggerExtension<IRequest>.TraceSync(new LoggerRequest(), Serilog.Events.LogEventLevel.Information, null, "Loaded LoggerHelper");

    // 2) Check for a global initialization error
    if (!string.IsNullOrEmpty(LoggerExtension<IRequest>.CurrentError))
    {
        return Results.BadRequest(LoggerExtension<IRequest>.CurrentError);
    }

    // 3) Check for per-sink initialization failures
    if (LoggerExtension<IRequest>.Errors?.Any() == true)
    {
        var details = LoggerExtension<IRequest>.Errors
            .Select(e => $"{e.SinkName}: {e.ErrorMessage}");
        return Results.BadRequest(string.Join("; ", details));
    }

    // 4) Proceed with business logic if all sinks initialized successfully
    var users = await service.SyncUsersAsync(page);
    return Results.Ok(users);
})
.WithName("SyncUsers")
.Produces<List<User>>(StatusCodes.Status200OK);
```

Here’s a **Minimal Configuration Example** that uses **only** the File sink and writes **all** log levels (`Information`, `Warning`, `Error`, `Fatal`):

you need to create **appsettings.LoggerHelper.json** in your project ( on development environment create with the name **appsettings.LoggerHelper.debug.json** )
```json
{
  "Serilog": {
    "SerilogConfiguration": {
      "ApplicationName": "DemoLogger 9.0",
      "SerilogCondition": [
        {
          "Sink": "File",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        }
      ]
    },
    "SerilogOption": {
      "File": {
        "Path": "C:\\Logs\\DemoLogger",
        "RollingInterval": "Day",
        "RetainedFileCountLimit": 7,
        "Shared": true
      }
    }
  }
}
```

* **SerilogConfiguration.ApplicationName**: your app’s name.
* **SerilogCondition**: a list of sink-level mappings; here we map **all** levels to the `"File"` sink.
* **SerilogOption.File**: settings specific to the File sink (output folder, rolling interval, retention, etc.).

---

## 🧪 xUnit Sink <a id='xunit-sink'></a>   [🔝](#table-of-contents)

This sink is useful when running integration tests, especially in CI/CD pipelines like Azure DevOps, where access to external sinks (e.g., databases, emails) might be restricted or blocked.

#### 1️⃣ Add Packages

Install the core logger and the specific xUnit sink package:

```bash
dotnet add package CSharpEssentials.LoggerHelper
dotnet add package CSharpEssentials.LoggerHelper.Sink.xUnit
```

#### 2️⃣ Configure Serilog for xUnit output

In your `appsettings.loggerhelper.json`, add the following to enable log forwarding to the xUnit output stream:

```json
{
  "Serilog": {
    "SerilogConfiguration": {
      "SerilogCondition": [
        {
          "Sink": "xUnit",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        }
      ]
    }
  }
}
```

#### 3️⃣ Register the xUnit output inside your test

In your test class (e.g. integration or endpoint tests), you **must** register the xUnit output sink **explicitly**:

> ⚠️ Without calling `XUnitTestOutputHelperStore.SetOutput(...)`, no log will appear in the test output — even if everything else is configured correctly.

```csharp
public class MinimalEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public MinimalEndpointTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;

        // 🚨 REQUIRED: This links the logger to xUnit's output stream
        XUnitTestOutputHelperStore.SetOutput(output);
    }

    [Fact]
    public async Task login_timeout()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/your-api-endpoint");
        response.EnsureSuccessStatusCode();
    }
}
```

#### ✅ Where to use `loggerExtension<T>`?

You **do not need to call `loggerExtension<T>` inside the test**.

Instead, you call it **inside your Web API project or any class library it uses** — for example, in a service class or middleware:

```csharp
public async Task<bool> AuthenticateUserAsync(string username)
{
    var request = new ProviderRequest { Step = "AuthenticateUser" };
    await loggerExtension<ProviderRequest>.LogInformationAsync(request, $"Authenticating {username}");
    ...
}
```

Thanks to the configuration, **if a test triggers this business logic**, the logs will automatically appear in the **test output**, making debugging much easier — even when external sinks are not available.

## 📨 HTML Email Sink<a id='html-email-sink'></a>   [🔝](#table-of-contents)
---

## ⚠️ Version 2.0.0 - Breaking Change

> Starting from version **2.0.0**, the `Email` configuration section has been **renamed**.
>
> If you are upgrading from `1.x.x`, you MUST update your `appsettings.LoggerHelper.json`.

Old (before 2.0.0):

```json
"Email": {
  "From": "...",
  "Host": "...",
  "Port": 587,
  "To": ["..."],
  "CredentialHost": "...",
  "CredentialPassword": "..."
}
```

New (since 2.0.0):

```json
"Email": {
  "From": "...",
  "Host": "...",
  "Port": 587,
  "To": "...",
  "username": "...",
  "password": "...",
  "EnableSsl": true,
  "TemplatePath": "Templates/email-template-default.html",
  "ThrottleInterval": "00:01:00"
}
```

`ThrottleInterval` is a simple but powerful mechanism that prevents the same sink from sending messages too frequently.  
It’s ideal for external services like **Telegram bots** or **SMTP servers**, which often have **rate limits** (e.g., HTTP 429 Too Many Requests).
### ✅ How it works

- Define a time interval (e.g., 10 seconds)
- The sink will **skip** any log messages emitted within that time frame
- Clean, configurable, and fully automatic

## 🚨 Why Email Handling Changed

Starting from version 2.0.0, LoggerHelper **no longer uses** the standard [Serilog.Sinks.Email](https://github.com/serilog/serilog-sinks-email) for sending emails.

**Reason:**
The official Serilog Email Sink does not support custom body formatting (HTML templates, structured logs, color coding, etc).
It only supports plain text messages generated via `RenderMessage()`, without the ability to control the message content.

🔎 See discussion: [GitHub Issue - serilog/serilog-sinks-email](https://github.com/serilog/serilog-sinks-email/issues/44)

**What changed:**

* LoggerHelper now uses a **custom internal SMTP sink**: `LoggerHelperEmailSink`.
* This allows sending fully customized **HTML-formatted emails**.
* Supports dynamic coloring based on log level (Information, Warning, Error).
* Supports secure SMTP with SSL/TLS.

✅ No third-party dependencies added.
✅ Full control over email appearance and content.

Since v2.0.0, LoggerHelper no longer uses `Serilog.Sinks.Email`. It ships with `LoggerHelperEmailSink`, allowing:

* ✅ Full HTML customization via external template
* ✅ Dynamic styling based on log level
* ✅ Secure SMTP (SSL/TLS)

Example HTML placeholders:

```html
{{Timestamp}}, {{Level}}, {{Message}}, {{Action}}, {{IdTransaction}}, {{MachineName}}, {{ApplicationName}}, {{LevelClass}}
```

### 🖌️ Email Template Customization (optional)

LoggerHelper allows you to customize the **HTML structure and appearance** of the email body.
You can provide an external `.html` file with placeholders like:

```html
{{Timestamp}}, {{Level}}, {{Message}}, {{Action}}, {{IdTransaction}}, {{MachineName}}, {{ApplicationName}}, {{LevelClass}}
```

Then, in the `appsettings.LoggerHelper.json` configuration file, set:

```json
"LoggerHelper": {
  "SerilogOption": {
    "Email": {
      ...
      "TemplatePath": "Templates/email-template-default.html"
    }
  }
}
```

If the file is missing or invalid, LoggerHelper will **fall back to the internal default template**, ensuring backward compatibility.
> 📸 Example of a formatted email message:
> ![Email Sample](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper/img/emailsample.png)


### 🔧 File + Email sink example configuration

This configuration writes **every** log event (`Information`, `Warning`, `Error`, `Fatal`) to the **File** sink, but only sends **Email** notifications for high-severity events (`Error` and `Fatal`):

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Debug",
        "System": "Debug"
      }
    },
    "SerilogConfiguration": {
      "ApplicationName": "DemoLogger 9.0",
      "SerilogCondition": [
        {
          "Sink": "File",
          "Level": [ "Information", "Warning", "Error", "Fatal" ]
        },
        {
          "Sink": "Email",
          "Level": [ "Error", "Fatal" ]
        }
      ],
      "SerilogOption": {
        "File": {
          "Path": "C:\\Logs\\DemoLogger",
          "RollingInterval": "Day",
          "RetainedFileCountLimit": 7,
          "Shared": true
        },
        "Email": {
          "From": "jobscheduler.pixelo@gmail.com",
          "Port": 587,
          "Host": "smtp.gmail.com",
          "To": "ops-team@example.com",
          "Username": "alerts@example.com",
          "Password": "YOUR_SMTP_PASSWORD",
          "EnableSsl": true,
          "TemplatePath": "Templates/email-template-default.html"
        }
      }
    }
  }
}
```

## 📣 Telegram Sink<a id='telegram-sink'></a>   [🔝](#table-of-contents)
LoggerHelper supports Telegram notifications to alert on critical events.

> ⚠️ **Recommended Levels**: Use only `Error` or `Fatal` to avoid exceeding Telegram rate limits.

### 🔧 Telegram sink example configuration


```json
"TelegramOption": {
  "chatId": "<YOUR_CHAT_ID>",
  "Api_Key": "<YOUR_BOT_API_KEY>",
  "ThrottleInterval":"00:00:45"
}
```
`ThrottleInterval` is a simple but powerful mechanism that prevents the same sink from sending messages too frequently.  
It’s ideal for external services like **Telegram bots** or **SMTP servers**, which often have **rate limits** (e.g., HTTP 429 Too Many Requests).
### ✅ How it works

- Define a time interval (e.g., 10 seconds)
- The sink will **skip** any log messages emitted within that time frame
- Clean, configurable, and fully automatic

To configure a Telegram Bot:

1. Open Telegram and search for [@BotFather](https://t.me/BotFather)
2. Create a new bot and copy the API token
3. Use [https://api.telegram.org/bot<YourBotToken>/getUpdates](https://core.telegram.org/bots/api#getupdates) to get your chat ID after sending a message to the bot

> 📸 Example of a formatted Telegram message:
> ![Telegram Sample](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper/img/telegramSample.png)

### File + Email + Telegram Sink Example Configuration
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Debug",
        "System": "Debug"
      }
    },
    "SerilogConfiguration": {
      "ApplicationName": "DemoLogger 9.0",
      "SerilogCondition": [
        {
          "Sink": "File",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "Email",
          "Level": [
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "Telegram",
          "Level": [
            "Error",
            "Fatal"
          ]
        }
      ],
      "SerilogOption": {
        "File": {
          "Path": "C:\\Logs\\DemoLogger",
          "RollingInterval": "Day",
          "RetainedFileCountLimit": 7,
          "Shared": true
        },
        "Email": {
          "From": "jobscheduler.pixelo@gmail.com",
          "Port": 587,
          "Host": "smtp.gmail.com",
          "To": "alexbypa@gmail.com",
          "username": "------",
          "password": "-------------",
          "EnableSsl": true,
          "TemplatePath": "Templates/email-template-default.html"
        },
        "TelegramOption": {
          "chatId": "xxxxxxxxxxx",
          "Api_Key": "wwwwwwwwww:zxxxxxxxxzzzzz"
        }
      }
    }
  }
}
```

## 🐘 PostgreSQL Sink<a id='postgresql-sink'></a>   [🔝](#table-of-contents)

LoggerHelper supports logging to PostgreSQL with optional custom schema definition.

* If `ColumnsPostGreSQL` is **not set**, the following default columns will be created and used:

  * `message`, `message_template`, `level`, `raise_date`, `exception`, `properties`, `props_test`, `machine_name`
* If `ColumnsPostGreSQL` is defined, LoggerHelper will use the exact fields provided.
* Setting `addAutoIncrementColumn: true` will add an `id SERIAL PRIMARY KEY` automatically.

Example configuration:

```json
"PostgreSQL": {
  "connectionString": "...",
  "tableName": "LogEntry",
  "schemaName": "public",
  "addAutoIncrementColumn": true,
  "ColumnsPostGreSQL": [
    { "Name": "Message", "Writer": "Rendered", "Type": "text" },
    { "Name": "Level", "Writer": "Level", "Type": "varchar" }
  ]
}
```
## 🧪 PostgreSQL Table Structure

If custom `ColumnsPostGreSQL` is defined, logs will include all specified fields.


> 🧩 Tip: PostgreSQL sink is ideal for deep analytics and long-term log storage.
> ⚠️ **Note:** When using `ColumnsPostGreSQL`, always enable `SelfLog` during development to detect unsupported or misconfigured column definitions. Invalid types or property names will be silently ignored unless explicitly logged via Serilog’s internal diagnostics.


---
## 💾 MS SQL Sink<a id='ms-sql-sink'></a>    [🔝](#table-of-contents)
This sink writes logs to a Microsoft SQL Server table and supports additional context properties out of the box.

### 📦 Changes

✅ **Version 2.0.8**  
* Added **complete management of standard columns** for the MSSQL sink (`standardColumns` option).
* Introduced the new **`additionalColumns`** array, which by default includes the base fields of the package:

  * `IdTransaction`
  * `Action`
  * `MachineName`
  * `ApplicationName`


### Configuration Example

```json
"MSSqlServer": {
  "connectionString": "<YOUR CONNECTIONSTRING>",
  "sinkOptionsSection": {
    "tableName": "logs",
    "schemaName": "dbo",
    "autoCreateSqlTable": true,
    "batchPostingLimit": 100,
    "period": "0.00:00:10"
  },
  "columnOptionsSection": {
    "addStandardColumns": [
      "LogEvent"
    ],
    "removeStandardColumns": [
      "Properties"
    ]
  }
}
```

### Explanation

* `connectionString`: Full connection string to the SQL Server instance.
* `tableName`: Name of the table that will receive log entries.
* `schemaName`: Schema to use for the log table (default is `dbo`).
* `autoCreateSqlTable`: If true, the log table will be created automatically if it does not exist.
* `batchPostingLimit`: Number of log events to post in each batch.
* `period`: Interval for batching log posts.
* `addStandardColumns`: Additional default Serilog columns to include (e.g., `LogEvent`).
* `removeStandardColumns`: Columns to exclude from the standard set.

### Additional Columns

This sink automatically adds the following custom fields to each log:

* `IdTransaction`: a unique identifier for tracking a transaction.
* `MachineName`: name of the server or machine.
* `Action`: custom action tag if set via `Request.Action`.
* `ApplicationName`: name of the application logging the message.

---
## 🔍 ElasticSearch Sink<a id='elasticsearch'></a>   [🔝](#table-of-contents)

ElasticSearch is ideal for indexing and searching logs at scale. When integrated with **Kibana**, it enables advanced analytics and visualization of log data.

### Benefits

* 🔎 Fast full-text search and filtering
* 📊 Seamless integration with Kibana for dashboards
* 📁 Efficient storage and querying for large volumes of structured logs

### Example Configuration

```json
"ElasticSearch": {
  "nodeUris": "http://<YOUR_IP>:9200",
  "indexFormat": "<YOUR_INDEX>"
}
```

* `nodeUris`: The ElasticSearch node endpoint.
* `indexFormat`: The format or name of the index that will store log entries.

---

## 📊 LoggerHelper.Telemetry <a id='telemetry'></a>   [🔝](#table-of-contents)

The **CSharpEssentials.LoggerHelper.Telemetry** package extends LoggerHelper with full OpenTelemetry support for metrics and traces.

## Installation

You can install the package directly from NuGet:

```bash
PM> Install-Package CSharpEssentials.LoggerHelper.Telemetry
```

Or from the [NuGet Gallery](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Telemetry).

## Usage

In your `Program.cs`, register the telemetry service with:

```csharp
builder.Services.AddLoggerTelemetry(builder);
```

This enables LoggerHelper to export logs, metrics, and traces via OpenTelemetry.

When the package starts, the database will be **automatically configured** and the required tables will be created based on the provider defined in your `appsettings.json`.

## Supported Providers

* **SQL Server**
* **PostgreSQL**

## Configuration

```json
"LoggerTelemetryOptions": {
  "Provider": "SqlServer", // or "PostgreSQL"
  "IsEnabled": true,
  "ConnectionString": "YOUR CONNECTION STRING",
  "MeterListenerIsEnabled": true,
  "CustomExporter": {
    "exportIntervalMilliseconds": 20000,
    "exportTimeoutMilliseconds": 30000
  }
}
```

## Database Schema

The following tables are automatically generated depending on the provider:

* `MetricEntry` → stores metrics (Name, Value, Timestamp, TagsJson, TraceId).
* `TraceEntry` → stores traces/spans (TraceId, SpanId, StartTime, EndTime, DurationMs, TagsJson, etc.).
* `LogEntry` → stores log events (Message, Level, Exception, TraceId, Timestamp, etc.).

> 📝 Note
>
> * Table names and schema are **static by design** and follow the **default schema of the provider** (e.g. `dbo` for SQL Server, `public` for PostgreSQL).
> * Tables are automatically created the first time telemetry export starts.
> * No manual migrations are required.

---

Hai ragione: il README diventa molto più convincente se oltre a mostrare **come dichiarare** le metriche, fai vedere anche **come usarle realmente** dentro un endpoint.
Ti propongo una versione aggiornata della sezione, dove includiamo anche il frammento con le chiamate a `ParallelTelemetry.IncActive()`, `ParallelTelemetry.IncTotal()`, `ParallelTelemetry.DecActive()`.

---


# 🔗 Correlation between Logs, Traces and Metrics

Logs and traces are automatically correlated using a common `IdTransaction`.
This makes it easy to navigate from a **trace span** to the corresponding **logs** and vice versa.

```csharp
using var trace = LoggerExtensionWithMetrics<RequestSample>.TraceAsync(
    request,
    Serilog.Events.LogEventLevel.Information,
    null,
    "Minimal API call"
)
.StartActivity("getUserInfo")
.AddTag("Minimal API", "GV"); // span tagging
```

✅ Every log emitted inside this activity shares the same `IdTransaction`, enabling end-to-end correlation.

---

# ⚡ Usage Example with [CSharpEssentials.HttpHelper](https://www.nuget.org/packages/CSharpEssentials.HttpHelper)

This example shows how to use telemetry with **HTTP calls**, correlated with traces and logs.

```csharp
public async Task<IResult> getUserInfo(
    [FromQuery] string UserID,
    [FromQuery] string Token,
    [FromServices] IHttptsClientHelperFactory httpFactory)
{
    RequestSample request = new RequestSample
    {
        Action = "getUserInfo",
        UserID = UserID,
        Token = Token
    };

    using var trace = LoggerExtensionWithMetrics<RequestSample>.TraceAsync(
        request,
        Serilog.Events.LogEventLevel.Information,
        null,
        "Chiamata a minimal API"
    )
    .StartActivity("getUserInfo")
    .AddTag("Minimal API", "GV");

    var verifyTokenResponseHandler = new VerifyTokenResponseHandler<RequestSample>(request, httpFactory);
    var userInfoResponseHandler = new UserInfoResponseHandler<RequestSample>(request, httpFactory);
    await verifyTokenResponseHandler.SetNext(userInfoResponseHandler);

    var result = await verifyTokenResponseHandler.HandleResponse(
        "Test_No_RateLimit",
        httpFactory,
        new ResponseContext(request),
        new BusinessLayerContext
        {
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
        "Esecuzione completata con risposta {res}"
    );

    return result.Success ? Results.Ok(result.Value) : Results.Problem(result.Error);
}
```

✅ With this setup:

* Each request starts a **trace activity**.
* Tags enrich the span with context.
* Logs and traces are automatically correlated through `IdTransaction`.

### Custom Metrics with `GaugeWrapper` + sending to Kibana/Elasticsearch

This section shows how to:

1. define custom metrics with a `CustomMetrics` class using `GaugeWrapper`;
2. use those metrics inside endpoints and send them automatically to Kibana/Elasticsearch via `LoggerHelper.Telemetry` (through `MetricsEnricher`).

> Note: you don’t need to use `.NET System.Diagnostics.Metrics` directly – everything is handled by the `CSharpEssentials.LoggerHelper.Telemetry` package.

---

### Packages

```
dotnet add package CSharpEssentials.LoggerHelper
dotnet add package CSharpEssentials.LoggerHelper.Telemetry
dotnet add package CSharpEssentials.LoggerHelper.Sink.Elasticsearch
```

---

### `CustomMetrics` class (with `GaugeWrapper`)

A minimal example exposing two “live” gauges and latency/success tracking:

```csharp
using System.Collections.Concurrent;
using CSharpEssentials.LoggerHelper.Telemetry.Metrics;

public sealed class CustomMetrics
{
    private int _active;           
    private long _totalRuns;       
    private readonly ConcurrentQueue<int> _latencies = new();

    private readonly GaugeWrapper<int> _activeGauge;
    private readonly GaugeWrapper<long> _totalGauge;

    private long _ok;               
    private long _count;            
    private readonly int _thresholdMs;

    public CustomMetrics(int thresholdMs = 100)
    {
        _thresholdMs = thresholdMs;

        _activeGauge = new GaugeWrapper<int>(
            name: "telemetry.parallel_active_requests",
            valueProvider: () => Volatile.Read(ref _active),
            unit: "count",
            description: "Current active parallel tasks");

        _totalGauge = new GaugeWrapper<long>(
            name: "telemetry.parallel_total_runs",
            valueProvider: () => Interlocked.Read(ref _totalRuns),
            unit: "count",
            description: "Total parallel runs since app start");
    }

    public void IncActive() => Interlocked.Increment(ref _active);
    public void DecActive() => Interlocked.Decrement(ref _active);
    public void IncTotal()  => Interlocked.Increment(ref _totalRuns);

    public void Track(int statusCode, int elapsedMs)
    {
        Interlocked.Increment(ref _count);
        if (statusCode is >= 200 and < 300) Interlocked.Increment(ref _ok);

        _latencies.Enqueue(elapsedMs);
        while (_latencies.Count > 10_000 && _latencies.TryDequeue(out _)) { }
    }

    public (int qosUnderThreshold, double successRatePct, int p95Ms, long count, int thresholdMs) Snapshot()
    {
        var arr = _latencies.ToArray();
        Array.Sort(arr);
        int p95 = arr.Length == 0 ? 0 : arr[(int)Math.Clamp(Math.Ceiling(arr.Length * 0.95) - 1, 0, arr.Length - 1)];
        double okPct = _count == 0 ? 0 : Math.Round((double)_ok / _count * 100, 2);
        int qos = arr.Count(v => v <= _thresholdMs);
        return (qos, okPct, p95, _count, _thresholdMs);
    }
}
```

---

### Integration in `Program.cs`

`LoggerHelper.Telemetry` enriches logs with metrics via `MetricsEnricher`. Register it in DI and use `loggerExtension<T>`:

```csharp
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IContextLogEnricher, MetricsEnricher>();
builder.Services.AddSingleton<CustomMetrics>();

var app = builder.Build();
app.Run();
```

---

### Using metrics in endpoints

Simple example: measure latency, track result, and optionally update gauges for parallel runs.

```csharp
app.MapGet("Telemetry/Simple", async (CustomMetrics metrics) =>
{
    var sw = System.Diagnostics.Stopwatch.StartNew();

    // ... business logic / external call ...
    int statusCode = 200;

    sw.Stop();
    metrics.Track(statusCode, (int)sw.ElapsedMilliseconds);

    return Results.Ok(new { ok = true });
});

app.MapGet("Telemetry/Parallel", async (CustomMetrics metrics) =>
{
    metrics.IncActive();
    metrics.IncTotal();
    try
    {
        await Task.Delay(50);
        metrics.Track(200, 50);
        return Results.Ok();
    }
    finally
    {
        metrics.DecActive();
    }
});
```

---

### Emitting “metric” events to Elasticsearch

Sending metrics to logs (and Elasticsearch) is a **built-in behavior** of `LoggerHelper.Telemetry`.
To emit a structured payload (e.g. QoS snapshot), use `loggerExtension<T>` in the correct format supported by `MetricsEnricher`:

```csharp
public sealed class RequestSample
{
    public string? Action { get; set; }
}

app.MapGet("Telemetry/QoS", (CustomMetrics metrics, IConfiguration cfg) =>
{
    var snap = metrics.Snapshot();

    var evt = new
    {
        ApplicationName   = cfg["Service:Name"] ?? "my-service",
        Environment       = cfg["Service:Environment"] ?? "dev",
        QosUnderThreshold = snap.qosUnderThreshold,
        SuccessRatePct    = snap.successRatePct,
        P95Ms             = snap.p95Ms,
        Count             = snap.count,
        ThresholdMs       = snap.thresholdMs
    };

    loggerExtension<RequestSample>.TraceAsync(
        new RequestSample { Action = "QoS_Snapshot" },
        Serilog.Events.LogEventLevel.Information,
        null,
        "metric {@qos}",
        evt);

    return Results.Json(snap);
});
```

> No need to build raw objects with `MetricName/Value/Unit/Kind`. `MetricsEnricher` takes care of serializing metrics in the expected format and sending them to all configured sinks, including Elasticsearch.
---

---
## 🕵️ CSharpEssentials.LoggerHelper.AI <a id='logger-ai'></a>   [🔝](#table-of-contents)
>Advanced AI Analysis Toolkit (RAG, Correlate Trace, Summarize Incident, Detect Anomaly)

🚀 **The Challenge: Log Analysis Is Too Slow?**

Are you tired of manually sifting through mountains of logs and connecting scattered traces to understand complex failures?
**CSharpEssentials.LoggerHelper.AI** offers a powerful solution by integrating **Large Language Models (LLMs)** directly into your logging and observability pipeline. It transforms raw log data and operational context into **actionable, real-time insights**.

---

### ⚠️ Breaking Change in Version 4.0.7

Starting with version **4.0.7**, the configuration for the `CSharpEssentials.LoggerHelper.AI` module has been refactored to support **multiple Large Language Models (LLMs)**, including **Gemini** alongside OpenAI. This change requires mandatory updates to your `appsettings.json` file. Please review the following points to ensure continued functionality.

-----

### 1\. Mandatory Addition: `FolderAIModelsLoaderContainer`

The key **`FolderAIModelsLoaderContainer`** is now mandatory. It must contain the **path to the folder** where the specific request and response templates (used for data extraction from the LLM) are stored for **each supported AI model (e.g., OpenAI, Gemini)**. This allows flexible management of model-specific input/output formats.

Example configuration in `appsettings.json`:

```json
"AIConfiguration": {
  "FolderAIModelsLoaderContainer": "Templates/AI"
}
```

### 2\. Relocating the API Key (Removal of `chatghapikey`)

The authentication key for the chat service, previously named **`chatghapikey`**, has been **removed** from direct `appsettings` configuration.

The LLM API key must now be included and managed within the **`headersLLM`** variable or configuration section. This change centralizes the management of HTTP headers (including authorization) for all LLM calls, regardless of the provider (e.g., using a single, unified structure for both OpenAI and Gemini keys/headers).

### 3\. Implementation Example

Here is an example of settings : 

```json
  "LoggerAIOptions": {
    "Name": "gemini",
    "Model": "gemini-2.5-flash",
    "chatghapikey": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "FolderSqlLoaderContainer": "D:\\github\\Csharp.Essentials.Extensions\\Web.Api\\SqlQueries",
    "FolderAIModelsLoaderContainer": "D:\\github\\Csharp.Essentials.Extensions\\Web.Api\\AIModels",
    "Temperature": 0.7,
    "topScore": 5,
    "urlLLM": "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent",
    "headersLLM": {
      "accept": "application/json",
      "x-goog-api-key": "xxxxxxxxxxxxxxxxxxxxxxxxxxxx"
    },
    "httpClientName": "testAI"
  },
```

## AI Package Configuration

To enable AI features in your project, you must add the `LoggerAIOptions` section to your `AppSettings.json` file. This section contains all the necessary settings to configure the AI model and its connection to the service.

```json
"LoggerAIOptions": {
  "Model": "gpt-4o-mini",
  "chatghapikey": "github_pat_xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "FolderSqlLoaderContainer": "D:\\github\\Csharp.Essentials.Extensions\\Web.Api\\SqlQueries",
  "Temperature": 0.7,
  "topScore": 5,
  "urlLLM": "https://models.inference.ai.azure.com/chat/completions",
  "headersLLM": [
    { "accept": "application/json" },
    { "X-GitHub-Api-Version": "2023-10-01" }
  ],
  "httpClientName": "testAI"
}
```

### ⚠️ Configuration Error Diagnostics

Should you encounter configuration issues with your `appSettings.json` file, don't worry. The `CSharpEssentials.LoggerHelper.AI` logger instance is designed to send an **error message** directly to your dashboard.

In the **Monitor-Sink** section, by filtering the `Sink` for "LoggerHelper.AI," you will be able to view the error details. This allows you to quickly identify and resolve any missing or invalid configuration keys.

![Dashboard AI Errors](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/Dashboard_AI_Error.png)

---

### Key Settings

*   **`Model`**: Specifies the name of the AI model to be used (e.g., `gpt-4o-mini`), which must be available on the configured AI inference service.
*   **`chatghapikey`**: Your GitHub API key, used for authentication with the chat service.
*   **`FolderSqlLoaderContainer`**: Defines the local path where SQL query files for the AI are stored. The AI assistant uses these queries to interact with your data.
*   **`Temperature`**: Controls the randomness of the AI model's output. Lower values produce more deterministic and focused responses, while higher values lead to more creative and varied outputs. The value should be between 0.0 and 1.0.
*   **`topScore`**: A parameter used for search results. It determines the number of top-ranking results to be considered by the AI model when retrieving information.
*   **`urlLLM`**: The URL endpoint of the Large Language Model (LLM) service. This is the address where API requests are sent.
*   **`headersLLM`**: A collection of HTTP headers required for API requests, such as authentication tokens or content types.
*   **`httpClientName`**: The name of the `HttpClient` instance used for making requests to the LLM service. This is typically configured in the application's startup file.

-----

### 🚀 Service Registration

To enable the AI-powered logging features, you must register the `CSharpEssentials.LoggerHelper.AI` services within your application's `Program.cs` or startup file.

Use the `AddCSharpEssentialsLoggerAI` extension method on `IServiceCollection`. This method requires your application's **`IConfiguration`** and an optional `Action<IServiceCollection>` to configure the persistence mechanism.

### Basic Registration


```csharp
using CSharpEssentials.LoggerHelper.AI.Infrastructure;

// ... in Program.cs
builder.Services.AddCSharpEssentialsLoggerAI(
    builder.Configuration,
    SqlAiPersistenceFactory.AddSqlAiPersistence(builder.Configuration)
);
```

-----

### Managing HttpClients with `CSharpEssentials.HttpHelper`

`httpClientName` specifies the name of the `HttpClient` used for making requests to the Large Language Model (LLM) service. This name corresponds to a named client configured in your application's startup file, typically using the `CSharpEssentials.HttpHelper` NuGet package.

By using a **named `HttpClient`**, you can centralize the configuration for the LLM service, including the base URL and any required headers (like those for authentication). This approach offers several benefits:

*   **Reusability**: You can reuse the same client configuration across different parts of your application without repeating code.
*   **Centralized Control**: All client-specific settings, such as timeouts and request headers, are managed in one place.
*   **Testability**: Named clients simplify testing by allowing you to easily mock or substitute the `HttpClient` for unit tests.

The `CSharpEssentials.HttpHelper` package streamlines this process by providing an easy way to define and manage these named clients, ensuring consistency and maintainability in your codebase.

---

### 🌟 Hybrid Contextual Data Sourcing (Unique Feature!)

`FolderSqlLoaderContainer` defines the path where SQL query files for the AI are stored. These files provide the context for the AI's actions, allowing it to perform specific tasks. The system currently supports four distinct modes, each corresponding to a different action.

**Example Folder Structure for `FolderSqlLoaderContainer`:**

```
YourApp/SqlQueries/
├── RagAnswerQuery/
│   ├── getLogs.sql
│   └── getLogs.txt
│   ├── getTraces.sql
│   └── getTraces.txt
├── CorrelateTrace/
│   ├── getTraces.sql
│   └── getTraces.txt
├── SummarizeIncident/
│   ├── getLogsForIncident.sql
│   └── getLogsForIncident.txt
└── DetectAnomaly/
    ├── getMetrics.sql
    └── getMetrics.txt
```

Enable the AI to reason over your *operational data* by dynamically providing context via a simple folder structure:

*   **SQL Query Injection (`.sql`):** Define your contextual data extraction in standard `.sql` files within a designated folder (`ContextFolderPath`). The AI system dynamically loads and executes these queries.
    
    **Crucially, the SQL syntax in these files must be compatible with the database provider selected in your `appsettings.json`:**
    
    ```json
    {
      "DatabaseProvider": "postgresql", // Set to "postgresql" or "sqlserver"
      "ConnectionStrings": {
        "Default": "Your_Connection_String_Here"
      }
    }
    ```
    
    The selection via `"DatabaseProvider"` determines the dialect and parameter syntax required for your `.sql` files. These queries fetch relevant data (e.g., transaction details, user history) to enrich the LLM prompt.

*   **Structured Formatting and Prompting (`.txt`):** For **every** `.sql` file that defines an extraction, a corresponding file with the **same name and a `.txt` extension must exist**. This `.txt` file dictates the exact **structured format** required for the resulting SQL data. This mechanism ensures the output is perfectly prepared, structured, and optimized to be consumed as context by the LLM, maintaining prompt integrity.

Each mode is represented by a specific C# class that inherits from `ILogMacroAction` and performs a unique task, often using an embedded SQL file to query data.

---

### The Four Available AI Actions

#### 1. `RagAnswerQuery`
This mode is designed to answer user questions based on a specific set of data. It uses **Retrieval-Augmented Generation (RAG)** by fetching the most relevant documents from a vector store and using them as context for the LLM.

*   **How it works**: The system embeds the user's query into a vector and uses it to retrieve similar documents from the vector store. The SQL query defined in the `sqlQuery` variable is used to query the vector store. The retrieved documents are then used as the `CONTEXT` for a prompt, and the AI generates a precise and concise answer.

#### 2. `CorrelateTrace`
This mode helps to identify the most suspicious trace within a recent set of logs.

*   **How it works**: It fetches the most recent 50 traces. It then composes a list of candidate traces, including details like `TraceId`, `duration`, and `anomaly` status. This list is provided to the LLM as a prompt, and the AI is instructed to select the most suspicious one and explain why.

#### 3. `SummarizeIncident`
This action is used to summarize the root cause, impact, and remediation of a specific incident using a `TraceId`.

*   **How it works**: The system fetches up to 200 logs associated with the provided `TraceId`. It builds a compact, chronological timeline from these logs, ensuring the content fits within a specified character budget. This timeline is then passed to the LLM, which generates a concise summary of the incident. The prompt includes an example of the desired output to guide the AI's tone and structure.

#### 4. `DetectAnomaly`
This mode is designed to detect anomalies in a time series of metrics.

*   **How it works**: It queries a repository for a specific metric (e.g., `http.client.request.duration`) over a defined time period (e.g., the last 30 minutes). It then calculates the statistical **mean** and **standard deviation** of the data points. Finally, it computes the **Z-score** of the last data point and determines if it indicates an anomaly based on a predefined threshold (e.g., `z >= 3`).

---

### How to Test AI Models

You can test the four AI actions using two different methods: the [web API demo with Scalar](https://github.com/alexbypa/Csharp.Essentials.Extensions/blob/main/Web.Api/MinimalApi/Endpoints/AI/ApiAIHelperDemo.cs) or the [CSharpEssentials.LoggerHelper.Dashboard](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Dashboard) client application.

#### 1. Web API Demo with Scalar

A web API demo is available to test the AI actions directly. This method allows you to interact with the backend API without using the frontend dashboard. The API documentation can be accessed via the Scalar interface.

To run a test, you must send a `POST` request to the `http://localhost:1234/AI/run` endpoint with a JSON body.

*   **Example for the `RagAnswerQuery` Action:**
    *   **Endpoint:** `http://localhost:1234/AI/run`
    *   **Body:**
        ```json
        {
          "docId": null,
          "traceId": null,
          "query": "I received http response with httpstatus 401",
          "system": "If you don't find anything in the context, reply with 'I'm sorry but I couldn't find this information in the database!'",
          "action": "RagAnswerQuery",
          "fileName": "getTraces.sql",
          "dtStart": "2025-09-22T08:00:00",
          "topResultsOnQuery" : 200
        }
        ```
*   **Result:** The `RagAnswerQuery` action will search for relevant information based on the provided `query` and `fileName`. As shown in the example, if the information is not found in the database, the AI will respond with the message defined in the `system` parameter.

---

#### 2. LoggerHelper.Dashboard

You can also test the AI models directly from the `LoggerHelper.Dashboard` client application. This method provides a user-friendly interface for interacting with the AI.

*   **Interface:** The interface includes dropdown menus for `Action` and `File Name`, along with text areas for `Query` and `System` prompts.
*   **Workflow:**
    1.  Select the desired **`Action`** from the dropdown menu (e.g., `RagAnswerQuery`).
    2.  Choose the specific SQL file from the **`File Name`** dropdown. This file is located in the `FolderSqlLoaderContainer` and corresponds to the selected action (e.g., `getTraces.sql` for `RagAnswerQuery`).
    3.  Enter your **`Query`** and a custom **`System`** prompt to guide the AI's response.
    4.  Click **`Send to LLM`** to process the request.

---

### 📊 RAG with SQL Query Files

A key feature of the **AI Assistant** is its ability to perform **Retrieval-Augmented Generation (RAG)** using pre-saved SQL queries. This is useful for fetching specific data from your database to provide context for the LLM.

#### **Use Case: Analyzing Recent Logs**

This example demonstrates how to use the RAG system to analyze recent log entries by referencing a saved SQL query file.

1.  **Prepare Your SQL Query**:
    Save your query in a `.sql` file within the `RagAnswerQuery` folder. For this example, let's use `getlogs.sql`. The query uses placeholders `{now}` for the current timestamp and `{n}` for the number of results, which are dynamically replaced at runtime.

    Here's an example of the `getlogs.sql` file content:

    ```sql
    select "Id", "ApplicationName" "App", "TimeStamp" "Ts", "Message", "IdTransaction" "TraceId" from "LogEntry"  
    where "TimeStamp" > @now
    order by "Id" desc
    limit @n
    ```

    *   `{now}`: The starting date from the user input. The query will return logs from this date onward.
    *   `{n}`: The limit on the number of results to fetch, which you can specify in the UI.

2.  **Use the AI Assistant Dashboard**:
    Navigate to the **AI Assistant** page in your dashboard.
    *   **Action**: Select `RagAnswerQuery`.
    *   **File Name**: Choose the `getlogs.sql` file.
    *   **Query**: Enter your natural language question, for example: "Were there any HTTP responses with status 401?"
    *   **Start Date**: Set the date to filter the query.
    *   **System**: Add any specific instructions for the LLM, such as "Stick closely to the context. If you don't find anything, reply with 'Sorry but I didn't find anything'".

The system will execute the `getlogs.sql` query, retrieve the relevant log entries, and use that data as context to generate a precise answer to your question.

---

### 🤖 AI-Powered Log Analysis with RAG

A key benefit of the **AI Assistant** is its ability to perform **Retrieval-Augmented Generation (RAG)** on your application's logs, saving operators from having to manually query the database. The LLM can analyze log data provided by a SQL query to answer complex questions instantly.

#### **Example: Troubleshooting an Error**

Let's illustrate with an example. An operator needs to check if a specific HTTP error occurred in the logs.

1.  **Context from SQL Query**:
    The system uses a pre-saved query, like `getlogs.sql`, to retrieve a specific set of log records. The query includes parameters for the start date and the maximum number of records, allowing the operator to define the search scope directly from the dashboard.

2.  **LLM Analysis**:
    The operator inputs a natural language query like "**Are there any HTTP responses with status 401?**" The LLM then receives the results from the executed SQL query as its context. By analyzing this context, the LLM provides a precise and detailed answer, summarizing the findings without the operator ever needing to write SQL or connect to the database.

3.  **Efficiency**:
    This process is extremely efficient. The more logs you write, the more powerful the LLM becomes in providing deep insights into your application's behavior. It automates the task of searching through vast amounts of log data, allowing your team to focus on resolving issues faster.

This feature is a powerful demonstration of how `CSharpEssentials.LoggerHelper.AI` can transform raw log data into actionable insights, improving diagnostic speed and overall operational efficiency.
![Dashboard AI RAG Example](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/dashboard_AI_Rag_Example.png)

---

### 🕵️ AI-Powered Root Cause Analysis with CorrelateTrace

Beyond simple log retrieval, the **AI Assistant** can perform powerful root cause analysis by correlating logs and distributed traces. This is particularly useful for debugging complex issues in microservice architectures without having to manually sift through data. While sharing a similar execution pattern with `SummarizeIncident`, the `CorrelateTrace` action is specifically designed to **identify the most suspicious trace** within a set of related activities, focusing on pinpointing anomalies rather than just summarizing.

#### **Use Case: Diagnosing a Timeout Error in a Chain of Responsibility Pattern**

Imagine you're troubleshooting a slow request or a timeout that occurs within a complex interaction, such as a Chain of Responsibility pattern where multiple HTTP calls are made. Instead of manually searching logs and traces across multiple services, you can let the AI Assistant do the heavy lifting.

**Real-world Scenario:** In a realistic application, a single user request might trigger a cascade of internal HTTP calls, each potentially handled by a different microservice. If one of these services introduces a delay, it can lead to a downstream timeout for the original request. Manually pinpointing the exact service or component causing the slowdown can be time-consuming and tedious.

**Simulating the Scenario for Testing:**
You can easily simulate this scenario using the `CSharpEssentials.LoggerHelper.Telemetry` package.
1.  **Trigger a Simulated Latency**: Using the demo project (which leverages `CSharpEssentials.HttpHelper` for mocking `HttpClient` behavior), you can invoke an HTTP endpoint that intentionally introduces a delay in one of its internal spans. This simulates a slow external dependency or a bottleneck in your application's chain of responsibility.
    For example, make a `GET` request to `http://localhost:1234/Telemetry/Simple` with a `SecondsDelay` parameter set to `40`. This will simulate a 40-second delay in one of the internal HTTP calls.

![scalar demo delay](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/scalar_http_Simple_delay.png)    
    
2.  **Capture the `IdTransaction`**: Upon receiving the (potentially timed-out) response, extract the `IdTransaction` (which corresponds to the `TraceId` of the activity). This ID links all the telemetry data for that specific request.
 
![scalar demo delay](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/scalar_http_Simple_delay_response.png)    
    
3.  **LLM Diagnosis via the Dashboard**:
    Now, with the `TraceId` in hand, navigate to the `CSharpEssentials.LoggerHelper.Dashboard` and use the AI Assistant. Provide a natural language query like: **"I have an issue with a slow request. Can you find the suspicious trace and tell me the root cause of the timeout?"**
    
    *   **Action**: Select `CorrelateTrace`.
    *   **Trace ID**: Input the `IdTransaction` you captured (e.g., `bf90d68e05126496e2ae2b5c45d3c4cd`).
    *   **System Prompt**: Use a specialized prompt such as: "You are an SRE assistant specialized in distributed tracing. Analyze the provided traces and logs, identify the longest-running span, and explain why the operation timed out. If you find a specific error, mention the service and the error message."

    The LLM, acting as a specialized Site Reliability Engineer (SRE), analyzes the correlated data provided by the `CorrelateTrace` action. It identifies the longest-running span within the trace and pinpoints the exact service or operation that caused the delay.

    **Example Output from the Dashboard:**
![Dashboard AI RAG Example](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/dashboard_AI_CorrelateTrace_Example.png)
   

4.  **Actionable Insights**:
    The LLM's response provides a clear diagnosis, explaining **why** the issue occurred and where to look. This transforms raw telemetry into an actionable summary, allowing developers and SREs to dramatically reduce their mean time to resolution (MTTR) by avoiding tedious manual database searches.

This feature showcases how `CSharpEssentials.LoggerHelper.AI` transforms raw data into intelligent, actionable insights. Here's an example of how you can test this functionality via cURL:

```curl
curl http://localhost:1234/AI/run \
  --request POST \
  --header 'Content-Type: application/json' \
  --data '{
  "docId": null,
  "traceId": "bf90d68e05126496e2ae2b5c45d3c4cd", # Replace with your actual TraceId from the simulated call
  "query": "I have an issue with a slow request. Can you find the suspicious trace and tell me the root cause of the timeout?",
  "system": "You are an SRE assistant specialized in distributed tracing. Analyze the provided traces and logs, identify the longest-running span, and explain why the operation timed out. If you find a specific error, mention the service and the error message.",
  "action": "CorrelateTrace",
  "fileName": "getTraces.sql",
  "dtStart": "2022-09-22T08:00:00",
  "topResultsOnQuery": 100
}'
```

---

### 🚨 AI-Powered Anomaly Detection

The **AI Assistant** excels at proactively identifying anomalies within your operational metrics, transforming raw data into actionable alerts and insights. This feature helps SREs and developers quickly pinpoint unusual behavior, understand its root cause, and implement timely solutions, significantly reducing the mean time to detect (MTTD) and mean time to resolve (MTTR) critical issues.

#### **Use Case: Detecting Abnormal Error Rates or Latency**

Imagine your application experiences a sudden spike in error rates or an unexpected increase in request latency. Manually sifting through dashboards and logs to find the anomaly and its cause can be a time-consuming process. The `DetectAnomaly` action automates this by leveraging the power of LLMs.

**Scenario:** A critical microservice starts exhibiting higher-than-usual error rates or response times.

**How to use the `DetectAnomaly` action:**

1.  **Configure your Metric Query**:
    Ensure you have a SQL query file, such as `getMetrics.sql`, 
```sql
select
	M."TraceId", 
	M."Value", 
	T."TagsJson" "TraceJson" from "MetricEntry" M 
	inner join "TraceEntry" T ON M."TraceId" = T."TraceId"
	where M."TraceId" IS NOT NULL and M."Name" = 'db.client.commands.duration' 
		and "Timestamp" between '2025-09-20T08:00:00' and '2025-09-28T11:00:00'
		order by M."Value" desc limit 10
```
within your `DetectAnomaly` folder. This query should retrieve the time-series data for the metric you want to monitor (e.g., `http.client.request.duration`, error counts, CPU usage).

    The corresponding `getMetrics.txt` file defines how the fetched data should be structured for the LLM. For instance:

    ```
    TraceId: {TraceId} | LogEvent: {TraceJson} | Score: {Value}
    ```

    This structure ensures the AI receives key information like the `TraceId`, the full `LogEvent` (potentially JSON data), and a calculated `Score` (e.g., Z-score or anomaly score) for each data point.

2.  **Use the AI Assistant Dashboard**:
    Navigate to the **AI Assistant** page in your dashboard.
    *   **Action**: Select `DetectAnomaly`.
    *   **File Name**: Choose the `getMetrics.sql` file.
    *   **Query**: Enter a natural language question asking the AI to analyze the context for anomalies, for example: "Analyze the data in context. Is there an anomaly? If so, what is the root cause and possible solution?"
    *   **Start Date / End Date**: Define the time window for the metric data you want to analyze.
    *   **Top Records**: Specify the number of records to fetch.
    *   **System Prompt**: Provide specific instructions to guide the AI's analysis and recommendations: "You are a systems analyst with expertise in observability. Analyze the provided metrics and logs (CONTEXT) to identify the root cause of the detected anomaly and recommend mitigation. Prioritize abnormal error rates and latency."

    The system will execute the `getMetrics.sql` query, retrieve the relevant metric data and associated logs, perform statistical analysis (like Z-score calculation), and then use this enriched data as context for the LLM. The AI will then identify any anomalies, explain their potential root causes, and suggest possible solutions.

    **Example Output from the Dashboard:**
![Dashboard AI detect anoamly Example](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper.AI/Docs/dashboard_AI_DetectAnomaly_Example.png)
---

## 🔍 Dashboard <a id='dashboard'></a>   [🔝](#table-of-contents)

> NuGet: [CSharpEssentials.LoggerHelper.Dashboard](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Dashboard)

The **embedded Dashboard** lets you quickly inspect the logging setup at runtime:
- See all **registered sinks** (e.g., MSSqlServer, Console, Elasticsearch, …)
- Check **enabled levels** per sink (Information, Warning, Error, Fatal, …)
- Spot **initialization / load errors** for any sink at a glance

#### Install
```bash
dotnet add package CSharpEssentials.LoggerHelper.Dashboard
````

#### Usage

Register the embedded dashboard in your Web API ( admin in this example is name of home page ):

```csharp
// Program.cs
// ...
app.UseLoggerHelperDashboard<RequestHelper>(); // registers the embedded dashboard
// ...
```

Once enabled, the dashboard UI is served by the application and provides a live view of
configured sinks, their write levels, and any sink-loading issues:

> ![Dashboard](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper/img/Dashboard.png)
The advanced features for **AI-powered querying and macros** (`RAG Answer Query`, `Correlate Trace`, etc.) are not part of the base dashboard but are exposed as separate API endpoints upon integration of the `CSharpEssentials.LoggerHelper.AI` package.

For detailed documentation on how to use and query logs via the AI functions, please refer to the **dedicated AI integration section** within this document.

---

## 🚀 Extending LogEvent Properties from Your Project<a id='customprop'></a>   [🔝](#table-of-contents)

Starting from version **2.0.9**, you can extend the default log event context by implementing your own **custom enricher**. This allows you to **add extra fields** to the log context and ensure they are included in **all log sinks** (not only in email notifications, but also in any other sink that supports additional fields—especially in the databases, where from version **2.0.8** onwards you can add dedicated columns for these custom properties).
**How to configure it:**

✅ **1️⃣ Register your custom enricher and logger configuration in `Program.cs`**
Before building the app:

```csharp
builder.Services.AddSingleton<IContextLogEnricher, MyCustomEnricher>();
builder.Services.AddloggerConfiguration(builder);
```

✅ **2️⃣ Assign the service provider to `LoggerHelperServiceLocator`**
After building the app:

```csharp
LoggerHelperServiceLocator.Instance = app.Services;
```

✅ **3️⃣ Create your custom enricher class**
Example implementation:

```csharp
public class MyCustomEnricher : IContextLogEnricher {
    public ILogger Enrich(ILogger logger, object? context) {
        if (context is MyCustomRequest req) {
            return logger
                .ForContext("Username", req.Username)
                .ForContext("IpAddress", req.IpAddress);
        }
        return logger;
    }

    public LoggerConfiguration Enrich(LoggerConfiguration configuration) => configuration;
}
```
👉 **Note:**
In addition to the fields already provided by the package (e.g., `MachineName`, `Action`, `ApplicationName`, `IdTransaction`), you can add **custom fields**—such as the **logged-in username** and the **IP address** of the request—using your own properties.

✅ **4️⃣ Use your custom request class in your application**

> **Note:** your custom request class (e.g. `myRequest`) must implement the `ILoggerRequest` interface provided by **LoggerHelper**.

Example usage:

```csharp
var myRequest = new MyCustomRequest {
    IdTransaction = Guid.NewGuid().ToString(),
    Action = "UserLogin",
    ApplicationName = "MyApp",
    MachineName = Environment.MachineName,
    Username = "JohnDoe",
    IpAddress = "192.168.1.100"
};

loggerExtension<MyCustomRequest>.TraceSync(myRequest, LogEventLevel.Information, null, "User login event");
```

✅ **5️⃣ Update your email template to include the new fields**
Example additions:

```html
<tr><th>User Name</th><td>{{Username}}</td></tr>
<tr><th>Ip Address</th><td>{{IpAddress}}</td></tr>
```
✅ **6️⃣ MSSQL and PostgresQL sink Template Customization**
- To add extra fields on table of MSSQL add fields on array **additionalColumns**
- To add extra fields on table of postgre add fields on array **ColumnsPostGreSQL**

🔗 **Download Example**
You can see an example in the [demo controller](https://github.com/alexbypa/CSharp.Essentials/blob/main/Test8.0/Controllers/logger/LoggerController.cs).
Whereas the custom class to generate extra fields can be found [here](https://github.com/alexbypa/CSharp.Essentials/blob/main/Test8.0/Controllers/logger/MyCustomEnricher.cs).

---
## 🧪 Demo API <a id='demo-api'></a>   [🔝](#table-of-contents)

Try it live with full logging and structured output on 📁 [Demo Project](https://github.com/alexbypa/CSharp.Essentials/tree/main/LoggerHelperDemo)

### 📝 appsettings.loggerhelper.json (Development – Debug)

This is the full `appsettings.LoggerHelper.json` used in the demo Minimal API (remember to use appsettings.LoggerHelper.debug.json on development):

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Debug",
        "System": "Debug"
      }
    },
    "SerilogConfiguration": {
      "ApplicationName": "DemoLogger 9.0",
      "SerilogCondition": [
        {
          "Sink": "ElasticSearch",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "File",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "Email",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "Telegram",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "PostgreSQL",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "MSSqlServer",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        },
        {
          "Sink": "Console",
          "Level": [
            "Information",
            "Warning",
            "Error",
            "Fatal"
          ]
        }
      ],
      "SerilogOption": {
        "File": {
          "Path": "C:\\Logs\\DemoLogger",
          "RollingInterval": "Day",
          "RetainedFileCountLimit": 7,
          "Shared": true
        },
        "Email": {
          "From": "jobscheduler.pixelo@gmail.com",
          "Port": 587,
          "Host": "your_host",
          "To": "recipient",
          "username": "username_smtp",
          "password": "password_smtp",
          "EnableSsl": true,
          "TemplatePath": "Templates/email-template-default.html",
          "ThrottleInterval": "00:00:20"
        },
        "TelegramOption": {
          "chatId": "chatid",
          "Api_Key": "api_key",
          "ThrottleInterval": "00:00:20"
        },
        "PostgreSQL": {
          "connectionString": "your_connection",
          "tableName": "LogEntry",
          "schemaName": "public",
          "needAutoCreateTable": true,
          "addAutoIncrementColumn": true,
          "ColumnsPostGreSQL": [
            { "Name": "Message",           "Writer": "Rendered",   "Type": "text1111" },
            { "Name": "MessageTemplate",   "Writer": "Template",   "Type": "text"     },
            { "Name": "Level",             "Writer": "Level",      "Type": "varchar"  },
            { "Name": "TimeStamp",         "Writer": "timestamp",  "Type": "timestamp"},
            { "Name": "Exception",         "Writer": "Exception",  "Type": "text"     },
            { "Name": "Properties",        "Writer": "Properties", "Type": "jsonb"    },
            { "Name": "LogEvent",          "Writer": "Serialized", "Type": "jsonb"    },
            { "Name": "IdTransaction",     "Writer": "Single",     "Property": "IdTransaction",   "Type": "varchar" },
            { "Name": "MachineName",       "Writer": "Single",     "Property": "MachineName",     "Type": "varchar" },
            { "Name": "Action",            "Writer": "Single",     "Property": "Action",          "Type": "varchar" },
            { "Name": "ApplicationName",   "Writer": "Single",     "Property": "ApplicationName", "Type": "varchar" },
            { "Name": "Username",          "Writer": "Single",     "Property": "Username",        "Type": "varchar" },
            { "Name": "IpAddress",         "Writer": "Single",     "Property": "IpAddress",       "Type": "varchar" }
          ]
        },
        "MSSqlServer": {
          "connectionString": "your_connection",
          "sinkOptionsSection": {
            "tableName": "LogEntry",
            "schemaName": "dbo",
            "autoCreateSqlTable": true,
            "batchPostingLimit": 100,
            "period": "0.00:00:10"
          },
          "columnOptionsSection": {
            "addStandardColumns": [
              "LogEvent",
              "Message",
              "MessageTemplate",
              "Level",
              "Exception"
            ],
            "removeStandardColumns": [ "Properties" ]
          },
          "additionalColumns": [
            "IdTransaction",
            "Action",
            "MachineName",
            "ApplicationName",
            "Username",
            "IpAddress"
          ]
        },
        "ElasticSearch": {
          "nodeUris": "endpoint",
          "indexFormat": "indexformat"
        }
      }
    }
  }
}
```

## 📜 Version History<a id='versions'></a>   [🔝](#table-of-contents)

* **1.1.2** – Added Middleware
* **1.1.4** – Removed `TraceAsync` on `finally` block of `RequestResponseLoggingMiddleware`
* **1.1.6** – Fixed issues detected by CodeQL
* **1.2.1** – Optimized with test Web API
* **1.2.2** – Optimized `Properties` handling and Email sink
* **1.3.1** – Added compatibility with .NET 6.0
* **2.0.0** – Fixed Email configuration and sink behavior
* **2.0.2** – Optimized HTML template for middleware
* **2.0.4** – Rollback: removed .NET 7.0 support
* **2.0.5** – Fixed `IRequest` interface
* **2.0.6** – Added external email template support
* **2.0.7** - Added addAutoIncrementColumn and ColumnsPostGreSQL on sink postgresQL
* **2.0.8** - Enhanced MSSQL Sink Configuration : Introduced comprehensive management of custom columns for the MSSQL sink.
* **2.0.9** - Breaking Change: Added support for extending log context with custom fields (IRequest extensions)
* **3.0.1** - Moved all built-in sinks into separate NuGet packages; updated documentation to highlight explicit sink installation and aligned sink package versions
* **3.0.2** - Duplicate registration and build errors in some Sinks
* **3.0.3** - Added workaround for Path wrong on sink file and fixed Environment development toload appSettings.LoggerHelper.Debug.json
* **3.1.0** - Added workaround for Path wrong on sink file and fixed Environment development toload appSettings.LoggerHelper.Debug.json
* **3.1.3** - Fixed Skin MSSQL 
* **3.1.4** - Added ThrottleInterval


## 🤝 Contributing

Contributions, issues, and feature requests are welcome!
Feel free to open a [pull request](https://github.com/alexbypa/CSharpEssentials/pulls) or [issue](https://github.com/alexbypa/CSharpEssentials.HttpHelper/issues).

---

## 📜 License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

