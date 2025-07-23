using BusinessLayer.DataAccess.EntityFramework;
using BusinessLayer.Repository.Github;
using DataAccessLayer.EFStrategy;
using DataAccessLayer.Entities;
using DataAccessLayer.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayer.DataAccess.Configuration;
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddUnitOfWorkInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        var provider = configuration["DatabaseProvider"];
        var connectionString = configuration.GetConnectionString("Default");

        var resolver = new ConnectionStrategyResolver(); // oppure puoi iniettarlo in DI

        services.AddDbContext<ServiceDbContext>(options => {
            var strategy = resolver.Resolve(provider);
            strategy.Configure(options, connectionString);
        });
        services.AddScoped<IRepository<tGitHubOptions>>(sp =>
            new Repository<ServiceDbContext, tGitHubOptions>(
                sp.GetRequiredService<ServiceDbContext>())
        );
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IGitHubOptionsRepo, GitHubOptionsRepo>();

        return services;
    }
}
