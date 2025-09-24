using BusinessLayer.Application;
using BusinessLayer.Contracts.Context;
using BusinessLayer.Mocks;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.AI.Application;
using CSharpEssentials.LoggerHelper.AI.Domain;
using CSharpEssentials.LoggerHelper.AI.Infrastructure;
using CSharpEssentials.LoggerHelper.AI.Ports;
using CSharpEssentials.LoggerHelper.Configuration;
using CSharpEssentials.LoggerHelper.Dashboard.Extensions;
using CSharpEssentials.LoggerHelper.model;
using CSharpEssentials.LoggerHelper.Telemetry.Configuration;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Npgsql;
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region LoggerHelper.AI Package 

builder.Services
    .AddOptions<LoggerAIOptions>()
    .Bind(builder.Configuration.GetSection("LoggerAIOptions"))
    .ValidateDataAnnotations()
    .ValidateOnStart();


// --- SEZIONE DATABASE ---
// Qui rendiamo l'applicazione flessibile, in grado di passare da un database
// all'altro cambiando solo una riga nel file di configurazione appsettings.json.
// -> Legge il valore "DatabaseProvider" dalla configurazione.
if (builder.Configuration.GetValue<string>("DatabaseProvider")!.Contains("postgresql", StringComparison.InvariantCultureIgnoreCase)) {
    builder.Services.AddScoped(_ => new NpgsqlConnection(builder.Configuration.GetConnectionString("Default")));
    builder.Services.AddScoped<IWrapperDbConnection>(_ => new FactoryPostgreSqlConnection(builder.Configuration.GetConnectionString("Default")!));
} else {
    builder.Services.AddScoped(_ => new SqlConnection(builder.Configuration.GetConnectionString("Default")));
    builder.Services.AddScoped<IWrapperDbConnection>(_ => new FactorySQlConnection(builder.Configuration.GetConnectionString("Default")!));
}

// --- SEZIONE REPOSITORY (Livello Accesso Dati) ---
// I repository sono classi che contengono la logica per interrogare il database.
// Dipendono da 'IWrapperDbConnection' che abbiamo registrato sopra.
// -> Quando una classe chiede 'ILogRepository', gli viene data un'istanza di 'SqlLogRepository'.
builder.Services.AddScoped<ILogRepository, SqlLogRepository>();
builder.Services.AddScoped<ITraceRepository, SqlTraceRepository>();
builder.Services.AddScoped<IMetricRepository, SqlMetricRepository>();

// -> Registra il servizio per creare gli embedding (vettori numerici dal testo).
builder.Services.AddScoped<IEmbeddingService, NaiveEmbeddingService>();

// -> Registra il nostro "costruttore di dati" da file. È 'Transient' perché è leggero,
//    senza stato, e vogliamo un'istanza nuova ogni volta che viene usato.
builder.Services.AddTransient<FileLogIndexer>(); // se vuoi usarlo per popolare il vettore store da file

builder.Services.AddTransient<IFileLoader, FileLoader>();

var serviceProvider = builder.Services.BuildServiceProvider();
var fileLoader = serviceProvider.GetRequiredService<IFileLoader>();
var sqlModels = fileLoader.getModelSQLLMModels();
builder.Services.AddSingleton(sqlModels);

builder.Services.AddScoped<ILogVectorStore, SqlLogVectorStore>();
//builder.Services.AddScoped<ILogVectorStore, InMemoryLogVectorStore>();

//builder.Services.AddHostedService<VectorStoreInitializationService>();//TODO:

// -> Registriamo più implementazioni per la stessa interfaccia 'ILogMacroAction'.
//    Questo permette all'orchestratore di riceverle tutte in una lista e decidere quale usare.
builder.Services.AddScoped<ILogMacroAction, SummarizeIncidentAction>();
builder.Services.AddScoped<ILogMacroAction, CorrelateTraceAction>();
builder.Services.AddScoped<ILogMacroAction, DetectAnomalyAction>();
builder.Services.AddScoped<ILogMacroAction, RagAnswerQueryAction>();

// -> Registra l'orchestratore, la classe che gestisce e coordina tutte le azioni.
builder.Services.AddScoped<IActionOrchestrator, ActionOrchestrator>();

builder.Services.AddScoped<ILlmChat, OpenAiLlmChat>(); // oppure
#endregion
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
