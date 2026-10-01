using Microsoft.Data.SqlClient;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString =
               "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";






        public List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT EmployeeId, EmployeeName, EmployyeeUserName FROM Employees", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        EmployeeName = reader.GetString(1),
                        EmployyeeUserName = reader.GetString(2),
                        
                    });

                }
            }
            return employees;
        }












        public void AddEmployee(Employee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "INSERT INTO Employees (EmployeeName, EmployyeeUserName) VALUES (@EmployeeName, @EmployyeeUserName)",
                    conn);

             
                cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                cmd.Parameters.AddWithValue("@EmployyeeUserName", employee.EmployyeeUserName);

                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveEmployee(Employee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("DELETE FROM Employees WHERE EmployeeId = @EmployeeId", conn);
                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "UPDATE Employees SET EmployeeName=@EmployeeName, EmployyeeUserName=@EmployyeeUserName WHERE EmployeeId=@EmployeeId",
                    conn);

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                cmd.Parameters.AddWithValue("@EmployyeeUserName", employee.EmployyeeUserName);
              

                cmd.ExecuteNonQuery();
            }
        }

        



    }
}       

               



        

        



        



    

