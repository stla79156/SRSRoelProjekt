using System;
using SRSRoelProjekt.Core.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SQLShoppingCartRepository : IShoppingCartRepository
    {
        private readonly string _connectionString =
                "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Product> GetShoppingCartItems()
        {
            var products = new List<Product>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT ProductNumber FROM ShoppingCart", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int productNumber = reader.GetInt32(0);
                    // You can use the productNumber to fetch product details if needed
                }
            }
            return products;
        }

        public void AddProductToCart(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO ShoppingCart (ProductNumber) VALUES (@ProductNumber)",
                    conn);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveProductFromCart(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "DELETE FROM ShoppingCart WHERE ProductNumber = @ProductNumber",
                    conn);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
