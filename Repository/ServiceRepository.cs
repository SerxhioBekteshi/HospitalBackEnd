using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
namespace Repository;

public class ServiceRepository : RepositoryBase<Services>, IServiceRepository
{
    public ServiceRepository(RepositoryContext repositoryContext)
      : base(repositoryContext)
    {
    }
    public void CreateRecord(Services service) => Create(service);
    public void DeleteRecord(Services service) => Delete(service);
    public async Task<Services> GetRecordByIdAsync(int id) =>
    await FindByCondition(c => c.Id.Equals(id))
          .SingleOrDefaultAsync();
    public void UpdateRecord(Services service) => Update(service);
    public async Task<IEnumerable<Services>> GetAllRecordsAsync() => await FindAll().ToListAsync();
}