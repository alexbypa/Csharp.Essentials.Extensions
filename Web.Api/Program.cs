using BusinessLayer.Application;
using BusinessLayer.Contracts.Context;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.AI.Infrastructure;
using CSharpEssentials.LoggerHelper.Configuration;
using CSharpEssentials.LoggerHelper.Dashboard.Extensions;
using CSharpEssentials.LoggerHelper.Telemetry.Configuration;
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
    builder.Services.AddCSharpEssentialsLoggerAI(builder.Configuration,
            SqlAiPersistenceFactory.AddSqlAiPersistence(builder.Configuration) // <-- Passa l'Action<IServiceCollection>
);
#endregion


// -> Registra il client per comunicare con il modello di linguaggio (es. OpenAI).
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

//AI EndPoint Dashboard
// Program.cs

app.MapAiEndpoints();
//AI EndPoint Dashboard

app.Run();
