using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IDeviceServiceRepository
    {
        public void CreateRecord(ServiceDevice deviceService);
        public void DeleteRecord(ServiceDevice deviceService);
        public Task<ServiceDevice> GetRecordByIdAsync(int id);
        public Task<ServiceDevice> GetRecordByDeviceIdServiceIdAsync(int deviceId, int serviceId);
        public Task<IEnumerable<int?>> GetDeviceIdsForServiceId(int serviceId);
        public Task<IEnumerable<int?>> GetServiceIdsForDeviceId(int deviceId);
        public Task<IEnumerable<ServiceDevice>> GetDeviceServiceForServiceIdAsync(int serviceId);
        public Task<IEnumerable<ServiceDevice>> GetDeviceServiceForDeviceIdAsync(int deviceId);
        public void UpdateRecord(ServiceDevice deviceService);
        public Task<int> GetCounterDeviceByServiceIdAsync(int serviceId);

    }
}
