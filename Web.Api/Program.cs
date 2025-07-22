using CSharpEssentials.HttpHelper;
using Web.Api.MinimalApi;


var builder = WebApplication.CreateBuilder(args);

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