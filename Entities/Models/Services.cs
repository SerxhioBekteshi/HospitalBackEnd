using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Services :BaseCreatedAndModified
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<ServiceStaff>? ServiceStaff { get; set; }
        public List<ServiceDevice>? DeviceService { get; set; }

    }
}
