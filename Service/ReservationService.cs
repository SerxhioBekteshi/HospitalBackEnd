using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Org.BouncyCastle.Asn1.Ocsp;
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
    public class ReservationService : IReservationService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IDapperRepository _dapperRepository;
        private readonly IEmailSender _emailSender;

        public ReservationService(ILoggerManager logger, IMapper mapper, IRepositoryManager repositoryManager, IDapperRepository dapperRepository, IEmailSender emailSender)
        {
            _logger = logger;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _dapperRepository = dapperRepository;
            _emailSender = emailSender;
        }

        public async Task<int> CreateRecord(ReservationDTO createReservationDTO, int userId)
        {
            try
            {
               
                var existingWorkingHourService = await GetWorkingHourServiceAndCheckIfExistsAsync((int)createReservationDTO.WorkingHourServiceId);

                var reservation = _mapper.Map<Reservation>(createReservationDTO);
                reservation.DateCreated = DateTime.UtcNow;
                reservation.CreatedBy = userId;


                _repositoryManager.ReservationRepository.CreateRecord(reservation);
                await _repositoryManager.SaveAsync();

                return reservation.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(CreateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllReservationTable(LookupRepositoryDTO filter)
        {
            try
            {
                var emailTemplateWithMetaData = await _dapperRepository.SearchReservations(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ReservationTableDTO>> response = new PagedListResponse<IEnumerable<ReservationTableDTO>>
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
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllReservationTable), ex.Message));
                throw new BadRequestException(ex.Message);
            }


        }

        public async Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllReservationTableForStaff(LookupRepositoryDTO filter, int staffId)
        {
            try
            {
                var emailTemplateWithMetaData = await _dapperRepository.SearchReservationsForStaff(filter, staffId);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ReservationTableDTO>> response = new PagedListResponse<IEnumerable<ReservationTableDTO>>
                {
                    TotalCount = emailTemplateWithMetaData.MetaData.TotalCount,
                    CurrentPage = emailTemplateWithMetaData.MetaData.CurrentPage,
                    PageSize = emailTemplateWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = emailTemplateWithMetaData,
                    HasNext = emailTemplateWithMetaData.MetaData.HasNext,
                    HasPrevious = emailTemplateWithMetaData.MetaData.HasPrevious,
                    TotalPages = emailTemplateWithMetaData.MetaData.TotalPages
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllReservationTableForStaff), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllPostponedReservations(LookupRepositoryDTO filter)
        {
            try
            {
                var reservationsWithMetaData = await _dapperRepository.SearchPostponedReservations(filter);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ReservationTableDTO>> response = new PagedListResponse<IEnumerable<ReservationTableDTO>>
                {
                    TotalCount = reservationsWithMetaData.MetaData.TotalCount,
                    CurrentPage = reservationsWithMetaData.MetaData.CurrentPage,
                    PageSize = reservationsWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = reservationsWithMetaData,
                    HasNext = reservationsWithMetaData.MetaData.HasNext,
                    HasPrevious = reservationsWithMetaData.MetaData.HasPrevious,
                    TotalPages = reservationsWithMetaData.MetaData.TotalPages
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllPostponedReservations), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        
        public async Task<PagedListResponse<IEnumerable<ReservationTableDTO>>> GetAllSuccededReservations(LookupRepositoryDTO filter, int staffId)
        {
            try
            {
                var reservationsWithMetaData = await _dapperRepository.SearchSuccededReservations(filter, staffId);
                var columns = GetDataTableColumns();

                PagedListResponse<IEnumerable<ReservationTableDTO>> response = new PagedListResponse<IEnumerable<ReservationTableDTO>>
                {
                    TotalCount = reservationsWithMetaData.MetaData.TotalCount,
                    CurrentPage = reservationsWithMetaData.MetaData.CurrentPage,
                    PageSize = reservationsWithMetaData.MetaData.PageSize,
                    Columns = columns,
                    Rows = reservationsWithMetaData,
                    HasNext = reservationsWithMetaData.MetaData.HasNext,
                    HasPrevious = reservationsWithMetaData.MetaData.HasPrevious,
                    TotalPages = reservationsWithMetaData.MetaData.TotalPages
                };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetAllSuccededReservations), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<bool> SubmitResult(int id, SubmitResultDTO result)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);
                existingReservation.Result = result.Result;

                var message = new Message(new string[] { result.Email }, "Rezultati i sherbimit", $"Pergjigja e analizave tuaja ne lidhje me sherbimin e kryer " +
                    $"eshte {result.Result} ");
                await _emailSender.SendEmailAsync(message);

                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(SubmitResult), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        public async Task<bool> DeleteRecord(int id)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);

                _repositoryManager.ReservationRepository.DeleteRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(DeleteRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> UpdateRecord(ReservationDTO updateReservationDto, int id, int userId, string StatusMessage)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);

                _mapper.Map(updateReservationDto, existingReservation);

                existingReservation.DateModified = DateTime.UtcNow;
                existingReservation.ModifiedBy = userId;


                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation); 
                //pasi eshte updatuar dmth ka ndryshuar statusi 
                //do e ruajme ndryshimin ne db dhe ky ndryshim do pasqyrohet tek tabela e statistika dhe gjithashtu 
                //workingHourService do i ndryshoj statusi gjithashtu 
                await _repositoryManager.SaveAsync();
                var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                if (existingReport is not null)
                {
                    _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                }

                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = StatusMessage,
                    ReservationId = id,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<bool> CancelReservation(int id, int userId)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);

                existingReservation.DateModified = DateTime.UtcNow;
                existingReservation.ModifiedBy = userId;
                existingReservation.Status = ReservationStatusEnum.Canceled;

                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existingReservation.WorkingHourServiceId);
                existingWorkingHourService.DateModified = DateTime.UtcNow;
                existingReservation.ModifiedBy = userId;
                existingReservation.Status = ReservationStatusEnum.Canceled;

                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation); 
                await _repositoryManager.SaveAsync();

                var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                if (existingReport is not null)
                {
                    _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                }
                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Anullim nga ana e klientit",
                    ReservationId = id,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();



                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(UpdateRecord), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<bool> SucceedReservation(int id, int userId)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);

                existingReservation.DateModified = DateTime.UtcNow;
                existingReservation.ModifiedBy = userId;
                existingReservation.Status = Shared.Types.ReservationStatusEnum.Succeded;

                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existingReservation.WorkingHourServiceId);
                existingWorkingHourService.DateModified = DateTime.UtcNow;
                existingReservation.ModifiedBy = userId;
                existingReservation.Status = ReservationStatusEnum.Succeded;


                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                if(existingReport is not null)
                {
                    _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                }

                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Rezervim i suksesshem",
                    ReservationId = id,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(SucceedReservation), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<bool> RegisterStartTimeReservation(int id, int userId)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);
                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existingReservation.WorkingHourServiceId);
                DateTime timeNow = DateTime.UtcNow;
                timeNow = timeNow.AddHours(1);
                TimeSpan ts = (TimeSpan)(timeNow - existingWorkingHourService.StartTime);
                var tt = ts.TotalMinutes;
                var statusMessage = "";
                if (tt >= 20 && tt <= 60)
                {
                    var delay = new Delay
                    {
                        UserId = userId
                    };
                    statusMessage = "Rezervim nisur me vonese";
                    _repositoryManager.DelayRepository.CreateRecord(delay);
                    await _repositoryManager.SaveAsync();
                }
                else
                {
                    if (tt > 60)
                    {
                        var delay = new Delay
                        {
                            UserId = userId
                        };
                        statusMessage = "Mos paraqitje ne rezervim";

                        _repositoryManager.DelayRepository.CreateRecord(delay);
                        await _repositoryManager.SaveAsync();
                    }
                    _repositoryManager.ReservationRepository.DeleteRecord(existingReservation);
                    var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                    if (existingReport is not null)
                    {
                        _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                    }

                    var reportsDTO = new ReportsDTO
                    {
                        StatusMessage = statusMessage,
                        ReservationId = id,
                        CreatedBy = userId,
                        DateCreated = DateTime.UtcNow
                    };
                    var report = _mapper.Map<Reports>(reportsDTO);
                    _repositoryManager.ReportsRepository.CreateRecord(report);
                    await _repositoryManager.SaveAsync();
                }
                    return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(RegisterStartTimeReservation), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

       

        public async Task<Reservation> GetReservationById(int id)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);
                //var deviceById = _mapper.Map<Device>(existingDevice);

                return existingReservation;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetReservationById), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<ReservationTableDTO> GetReservationDetailsById(int id)
        {
            try
            {
                var reservationDetails = await _dapperRepository.GetReservationDetails(id);
                //var deviceById = _mapper.Map<Device>(existingDevice);

                return reservationDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetReservationDetailsById), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }


        public async Task<bool> PostponeReservation(int id, postponeReservationDTO postponeReservation, int userId)
        {
            try
            {
                var existingReservation= await GetReservationAndCheckIfExistsAsync(id);
                var oldWorkingHourServiceId = existingReservation.WorkingHourServiceId;
                existingReservation.WorkingHourServiceId = postponeReservation.WorkingHourServiceId;
                existingReservation.Status = Shared.Types.ReservationStatusEnum.Postponed;

                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(oldWorkingHourServiceId);
                existingWorkingHourService.Status = ReservationStatusEnum.Pending;
                _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();
                //KETU MUND TE BEHET NJOFTIMI ME EMAIL PER DISA KLIENT PER TE NJEJTIN SHERBIM QE E KANE ORARE ME VONE
                var exWhs = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(postponeReservation.WorkingHourServiceId);
                var message = new Message(new string[] { postponeReservation.Email }, "Shtyrje rezervimi", $"Orari juaj i ri per rezervimin e kryer eshte {exWhs.StartTime} me {exWhs.EndTime} ");
                await _emailSender.SendEmailAsync(message);

                var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                if (existingReport is not null)
                {
                    _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                }

                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Rezervim i shtyre",
                    ReservationId = id,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();

              

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(PostponeReservation), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        
        public async Task<bool> PostponeReservationOfNoTime(int id, postponeReservationDTO postponeReservation, int userId)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);
                var oldWorkingHourServiceId = existingReservation.WorkingHourServiceId;
                existingReservation.WorkingHourServiceId = postponeReservation.WorkingHourServiceId;
                existingReservation.Status = ReservationStatusEnum.Pending;

                _repositoryManager.ReservationRepository.UpdateRecord(existingReservation);
                await _repositoryManager.SaveAsync();

                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(oldWorkingHourServiceId);
                _repositoryManager.WorkingHourServiceRepository.DeleteRecord(existingWorkingHourService);
                await _repositoryManager.SaveAsync();

                var exWhs = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(postponeReservation.WorkingHourServiceId);
                var message = new Message(new string[] { postponeReservation.Email }, "Shtyrje rezervimi", $"Orari juaj i ri per rezervimin e kryer eshte {exWhs.StartTime} me {exWhs.EndTime} ");
                await _emailSender.SendEmailAsync(message);

                var existingReport = await _repositoryManager.ReportsRepository.GetRecordByReservationIdAsync(existingReservation.Id);
                if (existingReport is not null)
                {
                    _repositoryManager.ReportsRepository.DeleteRecord(existingReport);
                }

                var reportsDTO = new ReportsDTO
                {
                    StatusMessage = "Rezervim i shtyre",
                    ReservationId = id,
                    CreatedBy = userId,
                    DateCreated = DateTime.UtcNow
                };
                var report = _mapper.Map<Reports>(reportsDTO);
                _repositoryManager.ReportsRepository.CreateRecord(report);
                await _repositoryManager.SaveAsync();



                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(PostponeReservationOfNoTime), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task<IEnumerable<WorkingHourServiceTableDTO>> AvailableTimesForReservation(int id)
        {
            try
            {
                var existingReservation = await GetReservationAndCheckIfExistsAsync(id);
                var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existingReservation.WorkingHourServiceId);
                var getTimesForReservation = await _dapperRepository.GetAllTimesAvailableForReservationServiceId((int)existingWorkingHourService.ServiceId);


                return getTimesForReservation;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(AvailableTimesForReservation), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        
        public async Task<int> GetPeopleServedNumber()
        {
            try
            {
                var peopleServed = await _dapperRepository.GetPeopleServedNumber();
              

                return peopleServed;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}: {1}", nameof(GetPeopleServedNumber), ex.Message));
                throw new BadRequestException(ex.Message);
            }
        }
        #region Private Methods

        private async Task<Reservation> GetReservationAndCheckIfExistsAsync(int id)
        {
            var existingReservation = await _repositoryManager.ReservationRepository.GetRecordByIdAsync(id);
            if (existingReservation is null)
                throw new NotFoundException(string.Format("Nuk u gjet!"));

            return existingReservation;
        }

        private async Task<WorkingHourService> GetWorkingHourServiceAndCheckIfExistsAsync(int id)
        {
            var existing = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(id);
            if (existing is null)
                throw new NotFoundException(string.Format("Nuk u gjet!"));

            return existing;
        }

        private List<DataTableColumn> GetDataTableColumns()
        {
            // get the columns
            var columns = GenerateDataTableColumn<ReservationTableColumn>.GetDataTableColumns();

            // return all columns
            return columns;
        }

        #endregion

    }
}
