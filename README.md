# Csharp.Essentials

A **modular helper library for .NET** projects designed to make common tasks like HTTP calls, background jobs and logging easier and more robust.  This repository contains demonstrations and extensions for the **Csharp.Essentials** NuGet packages.

Csharp.Essentials provides several packages that can be used independently or together depending on your project’s needs:

| Package | Description | NuGet |
|---|---|---|
| **CSharpEssentials.HttpHelper** | Simplifies `HttpClient` usage with built‑in resiliency (retries/fallbacks) and rate‑limiting strategies:contentReference[oaicite:0]{index=0}. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.HttpHelper) |
| **CSharpEssentials.LoggerHelper** | Provides logging helpers that leverage Serilog sinks to trace HTTP requests and responses:contentReference[oaicite:1]{index=1}. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper) |
| **CSharpEssentials.LoggerHelper.Sink.MSSqlServer** | Adds an MSSQL sink for durable log storage. | [NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper.Sink.MSSqlServer) |

These packages are intentionally lightweight. You can add only what you need, keeping your application lean and maintainable:contentReference[oaicite:2]{index=2}.

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

# LoggerHelper (optional) – enrich your HTTP calls with Serilog logging
 dotnet add package CSharpEssentials.LoggerHelper
````

### Program.cs setup

To start the demo, you only need to register the HttpHelper clients and enable the OpenAPI + Scalar documentation. The Minimal API endpoints themselves will be defined elsewhere in the project.

```csharp
using CSharpEssentials.HttpHelper;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register HttpHelper clients. The configuration section `HttpClientOptions` in appsettings.json
// allows you to specify certificates, rate‑limit settings and more:contentReference[oaicite:3]{index=3}.
// If you are not using a custom HttpMessageHandler (e.g. Moq for tests), pass null as the second argument.
builder.Services.AddHttpClients(builder.Configuration, null);

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
* Built‑in OpenAPI document generation (available at `/openapi/v1.json`) and **Scalar** as an interactive UI for exploring the API.

The actual endpoints demonstrating HttpHelper usage (GET, POST with retries, logging, etc.) will be defined in subsequent sections of this repository.
