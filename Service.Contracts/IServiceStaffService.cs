using Entities.Models;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IServiceStaffService
    {
        public Task<bool> CreateRecord(ServiceStaffDTO createServiceStaffDTO, int userId);
        public Task<bool> DeleteRecord(int id);
        public Task<bool> UpdateRecord(ServiceStaffDTO updateServiceStaffDTO, int id, int userId);
    }
}
