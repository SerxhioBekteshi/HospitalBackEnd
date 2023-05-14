

using Shared.DTO;
using Shared.RequestFeatures;

namespace Repository.Contracts;

public interface IDapperRepository
{
    public  Task<IEnumerable<ApplicationMenuDTO>> GetChidlrenMenuAsync(string userRole, int userId, int parentId);
    public  Task<IEnumerable<ApplicationMenuDTO>> GetApplicationMenuAsync(string userRole, int userId);
    public  Task<IEnumerable<ApplicationMenuDTO>> AllMenuList();
    public  Task<ApplicationMenuDTO> GetMenuById(int id);
    public Task<int> GetPeopleServedNumber();



    //TABLE PAGGINATION
    public Task<IEnumerable<DeviceDTO>> GetAllDevices(LookupRepositoryDTO filter);
    public Task<PagedList<ReservationTableDTO>> SearchReservations(LookupRepositoryDTO filter);
    public Task<PagedList<ReservationTableDTO>> SearchPostponedReservations(LookupRepositoryDTO filter);
    public Task<PagedList<ReservationTableDTO>> SearchReservationsForStaff(LookupRepositoryDTO filter, int staffId);
    public Task<PagedList<ReservationTableDTO>> SearchSuccededReservations(LookupRepositoryDTO filter, int staffId);
    public Task<PagedList<WorkingHourServiceTableDTO>> SearchAvailableTimes(LookupRepositoryDTO filter);
    public Task<PagedList<WorkingHourServiceTableDTO>> SearchAllServiceTimesForStaff(LookupRepositoryDTO filter, int staffId);
    public Task<PagedList<DeviceDTO>> SearchDevices(LookupRepositoryDTO filter);
    public Task<PagedList<ServiceDTO>> SearchServices(LookupRepositoryDTO filter);
    public Task<PagedList<UserTableDTO>> SearchUsers(LookupRepositoryDTO filter);
    public Task<PagedList<ReportsDeriviedDTO>> SearchReports(LookupRepositoryDTO filter); 
    public Task<PagedList<DelayCountDTO>> GetAllDelaysTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<ReservationTableDTO>> PostponedReservationsTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<DeviceDTO>> DeviceTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<ServiceDTO>> ServicesTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<UserTableDTO>> UsersTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<ReportsDeriviedDTO>> ReportsTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<ReservationTableDTO>> ReservationsTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<DelayCountDTO>> DelaysTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<ReservationTableDTO>> ReservationsTableForStaff(LookupRepositoryDTO filter, int staffId);
    public Task<IEnumerable<ReservationTableDTO>> SucceededReservationsTable(LookupRepositoryDTO filter, int staffId);
    public Task<IEnumerable<WorkingHourServiceTableDTO>> AvailableTimesTable(LookupRepositoryDTO filter);
    public Task<IEnumerable<WorkingHourServiceTableDTO>> ServiceTimesTableForStaff(LookupRepositoryDTO filter, int staffId);
    public Task<IEnumerable<WorkingHourServiceTableDTO>> GetAllTimesAvailableForServiceId(int serviceId);
    public Task<IEnumerable<WorkingHourServiceTableDTO>> GetAllTimesAvailableForReservationServiceId(int serviceId);
    public Task<List<WorkingHourServiceTableDTO>> GetTimesForServiceAndStaff(int serviceId, int staffId);
    public Task<IEnumerable<StaffListOptionDTO>> GetAllStaff(AutocompleteDTO autocomplete);

    //GETS 
    public Task<IEnumerable<ServiceListOptionsDTO>> GetAllServicesAsync(int workerId);
    public Task<IEnumerable<DeviceListDTO>> GetDevicesForServiceId(int serviceId);
    public Task<IEnumerable<ServiceListDTO>> GetServicesForDeviceId(int serviceId);
    public Task<IEnumerable<StaffListDTO>> GetStaffForServiceId(int serviceId);
    public Task<IEnumerable<UserTableDTO>> GetAllStaff(LookupRepositoryDTO filter);
    public Task<IEnumerable<ServiceListDTO>> GetServicesForStaffId(int staffId);
    public Task<IEnumerable<WorkingHourServiceDTO>> GetAllWorkingHoursServicesAsync(LookupRepositoryDTO filter);
    public Task<IEnumerable<WhsrDTO>> GetExistingReservationsWithService(int serviceId);
    public Task<IEnumerable<DwhrsDTO>> GetExistingReservationsWithDevice(int deviceId);
    public Task<IEnumerable<WhsrDTO>> GetExistingReservationsWithUser(int userId);
    public Task<IEnumerable<ServiceListOptionsDTO>> GetAllServices(AutocompleteDTO autocomplete);
    //public Task<IEnumerable<int>> GetServicesIdsForDeviceId(int deviceId);
    public Task<IEnumerable<DeviceListOptionsDTO>> GetAllDevices(AutocompleteDTO autocomplete);
    //public Task<IEnumerable<int>> GetDevicesIdsForServiceId(int serviceId);
    public Task<ReservationTableDTO> GetReservationDetails(int reservationId);
    public Task<IEnumerable<UserTableDTO>> GetWorkers();


    //POST A FEW
    public Task<bool> DeactivateDevices(int[] deviceIds);
    public Task<bool> ActivateDevices(int[] deviceIds);
    public Task<bool> DeactivateServices(int[] serviceIds);
    public Task<bool> ActivateServices(int[] serviceIds);

}