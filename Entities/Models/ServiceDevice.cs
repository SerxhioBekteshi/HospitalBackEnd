using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ServiceDevice : BaseCreatedAndModified
    {
        public int Id { get; set; }
        public int? DeviceId { get; set; }
        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }
        public int? ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Services? Service { get; set; }
        public int Counter { get; set; }
    }
}
