using CSharpEssentials.HttpHelper;
using Web.Api.MinimalApi;
using BusinessLayer.DataAccess.Configuration;
using Web.Api.MinimalApi.Mocks;

var builder = WebApplication.CreateBuilder(args);

#region UnitOfWork
builder.Services.AddUnitOfWorkInfrastructure(builder.Configuration);
#endregion

builder.Services.AddHttpClient("Test1").ConfigurePrimaryHttpMessageHandler(() => HttpMocks.CreateHandler()); // usa il mock per i test

#region CSharpEssentials.HttpHelper Package
builder.Services.AddHttpClients(builder.Configuration);
#endregion


#region Minimal API
builder.Services.AddEndpointDefinitions(); // registra gli endpoint via IEndpointDefinition
#endregion

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

#region Minimal API
app.UseEndpointDefinitions(); // definisce gli endpoint
#endregion

app.Run();