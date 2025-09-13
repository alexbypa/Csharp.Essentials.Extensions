﻿# 📦 Csharp.Essentials

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
| **CSharpEssentials.LoggerHelper.Telemetry** | A full OpenTelemetry sink for CSharpEssentials.LoggerHelper, enabling metrics, traces, and logs with automatic database storage for end-to-end observability. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Telemetry) |
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

# 📈 Custom Metrics

The package includes support for **custom and predefined metrics**.

* `GaugeWrapper` → create observable gauges easily
* Predefined metrics:

  * `memory_used_mb`
  * `postgresql.connections.active`
* Extendable via `CustomMetrics`

```csharp
// Example: create your own custom gauge
GaugeWrapper.Create("custom.active_users", () => MyService.GetActiveUsersCount());
```

---

# 🔗 Correlation between Logs and Traces

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

Register the embedded dashboard in your Web API:

```csharp
// Program.cs
// ...
app.UseLoggerHelperDashboard<RequestHelper>(); // registers the embedded dashboard
// ...
```

Once enabled, the dashboard UI is served by the application and provides a live view of
configured sinks, their write levels, and any sink-loading issues:

> ![Dashboard](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.LoggerHelper/img/Dashboard.png)

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

