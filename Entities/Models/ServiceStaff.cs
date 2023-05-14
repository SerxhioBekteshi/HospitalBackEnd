using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ServiceStaff : BaseCreatedAndModified
    {
        public int Id { get; set; }

        public int? StaffId { get; set; }
        [ForeignKey("StaffId")]
        public ApplicationUser? Staff { get; set; }
        public int? ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Services? Service { get; set; }
    }
}
