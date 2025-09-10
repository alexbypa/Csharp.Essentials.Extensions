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

## Contents

- [Using HttpHelper](#using-httphelper)

## Using HttpHelper

This section explains how to install **HttpHelper** and configure your project to run the demo. Detailed Minimal API examples for each HttpHelper method will be added later.

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

### 1. Simple GET request
```csharp
IhttpsClientHelperFactory factory = ...;
IContentBuilder contentBuilder = new NoBodyContentBuilder();

var client = factory.CreateOrGet("Test_No_RateLimit");

HttpResponseMessage response = await client.SendAsync(
    "https://jsonplaceholder.typicode.com/posts/1",
    HttpMethod.Get,
    null,
    contentBuilder);

string result = await response.Content.ReadAsStringAsync();
Console.WriteLine(result);
````

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

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!
Feel free to open a [pull request](https://github.com/alexbypa/CSharpEssentials.HttpHelper/pulls) or [issue](https://github.com/alexbypa/CSharpEssentials.HttpHelper/issues).

---

## 📜 License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

