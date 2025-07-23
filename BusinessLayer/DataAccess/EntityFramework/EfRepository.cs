using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessLayer.DataAccess.EntityFramework;
public class EfRepository<T> : IRepository<T> where T : class {
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;
    public EfRepository(DbContext context) {
        _context = context;
        _dbSet = context.Set<T>();
    }
    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public Task AddAsync(T entity) => _dbSet.AddAsync(entity).AsTask();
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
    public IQueryable<T> Query() => _dbSet;
    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) {
        throw new NotImplementedException();
    }
}
