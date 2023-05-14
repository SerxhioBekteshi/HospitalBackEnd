using Shared.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class ReservationDTO
    {
        public int? Id { get;set;}
        public int? WorkingHourServiceId { get; set; }
        //public DateTime? ReservationStartTime { get; set; }
        //public DateTime? ReservationEndTime { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? ClientId { get; set; }
        public string? PhoneNumber { get; set; }
        public ReservationStatusEnum? Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public DateTime? DateModified { get; set; }
        public int? ModifiedBy { get; set; }
        public string? ModifiedByFullName { get; set; }
    }
}
