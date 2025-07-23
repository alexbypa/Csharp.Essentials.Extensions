using DataAccessLayer.Entities;

namespace BusinessLayer.Repository.Github;

public interface IGitHubOptionsRepo {
    Task<IEnumerable<tGitHubOptions>> GetByUserName(string UserName);
}