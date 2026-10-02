using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Models
{
    public class Employee
    {

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string EmployeeUserName { get; set; }

        public override string ToString()
        {
            if (EmployeeId == 4 || EmployeeId == 5)
            {
                return $"{EmployeeName} - {EmployeeUserName} (Admin)";
            }

            return $"{EmployeeName} - {EmployeeUserName}";
        }

    }
}
