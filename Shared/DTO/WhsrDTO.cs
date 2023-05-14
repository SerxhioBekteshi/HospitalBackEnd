using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class WhsrDTO
    {
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int WorkingHourServiceId { get; set; }
        public int ReservationId { get; set; }

    }
}
