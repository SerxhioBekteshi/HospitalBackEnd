using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO
{
    public class DelayCountDTO
    {
        public int? Id { get; set; }
        public int? staffId { get; set; }
        public string? staffName { get; set; }
        public int? Count { get; set; } 
    }
}
