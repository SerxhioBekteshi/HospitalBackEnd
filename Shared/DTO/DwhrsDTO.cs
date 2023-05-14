using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class DwhrsDTO
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int WorkingHourServiceId { get; set; }
        public int ReservationId { get; set; }
    }
}
