using Entities.Models;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IDeviceServiceService
    {
        public Task<bool> CreateRecord(DeviceServiceDTO createDeviceServiceDTO, int userId);
        public Task<bool> DeleteRecord(int id);
        public Task<bool> UpdateRecord(DeviceServiceDTO updateDeviceServiceDTO, int id, int userId);
    }
}
