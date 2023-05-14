using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTO;

public class ApplicationMenuDTO 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Route { get; set; }
    public int Order { get; set; }
    public int RoleId { get; set; }
    public int ParentId { get; set; }

}