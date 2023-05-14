using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DelayRepository : RepositoryBase<Delay>, IDelayRepository
    {
        public DelayRepository(RepositoryContext repositoryContext)
          : base(repositoryContext)
        {
        }
        public void CreateRecord(Delay delay) => Create(delay);
        public void DeleteRecord(Delay delay) => Delete(delay);
        public async Task<Delay> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
              .SingleOrDefaultAsync();
        public async Task<IEnumerable<Delay>> GetAll() => await FindAll().ToListAsync();

    }
}
