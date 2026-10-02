using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Rack
    {
        public int RackNumber { get; set; }
        public RackStatus RackStatus { get; set; }
        public int? RenterId { get; set; }
        public bool WithHanger { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AvailableFrom { get; set; }

        public string DisplayText => $"Reol {RackNumber}";
    }
}
