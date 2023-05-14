using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? TokenHash { get; set; }
    public DateTime? DateCreated { get; set; }
    //public ApplicationUser? Manager { get; set; }
    public List<ApplicationUser>? Workers { get; set; }
    public List<ServiceStaff>? ServiceStaff { get; set; }
    public List<WorkingHourService>? WorkHours { get; set; }
    public List<Delay>? Delays { get; set; }

}