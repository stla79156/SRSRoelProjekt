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
        public bool IsAdmin { get; set; }

        public override string ToString()
        {
            if (IsAdmin == true)
            {
                return $"{EmployeeName} - {EmployeeUserName} (Admin)";
            }

            return $"{EmployeeName} - {EmployeeUserName}";
        }

    }
}
