using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    namespace SRSRoelProjekt.Core.Repositories
    {
        public class SqlRenterRepository : IRenterRepository
        {
            private readonly string _connectionString =
                "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

            public List<Renter> GetRenters()
            {
                var renters = new List<Renter>();

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("SELECT RenterId, Name, Email, PhoneNumber FROM Renters", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        renters.Add(new Renter
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3)
                        });
                    }
                }

                return renters;
            }

            public void AddRenter(Renter renter)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(
                        "INSERT INTO Renters (Name, Email, PhoneNumber) VALUES (@Name, @Email, @Phone)",
                        conn);

                    cmd.Parameters.AddWithValue("@Name", renter.Name);
                    cmd.Parameters.AddWithValue("@Email", renter.Email);
                    cmd.Parameters.AddWithValue("@Phone", renter.PhoneNumber);

                    cmd.ExecuteNonQuery();
                }
            }

            public void RemoveRenter(int id)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("DELETE FROM Renters WHERE RenterId = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                }
            }

            public void UpdateRenter(Renter renter)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(
                        "UPDATE Renters SET Name=@Name, Email=@Email, PhoneNumber=@Phone WHERE Id=@Id",
                        conn);

                    cmd.Parameters.AddWithValue("@Id", renter.Id);
                    cmd.Parameters.AddWithValue("@Name", renter.Name);
                    cmd.Parameters.AddWithValue("@Email", renter.Email);
                    cmd.Parameters.AddWithValue("@Phone", renter.PhoneNumber);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}