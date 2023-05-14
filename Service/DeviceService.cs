using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Shared.DTC;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DeviceService : IDeviceService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;
        private readonly IEmailSender _emailSender;

        public DeviceService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository, IEmailSender emailSender)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
            _emailSender = emailSender;
        }

        public async Task<IEnumerable<DeviceDTO>> GetAllDevicesAsync(LookupRepositoryDTO filter)
        {
            try
            {
                var devices = await _dapperRepository.GetAllDevices(filter);
                return devices;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllDevicesAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<PagedListResponse<IEnumerable<DeviceDTO>>> GetAllDevicesTable(LookupRepositoryDTO filter)
        {
            try
            {
                var emailTemplateWithMetaData = await _dapperRepository.SearchDevices(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<DeviceDTO>> response = new PagedListResponse<IEnumerable<DeviceDTO>>
                {
                    TotalCount = emailTemplateWithMetaData.MetaData.TotalCount,
                    CurrentPage = emailTemplateWithMetaData.MetaData.CurrentPage,
                    PageSize = emailTemplateWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = emailTemplateWithMetaData,
                    HasNext = emailTemplateWithMetaData.MetaData.HasNext,
                    HasPrevious = emailTemplateWithMetaData.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllDevicesTable), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> CreateRecord(DeviceDTO createDeviceDto, int userId)
        {
            try
            {
                var device = _mapper.Map<Device>(createDeviceDto);

                device.DateCreated = DateTime.UtcNow;
                device.CreatedBy = userId;
                device.IsActive = true;

                _repositoryManager.DeviceRepository.CreateRecord(device);
                await _repositoryManager.SaveAsync();
                if (createDeviceDto.ServiceIds != null)
                {
                    await CreateServiceDeviceRelation(createDeviceDto.ServiceIds, device.Id, userId);
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
        public async Task<DeviceDTO> GetRecordById(int id)
        {
            try
            {
                var existingDevice = await GetDeviceAndCheckIfExistsAsync(id);
                var deviceDto = _mapper.Map<DeviceDTO>(existingDevice);
                var servicesForDevice = await _dapperRepository.GetServicesForDeviceId((int)deviceDto.Id);
                var serviceIds = await _repositoryManager.DeviceServiceRepository.GetServiceIdsForDeviceId(id);
                deviceDto.ServiceIdsOptions = servicesForDevice;
                deviceDto.ServiceIds = (List<int?>?)serviceIds;
    
                return deviceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<bool> UpdateRecord(DeviceDTO deviceDto, int id, int userId)
        {
            try
            {
                var existingDevice = await GetDeviceAndCheckIfExistsAsync(id);
                var existingServiceDevice = existingDevice.DeviceService;
                //if (!existingDevice.IsActive)
                //    throw new BadRequestException($"Pajisja: {existingDevice.Name} nuk është aktive!");

                _mapper.Map(deviceDto, existingDevice);

                existingDevice.DateModified = DateTime.UtcNow;
                existingDevice.ModifiedBy = userId;
                if (deviceDto.ServiceIds.Count != 0)
                {
                    await UpdateServiceDevice(existingServiceDevice, deviceDto, id, userId);
                }

                _repositoryManager.DeviceRepository.UpdateRecord(existingDevice);
                await _repositoryManager.SaveAsync();

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
                var existingDevice = await GetDeviceAndCheckIfExistsAsync(id);

                if (existingDevice != null)
                {
                    await DeleteServiceDeviceRelationForServiceId(id);
                    await CancelReservationAndUpdateStatusByDevice(id, userId);
                    _repositoryManager.DeviceRepository.DeleteRecord(existingDevice);
                }
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> PostServicesToDevice(int deviceId, PostServicesToDeviceDTO postServicesToDevice, int managerId)
        {
            try
            {
                var existingDevice = await GetDeviceAndCheckIfExistsAsync(deviceId);

                if (existingDevice is null)
                    throw new NotFoundException(string.Format("Pika fizike me id: {0} nuk u gjet!", deviceId));

                _mapper.Map(postServicesToDevice, existingDevice);

                var servicesForDeviceId = await _dapperRepository.GetServicesForDeviceId(deviceId);

                var servicesToAdd = postServicesToDevice?.ServiceIds?.Where(x => !servicesForDeviceId.Equals(x)).ToList();
                await CreateServiceDeviceRelation(servicesToAdd, deviceId, managerId);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(PostServicesToDevice), ex.Message));
                throw new BadRequestException(ex.Message);
            }

        }

        public async Task<bool> DeactivateDeviceAsync(int[] deviceIds, int userId)
        {
            try
            {
                await _dapperRepository.DeactivateDevices(deviceIds);
                await PostponeReservation(deviceIds, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeactivateDeviceAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> ActivateDeviceAsync(int[] deviceIds)
        {
            try
            {
                await _dapperRepository.ActivateDevices(deviceIds);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(ActivateDeviceAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<IEnumerable<ServiceListOptionsDTO>> GetAllServices(AutocompleteDTO autocomplete)
        {
            try
            {
                var services = await _dapperRepository.GetAllServices(autocomplete);
                return services;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllServices), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        //private List<DataTableColumn> GetDataTableColumns()
        //{
        //    // get the columns
        //    var columns = GenerateDataTableColumn<DeviceColumn>.GetDataTableColumns();

        //    // return all columns
        //    return columns;
        //}

        #region PrivateMethods
        private async Task<Device> GetDeviceAndCheckIfExistsAsync(int id)
        {
            var existingDevice = await _repositoryManager.DeviceRepository.GetRecordByIdAsync(id);
            if (existingDevice is null)
                throw new NotFoundException(string.Format($"Pajisja me Id: {id} nuk u gjet!"));

            return existingDevice;
        }
        private async Task CreateServiceDeviceRelation(List<int?>? serviceIds, int deviceId, int userId)
        {
            if (serviceIds is not null)
            {
                foreach (var serviceId in serviceIds)
                {
                    //DUHET BERE CHECK NQS EKZISTON KJO DEVICE ID DHE NQS ESHTE AKTIVE
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

        private async Task DeleteServiceDeviceRelationForServiceId(int deviceId)
        {
            var existingServiceDevice = await _repositoryManager.DeviceServiceRepository.GetDeviceServiceForDeviceIdAsync(deviceId);
            foreach (var existingService in existingServiceDevice)
            {
                _repositoryManager.DeviceServiceRepository.DeleteRecord(existingService);
            }
 
        }

        private async Task CancelReservationAndUpdateStatusByDevice(int deviceId, int userId)
        {
            var existingReservationsWithDevice = await _dapperRepository.GetExistingReservationsWithDevice(deviceId);
            foreach (var existing in existingReservationsWithDevice)
            {
                var existingReservation = await _repositoryManager.ReservationRepository.GetRecordByIdAsync(existing.ReservationId);
                existingReservation.Status = ReservationStatusEnum.Canceled;
                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

              
                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existing.WorkingHourServiceId);
                existingReservation.Status = ReservationStatusEnum.Canceled;
                _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();


                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Mos ofrimi i pajisjes",
                    ReservationId = existing.ReservationId,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };


                var message = new Message(new string[] { existingReservation.Email }, "Shtyrje rezervimi", "I nderuar klient ju njoftojme se per shkak te mosofrimit te pajisjes per nje afat te pacaktuar, rezervimi juaj eshte " +
                    "anulluar dhe do te njoftoheni ne nje moment te dyte per nje orar te ri. Ju faleminderit");
                await _emailSender.SendEmailAsync(message);

                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();
            }
        }

        private async Task PostponeReservation(int[] devicesIds, int userId)
        {
            foreach(var deviceId in devicesIds)
            {

                var existingReservationsWithDevice = await _dapperRepository.GetExistingReservationsWithDevice(deviceId);
                foreach (var existing in existingReservationsWithDevice)
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

                    var message = new Message(new string[] { existingReservation.Email }, "Shtyrje rezervimi", "I nderuar klient ju njoftojme se per shkak te mosfunksionimit te pajisjes per nje afat te pacaktuar, rezervimi juaj eshte " +
                    "anulluar dhe do te njoftoheni ne nje moment te dyte per nje orar te ri. Ju faleminderit");
                    await _emailSender.SendEmailAsync(message);

                    var reportsDTO = new ReportsDTO
                    {
                        StatusMessage = "Prishja e pajisjes perkohesisht",
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
            var columns = GenerateDataTableColumn<DeviceColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }


        private async Task UpdateServiceDevice(List<ServiceDevice> existingServiceDevice, DeviceDTO updateDeviceDTO, int deviceId, int userId)
        {
            try
            {

                if (existingServiceDevice != null && existingServiceDevice.Count() > 0 && updateDeviceDTO.ServiceIds is not null)
                {
                    var ids = existingServiceDevice.Select(x => x.ServiceId).ToList();
                    var check = ids.Except(updateDeviceDTO.ServiceIds);

                    if (check.Count() != 0)
                    {
                        await DeleteServiceDeviceRelationForServiceId(deviceId);
                        CreateServiceDeviceRelation(updateDeviceDTO.ServiceIds, deviceId, userId);
                    }
                }
                else
                {
                    if (updateDeviceDTO.ServiceIds is not null)
                        CreateServiceDeviceRelation(updateDeviceDTO.ServiceIds, deviceId, userId);
                }

                await _repositoryManager.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateServiceDevice), ex.Message));
                throw new BadRequestException(ex.Message);
            }


        }


        #endregion


    }
}
