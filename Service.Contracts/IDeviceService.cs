using Entities.Models;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync(LookupRepositoryDTO filter);
        Task<bool> CreateRecord(DeviceDTO createDeviceDto, int userId);
        public Task<bool> DeleteRecord(int id, int userId);
        Task<bool> UpdateRecord(DeviceDTO updateDeviceDTO, int id, int userId);
        Task<DeviceDTO> GetRecordById(int id);
        //Task<PagedListResponse<IEnumerable<DeviceDTO>>> GetAllRecords(LookupRepositoryDTO filter);
        public Task<bool> PostServicesToDevice(int deviceId, PostServicesToDeviceDTO postServicesToDevice, int managerId);
        public Task<bool> DeactivateDeviceAsync(int[] deviceIds, int userId);
        public Task<bool> ActivateDeviceAsync(int[] deviceIds);
        public Task<PagedListResponse<IEnumerable<DeviceDTO>>> GetAllDevicesTable(LookupRepositoryDTO filter);
        public Task<IEnumerable<ServiceListOptionsDTO>> GetAllServices(AutocompleteDTO autocomplete);

    }
}
