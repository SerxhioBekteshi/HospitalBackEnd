namespace Shared.DTO;

public class UserTableDTO
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public int? RoleId { get; set; }
    public string? Role { get; set; }
    public DateTime? DateCreated { get; set; }

}