using BusinessLayer.Application;
using BusinessLayer.Contracts;
using BusinessLayer.Contracts.Context;
using CSharpEssentials.HttpHelper;
using CSharpEssentials.LoggerHelper;
using CSharpEssentials.LoggerHelper.AI.Infrastructure;
using CSharpEssentials.LoggerHelper.Configuration;
using CSharpEssentials.LoggerHelper.Dashboard.Extensions;
using CSharpEssentials.LoggerHelper.Telemetry.Configuration;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;
using Web.Api.MinimalApi;
using Web.Api.MinimalApi.Endpoints.Telemetries;

var builder = WebApplication.CreateBuilder(args);

/* --TODO:
var settings = builder.Configuration
                      .GetSection(nameof(FeatureSettings)) // Ad esempio, se è sotto "FeatureSettings" in appsettings.json
                      .Get<FeatureSettings>() ?? new FeatureSettings();

// 2. Registrazione della configurazione stessa (opzionale, ma consigliato)
// Inietta l'istanza di configurazione in DI.
builder.Services.AddSingleton(settings);

// 3. Esecuzione del metodo di validazione condizionale e registrazione
// Chiama il metodo di estensione sul container di servizi, passando l'istanza della configurazione.
builder.Services.AddCustomValidatedServices(settings);
*/


//TEMP ONLY FOR DEBUG
/*
Console.ForegroundColor = ConsoleColor.Yellow;

Console.WriteLine("========================================================================");
Console.WriteLine("ConnectionStrings:Default");
Console.WriteLine(builder.Configuration.GetValue<string>("ConnectionStrings:Default"));
Console.WriteLine("Serilog:SerilogConfiguration:LoggerTelemetryOptions:ConnectionString");
Console.WriteLine(builder.Configuration.GetValue<string>("Serilog:SerilogConfiguration:LoggerTelemetryOptions:ConnectionString"));

string ConnectionStrings = builder.Configuration.GetValue<string>("ConnectionStrings:Default");
var csb = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(ConnectionStrings) {
    InitialCatalog = "master" // <-- LA CORREZIONE AVVIENE QUI
};
ConnectionStrings = csb.ConnectionString;
Console.WriteLine($"new ConnectionString : {ConnectionStrings}");
var maxRetries = 15;
var delaySeconds = 3;

for (int i = 0; i < maxRetries; i++) {
    try {
        // Tentativo di connessione e SELECT 1
        using (var conn = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStrings)) {
            conn.Open();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[DB READY] Connessione a MSSQL MASTER riuscita dopo {i * delaySeconds}s.");
            Console.ResetColor();
            break;
        }
    } catch (Microsoft.Data.SqlClient.SqlException) {
        // Cattura l'eccezione e riprova
        Console.WriteLine($"[RETRY {i + 1}/{maxRetries}] MSSQL non pronto. Attendo {delaySeconds}s...");
        Task.Delay(TimeSpan.FromSeconds(delaySeconds)).Wait();
    }
}
Console.WriteLine("========================================================================");
Console.ForegroundColor = ConsoleColor.White;
*/


#region CSharpEssentials.HttpHelper Package
builder.Services.AddHttpClients(builder.Configuration); //if you dont use Moq
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
app.UseLoggerHelperDashboard<RequestSample>("admin"); // registra la dashboard embedded
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
