using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ReservationTableDTO
    {
        public int id { get; set; }
        public string clientId { get; set; }
        public string clientName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public ReservationStatusEnum status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string serviceName { get; set; }
        public int? serviceId { get; set; }
        public int? workingServiceHourId { get; set; }

    }
}
