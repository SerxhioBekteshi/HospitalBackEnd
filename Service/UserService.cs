using AutoMapper;
using ClosedXML.Excel;
using EmailService;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;
using Service.Contracts;
using Shared.DTC;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;
using Shared.Utility;

namespace Service;

public class UserService : IUserService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IDapperRepository _dapperRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DefaultConfiguration _defaultConfig;
    private readonly IEmailSender _emailSender;

    public UserService(ILoggerManager logger,
         IMapper mapper,
         IRepositoryManager repositoryManager,
         UserManager<ApplicationUser> userManager,
         IDapperRepository dapperRepository,
         DefaultConfiguration defaultConfig,
         IEmailSender emailSender)
    {
        _logger = logger;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _userManager = userManager;
        _dapperRepository = dapperRepository;
        _defaultConfig = defaultConfig;
        _emailSender = emailSender;
    }

    public async Task<UserListDTO> GetRecordById(int userId)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(userId);

            var userById = _mapper.Map<UserListDTO>(existingUser);
          
 
            return userById;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<UserDetailsDTO> GetUserById(int userId)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(userId);
            var userById = _mapper.Map<UserDetailsDTO>(existingUser);

            var serviceIds = await _repositoryManager.ServiceStaffRepository.GetServiceIdsForStaffId(userId);
            var userDTO = _mapper.Map<UserListDTO>(existingUser);
            userDTO.ServiceIds = (List<int?>?)serviceIds;


            var roles = await _userManager.GetRolesAsync(existingUser);

            foreach (var role in roles)
            {
                userById.Role = role;
            }

            return userById;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<bool> UpdateRecord(UserUpdateDTO userUpdateDto, int userId)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(userId);
            if (userUpdateDto.ServiceIds.Count != 0)
            {
                CreateServiceStaffRelation(userUpdateDto.ServiceIds, existingUser.Id, userId);
            }
            _mapper.Map(userUpdateDto, existingUser);
            _repositoryManager.UserRepository.UpdateRecord(existingUser);

            await _repositoryManager.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetRecordById), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<IEnumerable<UserTableDTO>> GetAllRecords(LookupRepositoryDTO filter)
    {
        try
        {
            var users = await _dapperRepository.GetAllStaff(filter);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<IEnumerable<UserTableDTO>> GetWorkers()
    {
        try
        {
            var users = await _dapperRepository.GetWorkers();
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }



    public async Task<PagedListResponse<IEnumerable<UserTableDTO>>> GetAllUsers(LookupRepositoryDTO filter)
    {
        try
        {
            var userWithMetaData = await _dapperRepository.SearchUsers(filter);
            var columns = GetDataTableColumns();

            PagedListResponse<IEnumerable<UserTableDTO>> response = new PagedListResponse<IEnumerable<UserTableDTO>>
            {
                TotalCount = userWithMetaData.MetaData.TotalCount,
                CurrentPage = userWithMetaData.MetaData.CurrentPage,
                PageSize = userWithMetaData.MetaData.PageSize,
                Columns = columns,
                Rows = userWithMetaData,
            };
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(GetAllRecords), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }
    public async Task<IdentityResult> AddUser(AddUserDTO addUserDto, int userId)
    {
        try
        {
            var user = new ApplicationUser
            {
                FirstName = addUserDto.FirstName,
                LastName = addUserDto.LastName,
                Email = addUserDto.Email,
                PhoneNumber = addUserDto.PhoneNumber,
                UserName = addUserDto.Email,
                DateCreated = DateTime.UtcNow
            };

            string password = $"{addUserDto.FirstName.First().ToString().ToUpper() + addUserDto.FirstName.Substring(1)}{addUserDto.LastName.ToLower()}123@";
            
            var result = await _userManager.CreateAsync(user, password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, addUserDto.Role);

                //if (addUserDto.Role == UserRole.Agent)
                //{
                //    if (addUserDto.LocationId.HasValue)
                //    {
                //        CreateAgentSettingRelation(addUserDto.LocationId, user.Id, userId);
                //    }
                //    CreateManagerAgentRelation(addUserDto.ManagerId, user.Id, userId);
                //}
            }

            await _repositoryManager.SaveAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(AddUser), ex.Message));
            throw new BadRequestException(ex.Message);
        }
    }

    public async Task<bool> DeleteRecord(int id)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(id);
            if (existingUser != null)
            {
                await DeleteServiceStaffRelationForStaffId(id);
                await DeleteCurrentUserFromReservation(id);
                _repositoryManager.UserRepository.DeleteRecord(existingUser);
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


    public async Task<bool> PostServicesToStaff(int staffId, PostServicesToStaffDTO postServicesToStaff, int managerId)
    {
        try
        {
            var existingUser = await GetUserAndCheckIfExistsAsync(staffId);

            if (existingUser is null)
                throw new NotFoundException(string.Format("Stafi me id: {0} nuk u gjet!", staffId));

            _mapper.Map(postServicesToStaff, existingUser);

            var servicesForStaffId = await _dapperRepository.GetServicesForStaffId(staffId);

            var servicesToAdd = postServicesToStaff?.ServiceIds?.Where(x => !servicesForStaffId.Equals(x)).ToList();
            await CreateServiceStaffRelation(servicesToAdd, staffId, managerId);
            await _repositoryManager.SaveAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format("{0}: {1}", nameof(PostServicesToStaff), ex.Message));
            throw new BadRequestException(ex.Message);
        }

    }

    //public async Task<IEnumerable<UserTableDTO>> GetAllWorkersAsync()
    //{
    //    try
    //    {
    //        var workers = await _dapperRepository.GetAllWorkers();
    //        return workers;
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(string.Format("{0}: {1}", nameof(GetAllWorkersAsync), ex.Message));
    //        throw new BadRequestException(ex.Message);
    //    }
    //}


    #region Private Methods
    private async Task<ApplicationUser> GetUserAndCheckIfExistsAsync(int userId)
    {
        var existingUser = await _repositoryManager.UserRepository.GetRecordByIdAsync(userId);
        if (existingUser is null)
            throw new NotFoundException(string.Format("Përdoruesi me Id: {0} nuk u gjet!", userId));

        return existingUser;
    }
    private async Task CreateServiceStaffRelation(List<int?>? serviceIds, int staffId, int userId)
    {
        if (serviceIds is not null)
        {
            foreach (var serviceId in serviceIds)
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

    private async Task DeleteServiceStaffRelationForStaffId(int staffId)
    {
        var existingServiceStaff = await _repositoryManager.ServiceStaffRepository.GetServiceStaffForStaffIdAsync(staffId);
        foreach (var existingstaff in existingServiceStaff)
        {
            _repositoryManager.ServiceStaffRepository.DeleteRecord(existingstaff);
        }

    }

    private List<DataTableColumn> GetDataTableColumns()
    {
        // get the columns
        var columns = GenerateDataTableColumn<UserColumn>.GetDataTableColumns();

        // return all columns
        return columns;
    }

    //private void CreateManagerAgentRelation(int? managerId, int agentId, int userId)
    //{
    //    var newManagerAgent = new ManagerAgent
    //    {
    //        ManagerId = managerId,
    //        AgentId = agentId,
    //        DateCreated = DateTime.UtcNow,
    //        CreatedBy = userId,
    //    };
    //    _repositoryManager.ManagerAgentRepository.CreateRecord(newManagerAgent);
    //}

    private async Task DeleteCurrentUserFromReservation(int userIdToBeDeleted)
    {
        var existingReservationsWithUser = await _dapperRepository.GetExistingReservationsWithUser(userIdToBeDeleted);
        foreach (var existing in existingReservationsWithUser)
        {
            var existingWorkingHourService = await _repositoryManager.WorkingHourServiceRepository.GetRecordByIdAsync(existing.WorkingHourServiceId);
            existingWorkingHourService.StaffId = null; 
            _repositoryManager.WorkingHourServiceRepository.UpdateRecord(existingWorkingHourService);
            await _repositoryManager.SaveAsync();
            //SKA staffId qe do te thoe qe per momentin pret te behet update me nje doktor tjeter ky rezervimi

            //var reportsDTO = new ReportsDTO
            //{
            //    StatusMessage = "Mos ofrimi i pajisjes",
            //    ReservationId = existing.ReservationId,
            //    CreatedBy = userId,
            //    DateCreated = DateTime.UtcNow
            //};
            //var report = _mapper.Map<Reports>(reportsDTO);
            //_repositoryManager.ReportsRepository.CreateRecord(report);
            //await _repositoryManager.SaveAsync();
        }
    }

    #endregion

}

