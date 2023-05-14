using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class WorkingHourServiceDTO
    {
        public int? Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public DateTime? DateModified { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByFullName { get; set; }

        public int? workerId { get; set; }
    }
}
