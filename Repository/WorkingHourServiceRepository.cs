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
    public class WorkingHourServiceRepository : RepositoryBase<WorkingHourService>, IWorkingHourServiceRepository
    {
        public WorkingHourServiceRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public void CreateRecord(WorkingHourService workingHourService) => Create(workingHourService);
        public void DeleteRecord(WorkingHourService workingHourService) => Delete(workingHourService);
        public async Task<WorkingHourService> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
             .SingleOrDefaultAsync();

        public void UpdateRecord(WorkingHourService workingHourService) => Update(workingHourService);

    }
}
