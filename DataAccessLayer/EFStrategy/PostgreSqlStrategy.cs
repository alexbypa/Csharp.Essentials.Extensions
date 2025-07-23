using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFStrategy;
public class PostgreSqlStrategy : IConnectionStrategy {
    public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        => optionsBuilder.UseNpgsql(connectionString);
}