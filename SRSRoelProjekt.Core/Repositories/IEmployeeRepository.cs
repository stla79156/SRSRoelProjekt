using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> GetEmployees();
   
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void RemoveEmployee(Employee employee);
    }
}

      

   

