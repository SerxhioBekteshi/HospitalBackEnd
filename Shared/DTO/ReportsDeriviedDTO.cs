using Shared.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ReportsDeriviedDTO
    {
        public int ReportId { get; set; }
        public int? ReservationId { get; set; }
        public int? WorkingHourServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string clientId { get; set; }
        public string name { get; set; }
        public string surname { get; set; } 
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public ReservationStatusEnum status { get; set; }
        public string statusMessage { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public DateTime? DateModified { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
