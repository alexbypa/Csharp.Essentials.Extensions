using BusinessLayer.DataAccess;
using DataAccessLayer.Entities;
using DataAccessLayer.Persistence;

namespace BusinessLayer.Repository.Github;
public class GitHubOptionsRepo : Repository<ServiceDbContext, tGitHubOptions>, IGitHubOptionsRepo {
    public GitHubOptionsRepo(ServiceDbContext context) : base(context) {}
    public async Task<IEnumerable<tGitHubOptions>> GetByUserName(string UserName) 
        => (await FindAsync(opt => opt.UserName == UserName)).ToList();
}