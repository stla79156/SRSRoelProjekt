using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Rack
    {
        public int RackNumber { get; set; }
        public RackStatus Status { get; set; }
        public string RenterName { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AvailableFrom { get; set; }
    }
}
