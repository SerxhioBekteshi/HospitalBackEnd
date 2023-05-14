namespace Shared.DTO;

public class UserListDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; }
    public string TokenHash { get; set; }

    public List<int?>? ServiceIds { get; set; }
}