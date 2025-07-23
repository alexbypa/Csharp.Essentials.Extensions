using BusinessLayer.DataAccess;
using BusinessLayer.DataAccess.EntityFramework;
using BusinessLayer.Repository.Github;
using CSharpEssentials.HttpHelper;
using DataAccessLayer.Entities;
using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Web.Api.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

#region UnitOfWork
builder.Services.AddDbContext<ServiceDbContext>(options => {
    // Usa qui il tuo strategy resolver se serve per ambiente dinamico
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<IRepository<tGitHubOptions>>(sp =>
    new Repository<ServiceDbContext, tGitHubOptions>(
        sp.GetRequiredService<ServiceDbContext>())
);
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IGitHubOptionsRepo, GitHubOptionsRepo>();
#endregion

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