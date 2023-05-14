using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Reports : BaseCreatedAndModified
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation? Reservation  { get; set; }
        public string StatusMessage { get; set; } //pershkrimi nqs eshte anulluar shtyre dhe bere rezervimi

    }
}
