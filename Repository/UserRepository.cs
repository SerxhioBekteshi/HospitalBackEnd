using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository;

public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
    {
    }
    public void CreateRecord(ApplicationUser user) => Create(user);
    public void UpdateRecord(ApplicationUser user) => Update(user);
    public void DeleteRecord(ApplicationUser user) => Delete(user);
    public async Task<ApplicationUser> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id)).SingleOrDefaultAsync();
}