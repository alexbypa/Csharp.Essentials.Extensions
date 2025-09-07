using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessLayer.DataAccess;
public class Repository<TContext, TEntity> : IRepository<TEntity>
    where TContext : DbContext
    where TEntity : class {
    protected readonly TContext _context;
    public Repository(TContext context) {
        _context = context;
    }
    public async Task<TEntity> GetByIdAsync(int id)
        => await _context.Set<TEntity>().FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await Task.FromResult(_context.Set<TEntity>().ToList());

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await Task.FromResult(_context.Set<TEntity>().Where(predicate).ToList());

    public async Task AddAsync(TEntity entity)
        => await _context.Set<TEntity>().AddAsync(entity);

    public void Remove(TEntity entity)
        => _context.Remove(entity);
}