using System.Linq.Expressions;

namespace BusinessLayer.DataAccess;
public interface IRepository<TEntity> where TEntity : class {
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    void Remove(TEntity entity);
}