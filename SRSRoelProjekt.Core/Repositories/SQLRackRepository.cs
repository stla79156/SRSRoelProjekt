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

                var cmd = new SqlCommand("SELECT RackNumber, WithHanger, RenterId, RackStatusId FROM Racks", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    racks.Add(new Rack
                    {
                        RackNumber = (int)reader["RackNumber"],
                        WithHanger = (bool)reader["WithHanger"],
                        RenterId = reader["RenterId"] == DBNull.Value ? null : (int?)reader["RenterId"],
                        RackStatus = (RackStatus)(int)reader["RackStatusId"]
                    });
                }
            }
            return racks;
        }

        public void UpdateRack(Rack rack)
        {
            throw new NotImplementedException();
        }
    }
}
