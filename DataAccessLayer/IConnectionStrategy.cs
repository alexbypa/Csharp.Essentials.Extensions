using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer;
public interface IConnectionStrategy {
    void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString);
}
public class SqlServerStrategy : IConnectionStrategy {
    public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        => optionsBuilder.UseSqlServer(connectionString);
}

public class PostgreSqlStrategy : IConnectionStrategy {
    public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        => optionsBuilder.UseNpgsql(connectionString);
}
public static class ConnectionStrategyFactory {
    public static IConnectionStrategy Get(string provider) {
        return provider.ToLower() switch {
            "sqlserver" => new SqlServerStrategy(),
            "postgresql" => new PostgreSqlStrategy(),
            _ => throw new NotSupportedException($"Provider '{provider}' not supported.")
        };
    }
}
public class AppDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext> {
    public ServiceDbContext CreateDbContext(string[] args) {
        var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var config = new ConfigurationBuilder()
            .AddJsonFile(jsonPath, optional: false)
            .Build();

        var provider = config["DatabaseProvider"];  // es: "sqlserver" o "postgresql"
        var connectionString = config.GetConnectionString("Default");

        var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
        var strategy = ConnectionStrategyFactory.Get(provider);
        strategy.Configure(optionsBuilder, connectionString);

        return new ServiceDbContext(optionsBuilder.Options);
    }
}
