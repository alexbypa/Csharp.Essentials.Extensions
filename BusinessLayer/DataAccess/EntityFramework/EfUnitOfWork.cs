using DataAccessLayer.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.DataAccess.EntityFramework;
public class EfUnitOfWork : IUnitOfWork {
    private readonly ServiceDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();
    public EfUnitOfWork(ServiceDbContext context) {
        _context = context;
    }
    public IRepository<T> Repository<T>() where T : class {
        if (_repositories.TryGetValue(typeof(T), out var repo))
            return (IRepository<T>)repo;

        var newRepo = new EfRepository<T>(_context);
        _repositories[typeof(T)] = newRepo;
        return newRepo;
    }
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    public async ValueTask DisposeAsync() => await _context.DisposeAsync();
}