using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class WorkingHourServiceTableDTO
    {
        public int? Id { get; set; }
        public ReservationStatusEnum? Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? serviceName { get; set; }

    }
}
