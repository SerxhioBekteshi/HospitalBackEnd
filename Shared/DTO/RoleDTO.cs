using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO;

public class RoleDTO
{
    public int? RoleId { get; set; }
    public string? Name { get; set; }
    public bool? CheckStatus { get; set; } 
    public int? RolePermissionId { get; set; }
}
