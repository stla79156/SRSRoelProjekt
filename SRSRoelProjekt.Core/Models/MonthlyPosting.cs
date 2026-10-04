using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class MonthlyPosting
    {
        public int MonthlyPostingId { get; set; }

        public int MonthNumber { get; set; }

        public int YearNumber { get; set; }

        public DateTime PostedDate { get; set; }

        public string EmployeeName { get; set; }
    }
}
