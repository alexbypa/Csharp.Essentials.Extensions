using BusinessLayer.DataAccess.Configuration;
using CSharpEssentials.HttpHelper;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Web.Api.MinimalApi;
using Web.Api.MinimalApi.Mocks;

var builder = WebApplication.CreateBuilder(args);

#region UnitOfWork
builder.Services.AddUnitOfWorkInfrastructure(builder.Configuration);
#endregion


#region CSharpEssentials.HttpHelper Package
builder.Services.AddHttpClients(builder.Configuration, null);
builder.Services.AddHttpClients(builder.Configuration, HttpMocks.CreateHandler());
#endregion



#region Minimal API
builder.Services.AddEndpointDefinitions(); // registra gli endpoint via IEndpointDefinition
#endregion

builder.Services.AddOpenApi();



var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

app.UseHttpsRedirection();

#region Minimal API
app.UseEndpointDefinitions(); // definisce gli endpoint
#endregion

app.Run();