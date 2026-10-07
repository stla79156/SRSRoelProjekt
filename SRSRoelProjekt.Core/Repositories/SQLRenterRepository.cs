using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

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

                    var cmd = new SqlCommand("SELECT RenterId, Name, Email, PhoneNumber, Username FROM Renters", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        renters.Add(new Renter
                        {
                            RenterId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Username = reader.GetString(4)
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
                        "INSERT INTO Renters (Name, Email, PhoneNumber, Username) VALUES (@Name, @Email, @Phone, @Username)",
                        conn);

                    cmd.Parameters.AddWithValue("@Name", renter.Name);
                    cmd.Parameters.AddWithValue("@Email", renter.Email);
                    cmd.Parameters.AddWithValue("@Phone", renter.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Username", renter.Username);

                cmd.ExecuteNonQuery();
                }
            }

            public void RemoveRenter(Renter renter)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("DELETE FROM Renters WHERE RenterId = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", renter.RenterId);

                    cmd.ExecuteNonQuery();
                }
            }

        //Ville blive brugt hvis vi implimenterede et window til lejerne, hvor de kunne opdatere deres informationer.
        public void UpdateRenter(Renter renter)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(
                        "UPDATE Renters SET Name=@Name, Email=@Email, PhoneNumber=@Phone, Username=@Username WHERE RenterId=@RenterId",
                        conn);

                    cmd.Parameters.AddWithValue("@RenterId", renter.RenterId);
                    cmd.Parameters.AddWithValue("@Name", renter.Name);
                    cmd.Parameters.AddWithValue("@Email", renter.Email);
                    cmd.Parameters.AddWithValue("@Phone", renter.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Username", renter.Username);

                    cmd.ExecuteNonQuery();
                }
            }


            public Renter GetRenterByUsername(string username)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(
                    "SELECT RenterId, Name, Email, PhoneNumber, Username " +
                    "FROM Renters " +
                    "WHERE Username = @Username",
                    conn);

                    cmd.Parameters.AddWithValue("@Username", username);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Renter
                        {
                            RenterId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            PhoneNumber = reader.GetString(3),
                            Username = reader.GetString(4)
                        };
                    }
                }

                return null;
            }

    }
}