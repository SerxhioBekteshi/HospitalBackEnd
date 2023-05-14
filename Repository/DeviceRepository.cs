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
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(RepositoryContext repositoryContext)
          : base(repositoryContext)
        {
        }
        public void CreateRecord(Device device) => Create(device);
        public void DeleteRecord(Device device) => Delete(device);
        public async Task<Device> GetRecordByIdAsync(int id) =>
        await FindByCondition(c => c.Id.Equals(id))
              .SingleOrDefaultAsync();
        public void UpdateRecord(Device device) => Update(device);
        public async Task<IEnumerable<Device>> GetAll() => await FindAll().ToListAsync();

    }
}
