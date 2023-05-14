using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IWorkingHourServiceService
    {
        public Task<bool> CreateRecord(WorkingHourServiceDTO createWorkingHourServiceDTO, int userId);
        public Task<bool> DeleteRecord(int id);
        public Task<bool> UpdateRecord(WorkingHourServiceDTO updateWorkingHourServiceDTO, int id, int userId);
        public Task<WorkingHourServiceDTO> GetRecordById(int id);
        public Task<PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>> GetAllWorkingHoursServicesAsync(LookupRepositoryDTO filter);
        public Task<PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>> GetAllTimeServicesTableForStaff(LookupRepositoryDTO filter, int staffId);
        public Task<IEnumerable<WorkingHourServiceTableDTO>> GetTimesForServiceId(int id);


    }
}
