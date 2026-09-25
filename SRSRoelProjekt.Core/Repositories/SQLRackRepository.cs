using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using SRSRoelProjekt.Core.Models;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SQLRackRepository : IRackRepository
    {
        private readonly string _connectionString =
              "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Rack> GetRacks()
        {
            var racks = new List<Rack>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT RackNumber, WithHanger, EndDate, AvailableFrom, RenterId, RackStatusId FROM Racks", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    racks.Add(new Rack
                    {
                        RackNumber = (int)reader["RackNumber"],
                        WithHanger = (bool)reader["WithHanger"],
                        EndDate = reader["EndDate"] == DBNull.Value ? null : (DateTime?)reader["EndDate"],
                        AvailableFrom = reader["AvailableFrom"] == DBNull.Value ? null : (DateTime?)reader["AvailableFrom"],
                        RenterId = reader["RenterId"] == DBNull.Value ? null : (int?)reader["RenterId"],
                        RackStatus = (RackStatus)(int)reader["RackStatusId"],
                    });
                }
            }
            return racks;
        }

        public void StartRental(int rackNumber, int renterId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                UPDATE Racks 
                SET RenterId = @RenterId, 
                    RackStatusId = @RackStatusId 
                WHERE RackNumber = @RackNumber", conn);

                cmd.Parameters.AddWithValue("@RenterId", renterId);
                cmd.Parameters.AddWithValue("@RackStatusId", (int)RackStatus.Reserved);
                cmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                cmd.ExecuteNonQuery();
            }
        }

        public void StopRental(int rackNumber)
        {
            //Opsagt senest d. 20. i måneden, så er den ledig fra 1. i næste måned
            DateTime endDate = DateTime.Today;
            DateTime availableFrom;

            if (endDate.Day <= 20) 
            { 
                availableFrom = new DateTime(endDate.Year, endDate.Month,1).AddMonths(1);
            }

            else 
            { 
                availableFrom = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(2);
            }


            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
                UPDATE Racks 
                SET EndDate = @EndDate,
                    AvailableFrom = @AvailableFrom,
                    RackStatusId = @RackStatusId
                WHERE RackNumber = @RackNumber", conn);
                
                cmd.Parameters.AddWithValue("@EndDate", endDate);
                cmd.Parameters.AddWithValue("@AvailableFrom", availableFrom);
                cmd.Parameters.AddWithValue("@RackStatusId", (int)RackStatus.EndingSoon);
                cmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
