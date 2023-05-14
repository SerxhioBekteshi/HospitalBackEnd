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
    public class DeviceServiceRepository :RepositoryBase<ServiceDevice> , IDeviceServiceRepository
    {
        public DeviceServiceRepository(RepositoryContext repositoryContext)
       : base(repositoryContext)
        {
        }

        public void CreateRecord(ServiceDevice deviceService) => Create(deviceService);
        public void DeleteRecord(ServiceDevice deviceService) => Delete(deviceService);
        public async Task<ServiceDevice> GetRecordByIdAsync(int id) =>
            await FindByCondition(c => c.Id.Equals(id))
            .SingleOrDefaultAsync();
        public async Task<ServiceDevice> GetRecordByDeviceIdServiceIdAsync(int deviceId, int serviceId) =>
            await FindByCondition(c => c.DeviceId.Equals(deviceId) && c.ServiceId.Equals(serviceId))
            .SingleOrDefaultAsync();

        public async Task<int> GetCounterDeviceByServiceIdAsync(int serviceId) =>
          await FindByCondition(c => c.ServiceId.Equals(serviceId)).Select(c => c.Counter)
          .SingleOrDefaultAsync();

        public async Task<IEnumerable<int?>> GetDeviceIdsForServiceId(int serviceId) =>
            await FindByCondition(x => x.ServiceId.Equals(serviceId))
            .Select(x => x.DeviceId)
            .ToListAsync();

        public async Task<IEnumerable<int?>> GetServiceIdsForDeviceId(int deviceId) =>
            await FindByCondition(x => x.DeviceId.Equals(deviceId))
            .Select(x => x.ServiceId)
            .ToListAsync();
        public async Task<IEnumerable<ServiceDevice>> GetDeviceServiceForServiceIdAsync(int serviceId) =>
            await FindByCondition(c => c.ServiceId.Equals(serviceId), true)
            .ToListAsync();

        public async Task<IEnumerable<ServiceDevice>> GetDeviceServiceForDeviceIdAsync(int deviceId) =>
           await FindByCondition(c => c.DeviceId.Equals(deviceId), true)
           .ToListAsync();
        public void UpdateRecord(ServiceDevice deviceService) => Update(deviceService);
    }
}
