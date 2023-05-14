using Microsoft.AspNetCore.Identity;
using Shared.DTO;
using Shared.RequestFeatures;
using Shared.ResponseFeatures;

namespace Service.Contracts;

public interface IUserService
{
    Task<IdentityResult> AddUser(AddUserDTO addUserDto, int userId);
    Task<UserListDTO> GetRecordById(int userId);
    Task<UserDetailsDTO> GetUserById(int userId);
    Task<bool> UpdateRecord(UserUpdateDTO userUpdateDto, int userId);
    Task<IEnumerable<UserTableDTO>> GetAllRecords(LookupRepositoryDTO filter);
    Task<bool> PostServicesToStaff(int staffId, PostServicesToStaffDTO postServicesToStaff, int managerId);
    Task<bool> DeleteRecord(int id);

    //Task<IEnumerable<UserTableDTO>> GetAllWorkersAsync();
    Task<PagedListResponse<IEnumerable<UserTableDTO>>> GetAllUsers(LookupRepositoryDTO filter);
    Task<IEnumerable<UserTableDTO>> GetWorkers();

}