using DocumentFormat.OpenXml.Spreadsheet;
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
    public class ServiceStaffRepository : RepositoryBase<ServiceStaff>, IServiceStaffRepository
    {
        public ServiceStaffRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

        public void CreateRecord(ServiceStaff serviceStaff) => Create(serviceStaff);
        public void DeleteRecord(ServiceStaff serviceStaff) => Delete(serviceStaff);
        public async Task<ServiceStaff> GetRecordByIdAsync(int id) =>
            await FindByCondition(c => c.Id.Equals(id))
            .SingleOrDefaultAsync();
        public async Task<ServiceStaff> GetRecordByStaffIdServiceIdAsync(int userId, int serviceId) =>
            await FindByCondition(c => c.StaffId.Equals(userId) && c.ServiceId.Equals(serviceId))
            .SingleOrDefaultAsync();
        public async Task<IEnumerable<int?>> GetServiceIdsForStaffId(int userId) =>
            await FindByCondition(x => x.StaffId.Equals(userId))
            .Select(x => x.ServiceId)
            .ToListAsync();
        public async Task<IEnumerable<int?>> GetStaffIdsForServiceId(int serviceId) =>
           await FindByCondition(x => x.ServiceId.Equals(serviceId))
           .Select(x => x.StaffId)
           .ToListAsync();

        public async Task<IEnumerable<ServiceStaff>> GetServiceStaffForStaffIdAsync(int userId) =>
            await FindByCondition(c => c.StaffId.Equals(userId), true)
            .ToListAsync();

        public async Task<IEnumerable<ServiceStaff>> GetServiceStaffForServiceIdAsync(int serviceId) =>
           await FindByCondition(c => c.ServiceId.Equals(serviceId), true)
           .ToListAsync();
        public void UpdateRecord(ServiceStaff serviceStaff) => Update(serviceStaff);
    }
}
