using Shared.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Reservation : BaseCreatedAndModified
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ClientId { get; set; }
        public string PhoneNumber { get; set; }
        public ReservationStatusEnum Status { get; set; }
        public int WorkingHourServiceId { get; set; }
        [ForeignKey("WorkingHourServiceId")]
        public WorkingHourService? WorkingHourService { get; set; }
        public string? Result { get; set; }
        //public DateTime ReservationStartTime { get; set; }
        //public DateTime ReservationEndTime { get; set; }

    }
}