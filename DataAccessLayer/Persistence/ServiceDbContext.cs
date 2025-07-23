using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Persistence;
public class ServiceDbContext : DbContext {
    public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }
    public DbSet<tGitHubOptions> tGitHubOptions { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceDbContext).Assembly);
    }
}