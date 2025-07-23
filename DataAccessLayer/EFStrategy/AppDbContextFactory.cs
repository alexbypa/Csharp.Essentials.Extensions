using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.EFStrategy;
public class AppDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext> {
    private readonly IConnectionStrategyResolver _resolver;
    public AppDbContextFactory() : this(new ConnectionStrategyResolver()) { }
    public AppDbContextFactory(IConnectionStrategyResolver resolver) {
        _resolver = resolver;
    }

    public ServiceDbContext CreateDbContext(string[] args) {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var provider = config["DatabaseProvider"];
        var connectionString = config.GetConnectionString("Default");

        var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
        _resolver.Resolve(provider).Configure(optionsBuilder, connectionString);

        return new ServiceDbContext(optionsBuilder.Options);
    }
}
