using Entities.Models;
using Shared.DTO;

namespace Service.Contracts;

public interface IApplicationMenuService
{
    Task<List<ApplicationMenuDTO>> GetMenuByRoleAndUserId(string userRole, int userId);
    Task<ApplicationMenuDTO> GetRecordById(int id);
    Task<IEnumerable<ApplicationMenuDTO>> GetAllMenu();
    Task<IEnumerable<ApplicationMenuDTO>> GetMenu(int roleId);

}