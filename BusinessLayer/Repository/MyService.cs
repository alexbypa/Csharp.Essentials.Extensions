using BusinessLayer.DataAccess.EntityFramework;
using DataAccessLayer.Entities;

namespace BusinessLayer.Services;
public class MyService {
    private readonly IUnitOfWork _uow;

    public MyService(IUnitOfWork uow) {
        _uow = uow;
    }
    public async Task DoWork() {
        var repo = _uow.Repository<tGitHubOptions>();
        var all = await repo.GetAllAsync();
        await Task.Delay(100);
        //await _uow.SaveChangesAsync();
    }
}
