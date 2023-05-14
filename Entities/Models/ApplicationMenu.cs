using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Entities.Models;

public class ApplicationMenu
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Route { get; set; }
    public int Order { get; set; }
    [ForeignKey(nameof(Roles))]
    public int RoleId { get; set; }
    public ApplicationRole Roles { get; set; }
    public int ParentId { get; set; }

}