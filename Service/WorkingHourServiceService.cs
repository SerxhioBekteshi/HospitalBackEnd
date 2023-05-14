using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.Exceptions;
using Entities.Models;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTC;
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
    public class WorkingHourServiceService : IWorkingHourServiceService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;

        public WorkingHourServiceService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
        }


        public async Task<bool> CreateRecord(WorkingHourServiceDTO createWorkingHourServiceDTO, int userId)
        {
            try
            {
              
                var workingHourService = _mapper.Map<WorkingHourService>(createWorkingHourServiceDTO);
                var getAllTimes = await _dapperRepository.GetTimesForServiceAndStaff((int)createWorkingHourServiceDTO.ServiceId, (userId != null) ? userId : (int)createWorkingHourServiceDTO.workerId);


                var error = false;
                var getCounterDeviceForService = await _repositoryManager.DeviceServiceRepository.GetCounterDeviceByServiceIdAsync((int)createWorkingHourServiceDTO.ServiceId);
                if(getCounterDeviceForService == 0)
                {
                    //foreach (var times in getAllTimes)
                    //{
                    //    if ((createWorkingHourServiceDTO.StartTime >= times.StartTime && createWorkingHourServiceDTO.StartTime <= times.EndTime) ||
                    //        (createWorkingHourServiceDTO.EndTime >= times.StartTime && createWorkingHourServiceDTO.StartTime <= times.EndTime))
                    //    {
                    //        error = true;
                    //        break;
                    //    }
                    //}
                    error = true;
                }
                else if(getCounterDeviceForService > 0)
                {
                    var deviceService = await _repositoryManager.DeviceServiceRepository.GetDeviceServiceForServiceIdAsync((int)createWorkingHourServiceDTO.ServiceId);
                    foreach(var ds in deviceService)
                    {
                        ds.Counter = ds.Counter - 1;
                        _repositoryManager.DeviceServiceRepository.UpdateRecord(ds);
                        await _repositoryManager.SaveAsync();
                    }

                }
             

                if(error == false)
                {
                    workingHourService.DateCreated = DateTime.UtcNow;
                    workingHourService.CreatedBy = userId;
                    workingHourService.StaffId = userId;
                    workingHourService.Status = 0;

                    _repositoryManager.WorkingHourServiceRepository.CreateRecord(workingHourService);
                    await _repositoryManager.SaveAsync();


                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> DeleteRecord(int id) //DO FSHIHET VETEM NQS SESHTE I REZERVUAR NE
                                                 //TE KUNDERT DO ANULLOHET REZERVIMI PERKATES BASHKE ME KETE 
        {
            try
            {
                var existingWorkingHourService = await GetWorkingHourServiceAndCheckIfExistsAsync(id);

                _repositoryManager.WorkingHourServiceRepository.DeleteRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> UpdateRecord(WorkingHourServiceDTO updateWorkingHourServiceDTO, int id, int userId)
        {
            try
            {
                var existingWorkingHourService = await GetWorkingHourServiceAndCheckIfExistsAsync(id);

                _mapper.Map(updateWorkingHourServiceDTO, existingWorkingHourService);

                existingWorkingHourService.DateModified = DateTime.UtcNow;
                existingWorkingHourService.ModifiedBy = userId;
                   //KETU DO BEHET NJE CHECK NE RAST SE NDRYSHOHET DOKTORI OSE STAFI PERKATES 
                  
                _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<WorkingHourServiceDTO> GetRecordById(int id)
        {
            try
            {
                var existingWorkingHourService = await GetWorkingHourServiceAndCheckIfExistsAsync(id);
                var workingHourServiceDto = _mapper.Map<WorkingHourServiceDTO>(existingWorkingHourService);

                return workingHourServiceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>> GetAllWorkingHoursServicesAsync(LookupRepositoryDTO filter)
        {
            try
            {
                var availableTimes = await _dapperRepository.SearchAvailableTimes(filter);
                var times = availableTimes.ToList();

                foreach (var time in times)
                {
                    var reservationWithThisTime = await _repositoryManager.ReservationRepository.GetRecordByWorkingIdAsync((int)time.Id);
                    if (reservationWithThisTime is not null)
                    {
                        availableTimes.Remove(time);

                    }
                    else continue;
                }

          
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>> response = new PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>
                {
                    TotalCount = availableTimes.MetaData.TotalCount,
                    CurrentPage = availableTimes.MetaData.CurrentPage,
                    PageSize = availableTimes.MetaData.PageSize,
                    Columns = columns,
                    Rows = availableTimes,
                    HasNext = availableTimes.MetaData.HasNext,
                    HasPrevious = availableTimes.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllWorkingHoursServicesAsync), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>> GetAllTimeServicesTableForStaff(LookupRepositoryDTO filter, int staffId)
        {
            try
            {
                var availableTimes = await _dapperRepository.SearchAllServiceTimesForStaff(filter, staffId);
                var columns = GetDataTableColumns();
                var times = availableTimes.ToList();

                foreach (var time in times)
                {
                    var reservationWithThisTime = await _repositoryManager.ReservationRepository.GetRecordByWorkingIdAsync((int)time.Id);
                    if (reservationWithThisTime is not null)
                    {
                        availableTimes.Remove(time);

                    }
                    else continue;
                }


                PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>> response = new PagedListResponse<IEnumerable<WorkingHourServiceTableDTO>>
                {
                    TotalCount = availableTimes.MetaData.TotalCount,
                    CurrentPage = availableTimes.MetaData.CurrentPage,
                    PageSize = availableTimes.MetaData.PageSize,
                    Columns = columns,
                    Rows = availableTimes,
                    HasNext = availableTimes.MetaData.HasNext,
                    HasPrevious = availableTimes.MetaData.HasPrevious
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllTimeServicesTableForStaff), ex.Message));
                throw new BadRequestException(ex.Message);
            }


        }
        public async Task<IEnumerable<WorkingHourServiceTableDTO>> GetTimesForServiceId(int id)
        {
            try
            {
                var existingTimes = await _dapperRepository.GetAllTimesAvailableForServiceId(id);
                return existingTimes;

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetTimesForServiceId), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }



        #region Private Methods

        private async Task<WorkingHourService> GetWorkingHourServiceAndCheckIfExistsAsync(int id)
        {
            var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(id);
            if (existingWorkingHourService is null)
                throw new NotFoundException(string.Format("Nuk u gjet!"));

            return existingWorkingHourService;
        }

        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<WorkingHourServiceColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }

        #endregion
    }
}
public enum WorkingHourServiceStatusEnum
{
    Pending,
    Succeded,
    Postponed,
    Canceled,
}
