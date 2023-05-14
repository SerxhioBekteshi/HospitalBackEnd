namespace Shared.DTO;

public class UserUpdateDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? Birthday { get; set; }

    public List<int?>? ServiceIds { get; set; }
}