using Entities.Models;

namespace Repository.Contracts;

public interface IUserRepository
{
    void CreateRecord(ApplicationUser user);
    void UpdateRecord(ApplicationUser user);
    void DeleteRecord(ApplicationUser user);
    Task<ApplicationUser> GetRecordByIdAsync(int id);
}