using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EFStrategy;
public interface IConnectionStrategy {
    void Configure(DbContextOptionsBuilder optionsBuilder, string connectionString);
}
