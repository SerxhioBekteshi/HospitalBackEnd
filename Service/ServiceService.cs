using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Asn1.Ocsp;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTC;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Types;

namespace Service
{
    public class ServiceService  : IServiceService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;
        private readonly IEmailSender _emailSender;

        public ServiceService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository, IEmailSender emailSender)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
            _emailSender = emailSender;
        }

        public async Task<IEnumerable<ServiceListOptionsDTO>> GetAllServicesAsync(int workerId)
        {
            try
            {
                var services = await _dapperRepository.GetAllServicesAsync(workerId);
                return services;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(ServiceListOptionsDTO), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<ServiceDTO>>> GetAllServicesTable(LookupRepositoryDTO filter)
        {
            try
            {
                var servicesWithMetaData = await _dapperRepository.SearchServices(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ServiceDTO>> response = new PagedListResponse<IEnumerable<ServiceDTO>>
                {
                    TotalCount = servicesWithMetaData.MetaData.TotalCount,
                    CurrentPage = servicesWithMetaData.MetaData.CurrentPage,
                    PageSize = servicesWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = servicesWithMetaData,
                    HasNext = servicesWithMetaData.MetaData.HasNext,
                    HasPrevious = servicesWithMetaData.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllServicesTable), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<bool> CreateRecord(ServiceDTO serviceDTO, int userId)
        {
            try
            {
                var service = _mapper.Map<Services>(serviceDTO);

                service.DateCreated = DateTime.UtcNow;
                service.CreatedBy = userId;
                service.IsActive = true;

                _repositoryManager.ServiceRepository.CreateRecord(service);
                await _repositoryManager.SaveAsync();

                if (serviceDTO.DeviceIds != null)
                {
                    await CreateServiceDeviceRelation(serviceDTO.DeviceIds, service.Id, userId);
                }
                if (serviceDTO.StaffIds != null)
                {
                    await CreateServiceStaffRelation(serviceDTO.StaffIds, service.Id, userId);
                }
                await _repositoryManager.SaveAsync();
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<ServiceDTO> GetRecordById(int id)
        {
            try
            {
                var existingService = await GetServiceAndCheckIfExistsAsync(id);
                var serviceIds = await _repositoryManager.DeviceServiceRepository.GetDeviceIdsForServiceId(id);
                var staffIds = await _repositoryManager.ServiceStaffRepository.GetStaffIdsForServiceId(id);

                var serviceDto = _mapper.Map<ServiceDTO>(existingService);
                serviceDto.DeviceIds = (List<int?>?)serviceIds;

                var staffDTO = _mapper.Map<ServiceDTO>(existingService);
                staffDTO.StaffIds = (List<int?>?)staffIds;

                return serviceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<bool> UpdateRecord(ServiceDTO serviceDTO, int id, int userId)
        {
            try
            {
                var existingService = await GetServiceAndCheckIfExistsAsync(id);
                var existingDeviceService = existingService.DeviceService;
                var existingServiceStaff = existingService.ServiceStaff;

                if (serviceDTO.DeviceIds.Count != 0)
                {
                    await UpdateServiceDevice(existingDeviceService, serviceDTO, id, userId);
                }
                if (serviceDTO.StaffIds.Count != 0)
                {
                    await UpdateServiceStaff(existingServiceStaff, serviceDTO, id, userId);

                }

                _mapper.Map(serviceDTO, existingService);

                existingService.DateModified = DateTime.UtcNow;
                existingService.ModifiedBy = userId;

                _repositoryManager.ServiceRepository.UpdateRecord(existingService);
                await _repositoryManager.SaveAsync();


                if(serviceDTO.IsActive == false)
                {
                    int[] serviceIds = {id};
                    await PostponeReservation(serviceIds, userId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> DeleteRecord(int id, int userId)
        {
            try
            {
                var existingService = await GetServiceAndCheckIfExistsAsync(id);
                if (existingService != null)
                {
                    await DeleteServiceDeviceRelationForServiceId(id);
                    await _repositoryManager.SaveAsync();
                    await DeleteServiceStaffRelationForServiceId(id);
                    await _repositoryManager.SaveAsync();
                    await CancelReservationAndUpdateStatusForWorkingHourService(id, userId);
                    _repositoryManager.ServiceRepository.DeleteRecord(existingService);
                    await _repositoryManager.SaveAsync();

                }
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> PostDevicesToService(int serviceId, PostDevicesToServiceDTO postDevicesToService, int managerId)
        {
            try
            {
                var existingService = await GetServiceAndCheckIfExistsAsync(serviceId);

                if (existingService is null)
                    throw new NotFoundException(string.Format("Pika fizike me id: {0} nuk u gjet!", serviceId));

                _mapper.Map(postDevicesToService, existingService);

                var devicesForServiceId = await _dapperRepository.GetDevicesForServiceId(serviceId);

                var devicesToAdd = postDevicesToService?.DevicesIds?.Where(x => !devicesForServiceId.Equals(x)).ToList();
                await CreateServiceDeviceRelation(devicesToAdd, serviceId, managerId);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(PostDevicesToService), ex.Message));
                throw new BadRequestException(ex.Message);
            }

        }

        public async Task<bool> PostStaffToServiceId(int serviceId, PostStaffToServiceDTO postStaffToService, int managerId)
        {
            try
            {
                var existingService = await GetServiceAndCheckIfExistsAsync(serviceId);

                if (existingService is null)
                    throw new NotFoundException(string.Format("Pika fizike me id: {0} nuk u gjet!", serviceId));

                _mapper.Map(postStaffToService, existingService);

                var staffForServiceId = await _dapperRepository.GetStaffForServiceId(serviceId);

                var staffToAdd = postStaffToService?.StaffIds?.Where(x => !staffForServiceId.Equals(x)).ToList();
                await CreateServiceStaffRelation(staffToAdd, serviceId, managerId);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(PostDevicesToService), ex.Message));
                throw new BadRequestException(ex.Message);
            }

        }

        public async Task<bool> DeactivateServiceAsync(int[] serviceIds, int userId)
        {
            try
            {
                await _dapperRepository.DeactivateServices(serviceIds);
                await PostponeReservation(serviceIds, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeactivateServiceAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> ActivateServiceAsync(int[] serviceIds)
        {
            try
            {
                await _dapperRepository.ActivateServices(serviceIds);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(ActivateServiceAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<IEnumerable<DeviceListOptionsDTO>> GetAllDevices(AutocompleteDTO autocomplete)
        {
            try
            {
                var devices = await _dapperRepository.GetAllDevices(autocomplete);
                return devices;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllDevices), ex.Message));
                throw new BadRequestException(ex.Message);
            }

        }

        public async Task<IEnumerable<StaffListOptionDTO>> GetAllStaff(AutocompleteDTO autocomplete)
        {
            try
            {
                var staff = await _dapperRepository.GetAllStaff(autocomplete);
                return staff;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllStaff), ex.Message));
                throw new BadRequestException(ex.Message);
            }

        }
        
        //public async Task<PagedListResponse<IEnumerable<ServiceDTO>>> GetAllRecords(LookupRepositoryDTO filter)
        //{
        //    try
        //    {
        //        var servicesWithMetaData = await _dapperRepository.SearchServices(filter);
        //        var servicesDtoList = _mapper.Map<IEnumerable<ServiceDTO>>(servicesWithMetaData);
        //        var columns = GetDataTableColumns();

        //        PagedListResponse<IEnumerable<ServiceDTO>> response = new PagedListResponse<IEnumerable<ServiceDTO>>
        //        {
        //            RowCount = servicesWithMetaData.MetaData.TotalCount,
        //            Page = servicesWithMetaData.MetaData.CurrentPage,
        //            PageSize = servicesWithMetaData.MetaData.PageSize,
        //            Columns = columns,
        //            Rows = servicesDtoList,
        //            HasNext = servicesWithMetaData.MetaData.HasNext,
        //            HasPrevious = servicesWithMetaData.MetaData.HasPrevious,
        //            TotalPages = servicesWithMetaData.MetaData.TotalPages
        //        };
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
        //        throw new BadRequestException(ex.Message);
        //    }
        //}

        //private List<DataTableColumn> GetDataTableColumns()
        //{
        //    // get the columns
        //    var columns = GenerateDataTableColumn<ServiceColumn>.GetDataTableColumns();

        //    // return all columns
        //    return columns;
        //}

        #region PrivateMethods
        private async Task<Services> GetServiceAndCheckIfExistsAsync(int id)
        {
            var existingCategory = await _repositoryManager.ServiceRepository.GetRecordByIdAsync(id);
            if (existingCategory is null)
                throw new NotFoundException(string.Format($"Sherbimi me Id: {id} nuk u gjet!"));

            return existingCategory;
        }

        private async Task CreateServiceDeviceRelation(List<int?>? deviceIds, int serviceId, int userId)
        {
            if (deviceIds is not null)
            {
                foreach (var deviceId in deviceIds)
                {
                    //var existingDevices = await _repositoryManager.DeviceServiceRepository.GetDeviceServiceForDeviceIdAsync((int)deviceId);
                    //foreach(var dd in existingDevices)
                    //{
                    //    _repositoryManager.DeviceServiceRepository.DeleteRecord(dd);

                    //}
                    var newServiceDevice = new ServiceDevice
                    {
                        ServiceId = serviceId,
                        DeviceId = deviceId,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = userId
                    };
                    _repositoryManager.DeviceServiceRepository.CreateRecord(newServiceDevice);
                   
                }
            }
        }
        private async Task CreateServiceStaffRelation(List<int?>? staffIds, int serviceId, int userId)
        {
            if (staffIds is not null)
            {
                foreach (var staffId in staffIds)
                {
                    //DUHET BERE CHECK NQS EKZISTON KJO DEVICE ID DHE NQS ESHTE AKTIVE
                    var newServiceStaff = new ServiceStaff
                    {
                        ServiceId = serviceId,
                        StaffId = staffId,
                        DateCreated = DateTime.UtcNow,
                        CreatedBy = userId
                    };
                    _repositoryManager.ServiceStaffRepository.CreateRecord(newServiceStaff);

                }
            }
        }

        private async Task DeleteServiceDeviceRelationForServiceId(int serviceId)
        {
            var existingServiceDevice = await _repositoryManager.DeviceServiceRepository.GetDeviceServiceForServiceIdAsync(serviceId);
            
                foreach (var existingDevice in existingServiceDevice)
                {
                    _repositoryManager.DeviceServiceRepository.DeleteRecord(existingDevice);
                }
            
       
          
        }

        private async Task DeleteServiceStaffRelationForServiceId(int serviceId)
        {
            var existingServiceStaff = await _repositoryManager.ServiceStaffRepository.GetServiceStaffForServiceIdAsync(serviceId);
            foreach (var existingStaff in existingServiceStaff)
            {
                _repositoryManager.ServiceStaffRepository.DeleteRecord(existingStaff);
            }
        }


        private async Task CancelReservationAndUpdateStatusForWorkingHourService (int serviceId, int userId)
        {
            var existingReservationWithService = await _dapperRepository.GetExistingReservationsWithService(serviceId);
            foreach(var existing in existingReservationWithService)
            {
                var existingReservation = await _repositoryManager.ReservationRepository.GetRecordByIdAsync(existing.ReservationId);
                existingReservation.Status = ReservationStatusEnum.Canceled;
                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existing.WorkingHourServiceId);
                existingReservation.Status = ReservationStatusEnum.Canceled;
                _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();

                var message = new Message(new string[] { existingReservation.Email }, "Shtyrje rezervimi", "I nderuar klient ju njoftojme se per shkak te mosofrimit te sherbimit ne ambjentet tona ,jemi te detyruar " +
                 "t'iu anullojme rezervimin");
                await _emailSender.SendEmailAsync(message);
                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Mos ofrimi i sherbimit",
                    ReservationId = existing.ReservationId,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();
            }
        }

        private async Task PostponeReservation(int[] serviceIds, int userId)
        {
            foreach (var serviceId in serviceIds)
            {
                var existingReservationWithService = await _dapperRepository.GetExistingReservationsWithService(serviceId);
                foreach (var existing in existingReservationWithService)
                {
                    var existingReservation = await _repositoryManager.ReservationRepository.GetRecordByIdAsync(existing.ReservationId);
                    existingReservation.Status = ReservationStatusEnum.Postponed;

                    _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                    await _repositoryManager.SaveAsync();

                    var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existing.WorkingHourServiceId);
                    existingWorkingHourService.Status = ReservationStatusEnum.Postponed;
                    existingWorkingHourService.StartTime = null;
                    existingWorkingHourService.EndTime = null;

                    _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
                    await _repositoryManager.SaveAsync();

                    var message = new Message(new string[] { existingReservation.Email }, "Shtyrje rezervimi", "I nderuar klient ju njoftojme se per shkak te mosofrimit te sherbimit per nje afat te pacaktuar, rezervimi juaj eshte " +
             "anulluar dhe do te njoftoheni ne nje moment te dyte per nje orar te ri. Ju faleminderit");
                    await _emailSender.SendEmailAsync(message);

                    var reportsDTO = new ReportsDTO
                    {
                        StatusMessage = "Mos ofrimi i sherbimit perkohesisht",
                        ReservationId = existing.ReservationId,
                        CreatedBy = userId,
                        DateCreated = DateTime.UtcNow
                    };
                    var report = _mapper.Map<Reports>(reportsDTO);
                    _repositoryManager.ReportsRepository.CreateRecord(report);
                    await _repositoryManager.SaveAsync();
                }
            }
        }

        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<ServiceColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }


        private async Task UpdateServiceDevice(List<ServiceDevice> existingServiceDevice, ServiceDTO updateServiceDTO, int serviceId, int userId)
        {
            try
            {

                if (existingServiceDevice != null && existingServiceDevice.Count() > 0 && updateServiceDTO.DeviceIds is not null)
                {
                    var ids = existingServiceDevice.Select(x => x.DeviceId).ToList();
                    var check = ids.Except(updateServiceDTO.DeviceIds);

                    if (check.Count() != 0)
                    {
                        await DeleteServiceStaffRelationForServiceId(serviceId);
                        CreateServiceDeviceRelation(updateServiceDTO.DeviceIds, serviceId, userId);
                    }
                }
                else
                {
                    if (updateServiceDTO.DeviceIds is not null)
                        CreateServiceDeviceRelation(updateServiceDTO.DeviceIds, serviceId, userId);
                }

                await _repositoryManager.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateServiceDevice), ex.Message));
                throw new BadRequestException(ex.Message);
            }


        }


        private async Task UpdateServiceStaff(List<ServiceStaff> existingServiceStaff, ServiceDTO updateServiceDTO, int serviceId, int userId)
        {
            try
            {

                if (existingServiceStaff != null && existingServiceStaff.Count() > 0 && updateServiceDTO.StaffIds is not null)
                {
                    var ids = existingServiceStaff.Select(x => x.StaffId).ToList();
                    var check = ids.Except(updateServiceDTO.StaffIds);

                    if (check.Count() != 0)
                    {
                        await DeleteServiceStaffRelationForServiceId(serviceId);
                        CreateServiceStaffRelation(updateServiceDTO.StaffIds, serviceId, userId);
                    }
                }
                else
                {
                    if (updateServiceDTO.DeviceIds is not null)
                        CreateServiceStaffRelation(updateServiceDTO.StaffIds, serviceId, userId);
                }

                await _repositoryManager.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateServiceStaff), ex.Message));
                throw new BadRequestException(ex.Message);
            }



        }
        #endregion
    }
}
