namespace BusinessLayer.DataAccess.EntityFramework;
public interface IUnitOfWork : IAsyncDisposable {
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}