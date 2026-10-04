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

                var cmd = new SqlCommand("SELECT EmployeeId, EmployeeName, EmployeeUserName, IsAdmin FROM Employees", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        EmployeeName = reader.GetString(1),
                        EmployeeUserName = reader.GetString(2),
                        IsAdmin = reader.GetBoolean(3)
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
                    "INSERT INTO Employees (EmployeeName, EmployeeUserName, IsAdmin) VALUES (@EmployeeName, @EmployeeUserName, @IsAdmin)",
                    conn);
             
                cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                cmd.Parameters.AddWithValue("@EmployeeUserName", employee.EmployeeUserName);
                cmd.Parameters.AddWithValue("@IsAdmin", employee.IsAdmin);

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
                    "UPDATE Employees SET EmployeeName=@EmployeeName, EmployeeUserName=@EmployeeUserName WHERE EmployeeId=@EmployeeId",
                    conn);

                cmd.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                cmd.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                cmd.Parameters.AddWithValue("@EmployeeUserName", employee.EmployeeUserName);
              

                cmd.ExecuteNonQuery();
            }
        }

        public Employee GetEmployeeByUsername(string username)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                "SELECT EmployeeId, EmployeeName, EmployeeUserName, IsAdmin " +
                "FROM Employees " +
                "WHERE EmployeeUserName = @Username",
                conn); // her har jeg ændret parameteren til @Username for at matche SQL-forespørgslen til kun at hente en enkelt medarbejder baseret på brugernavnet

                cmd.Parameters.AddWithValue("@Username", username);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Employee
                    {
                        EmployeeId = reader.GetInt32(0),
                        EmployeeName = reader.GetString(1),
                        EmployeeUserName = reader.GetString(2),
                        IsAdmin = reader.GetBoolean(3)
                    };
                }
            }

            return null;
        }



    }
}       

               



        

        



        



    

