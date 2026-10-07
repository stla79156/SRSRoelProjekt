using Microsoft.Data.SqlClient;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SqlMonthlyPostingRepository
    : IMonthlyPostingRepository
    {
        private readonly string _connectionString =
            "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

        public void CreatePosting(int monthNumber, int yearNumber, DateTime postedDate, int employeeId)
        {
            using var conn =
                new SqlConnection(_connectionString);

            conn.Open();

            var cmd = new SqlCommand(
            @"INSERT INTO MonthlyPosting
        (
            MonthNumber,
            YearNumber,
            PostedDate,
            EmployeeId
        )
        VALUES
        (
            @MonthNumber,
            @YearNumber,
            @PostedDate,
            @EmployeeId
        )",
            conn);

            cmd.Parameters.AddWithValue(
                "@MonthNumber",
                monthNumber);

            cmd.Parameters.AddWithValue(
                "@YearNumber",
                yearNumber);

            cmd.Parameters.AddWithValue(
                "@PostedDate",
                postedDate);

            cmd.Parameters.AddWithValue(
                "@EmployeeId",
                employeeId);

            cmd.ExecuteNonQuery();
        }

        public MonthlyPosting? GetPosting(
            int monthNumber,
            int yearNumber)
        {
            using var conn =
                new SqlConnection(_connectionString);

            conn.Open();

            var cmd = new SqlCommand(
            @"SELECT mp.MonthlyPostingId,
                mp.MonthNumber,
                mp.YearNumber,
                mp.PostedDate,
                mp.EmployeeId,
                e.EmployeeName
                FROM MonthlyPosting mp
                INNER JOIN Employees e
                ON mp.EmployeeId = e.EmployeeId
                WHERE mp.MonthNumber = @MonthNumber
                AND mp.YearNumber = @YearNumber",
            conn);

            cmd.Parameters.AddWithValue(
                "@MonthNumber",
                monthNumber);

            cmd.Parameters.AddWithValue(
                "@YearNumber",
                yearNumber);

            var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new MonthlyPosting
                {
                    MonthlyPostingId = reader.GetInt32(0),
                    MonthNumber = reader.GetInt32(1),
                    YearNumber = reader.GetInt32(2),
                    PostedDate = reader.GetDateTime(3),
                    EmployeeId = reader.GetInt32(4)
                };
            }

            return null;
        }
    }
}
