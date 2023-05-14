using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IServiceStaffRepository
    {
        public void CreateRecord(ServiceStaff serviceStaff);
        public void DeleteRecord(ServiceStaff serviceStaff);
        public Task<ServiceStaff> GetRecordByIdAsync(int id);
        public Task<ServiceStaff> GetRecordByStaffIdServiceIdAsync(int userId, int serviceId);
        public Task<IEnumerable<int?>> GetServiceIdsForStaffId(int userId);
        public Task<IEnumerable<ServiceStaff>> GetServiceStaffForStaffIdAsync(int userId);
        public void UpdateRecord(ServiceStaff serviceStaff);
        public Task<IEnumerable<int?>> GetStaffIdsForServiceId(int serviceId);
        public Task<IEnumerable<ServiceStaff>> GetServiceStaffForServiceIdAsync(int serviceId);

    }
}
