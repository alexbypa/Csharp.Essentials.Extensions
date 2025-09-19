using BusinessLayer.Application;
using BusinessLayer.Contracts.Context;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.AI.Application;
using CSharpEssentials.LoggerHelper.AI.Domain;
using CSharpEssentials.LoggerHelper.AI.Infrastructure;
using CSharpEssentials.LoggerHelper.AI.Ports;
using CSharpEssentials.LoggerHelper.Configuration;
using CSharpEssentials.LoggerHelper.Dashboard.Extensions;
using CSharpEssentials.LoggerHelper.Telemetry.Configuration;
using Microsoft.Data.SqlClient;
using Scalar.AspNetCore;
using Web.Api.MinimalApi;
using Web.Api.MinimalApi.Endpoints.Telemetries;

var builder = WebApplication.CreateBuilder(args);

#region CSharpEssentials.HttpHelper Package
builder.Services.AddHttpClients(builder.Configuration, null); //if you dont use Moq
//builder.Services.AddHttpClients(builder.Configuration, HttpMocks.CreateHandler());
#endregion

#region Logger Configuration
builder.Services.AddSingleton<IContextLogEnricher, MetricsEnricher>();
builder.Services.AddloggerConfiguration(builder);
builder.Services.AddSingleton(new MyCustomMetrics(500));
#endregion

builder.Services.AddLoggerTelemetry(builder);

#region CORS
const string CorsPolicy = "ViteDev";
builder.Services.AddCors(opt => {
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins("http://localhost:5173")   // porta di Vite
         .AllowAnyMethod()
         .AllowAnyHeader()
    // .AllowCredentials() // scommenta solo se usi cookie/autenticazione
    );
});
#endregion

#region LoggerHelper.AI Package
// SQL Server
builder.Services.AddScoped(_ => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

// Repos
builder.Services.AddScoped<ILogRepository, SqlLogRepository>();
builder.Services.AddScoped<ITraceRepository, SqlTraceRepository>();
builder.Services.AddScoped<IMetricRepository, SqlMetricRepository>();

// Azioni macro e orchestratore (come già definito in precedenza)
builder.Services.AddScoped<IEmbeddingService, NaiveEmbeddingService>();
builder.Services.AddScoped<ILogVectorStore, SqlLogVectorStore>(); 
builder.Services.AddScoped<ILogVectorStore, InMemoryLogVectorStore>(); 

builder.Services.AddScoped<ILogMacroAction, SummarizeIncidentAction>();
builder.Services.AddScoped<ILogMacroAction, CorrelateTraceAction>();
builder.Services.AddScoped<ILogMacroAction, DetectAnomalyAction>();
builder.Services.AddScoped<ILogMacroAction, RagAnswerQueryAction>();
builder.Services.AddScoped<IActionOrchestrator, ActionOrchestrator>();

builder.Services.AddScoped<ILlmChat, OpenAiLlmChat>(); // oppure
/*
// GitHub Models via Azure AI Inference
builder.Services.AddHttpClient("ghmodels", c => {
    c.BaseAddress = new Uri("https://models.inference.ai.azure.com/");
    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    c.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2023-10-01");

    var pat =
        builder.Configuration["GITHUB_TOKEN"] ??
        builder.Configuration["Parameters:chat-gh-apikey"] ??
        Environment.GetEnvironmentVariable("GITHUB_TOKEN");

    if (string.IsNullOrWhiteSpace(pat))
        throw new InvalidOperationException("GITHUB_TOKEN mancante.");

    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pat);
});
*/

#endregion

#region Minimal API
builder.Services.AddEndpointDefinitions(); // registra gli endpoint via IEndpointDefinition
#endregion

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

#region CSharpEssentials.Dashboard Package
app.UseCors(CorsPolicy);
app.UseLoggerHelperDashboard<RequestSample>(); // registra la dashboard embedded
#endregion
app.UseHttpsRedirection();

#region Minimal API
app.UseEndpointDefinitions(); // definisce gli endpoint
#endregion


app.Run();