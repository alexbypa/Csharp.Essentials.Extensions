using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.EFStrategy;

public class SqlServerStrategy : IConnectionStrategy {
    public void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString)
        => optionsBuilder.UseSqlServer(connectionString);
}