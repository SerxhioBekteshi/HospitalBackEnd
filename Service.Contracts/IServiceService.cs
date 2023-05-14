using Entities.Models;
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
    public interface IServiceService
    {
        Task<IEnumerable<ServiceListOptionsDTO>> GetAllServicesAsync(int workerId);
        //Task<bool> UploadImage(UploadIconDTO uploadImageDto, int id, int userId);
        Task<bool> CreateRecord(ServiceDTO addDeviceDTO, int userId);
        Task<bool> UpdateRecord(ServiceDTO updateDeviceDTO, int id, int userId);
        Task<bool> DeleteRecord(int id, int userId);
        Task<ServiceDTO> GetRecordById(int id);

        //Task<IEnumerable<DeviceListDTO>> GetAllDevicesInAService(int serviceId);
        Task<bool> PostDevicesToService(int serviceId, PostDevicesToServiceDTO postDevicesToService, int managerId);
        Task<bool> PostStaffToServiceId(int serviceId, PostStaffToServiceDTO postStaffToService, int managerId);
        Task<bool> DeactivateServiceAsync(int[] serviceIds, int userId);
        Task<bool> ActivateServiceAsync(int[] serviceIds);
        Task<PagedListResponse<IEnumerable<ServiceDTO>>> GetAllServicesTable(LookupRepositoryDTO filter);
        Task<IEnumerable<DeviceListOptionsDTO>> GetAllDevices(AutocompleteDTO autocomplete);
        Task<IEnumerable<StaffListOptionDTO>> GetAllStaff(AutocompleteDTO autocomplete);

    }
}
