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

        public List<ShoppingCartItem> GetShoppingCartItems(int shoppingCartId)
        {
            var shoppingCartItems = new List<ShoppingCartItem>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
            SELECT
                p.ProductNumber,
                p.ProductName,
                p.ProductDescription,
                p.Price,
                p.IsSold,
                p.EAN13Number,
                p.RackNumber
            FROM ShoppingCartItem sci
            INNER JOIN Products p
                ON sci.ProductNumber = p.ProductNumber
            WHERE sci.ShoppingCartId = @ShoppingCartId",
                    conn);

                cmd.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    shoppingCartItems.Add(new ShoppingCartItem
                    {
                        ShoppingCartId = shoppingCartId,
                        ProductNumber = (int)reader["ProductNumber"],

                        Product = new Product
                        {
                            ProductNumber = (int)reader["ProductNumber"],
                            ProductName = reader["ProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Price = (decimal)reader["Price"],
                            IsSold = (bool)reader["IsSold"],
                            EAN13Number = reader["EAN13Number"].ToString(),
                            RackNumber = (int)reader["RackNumber"]
                        }
                    });
                }
            }

            return shoppingCartItems;
        }

        public void AddProductToCart(int shoppingCartId, Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    @"INSERT INTO ShoppingCartItem
              (ShoppingCartId, ProductNumber)
              VALUES
              (@ShoppingCartId, @ProductNumber)",
                    conn);

                cmd.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);

                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveProductFromCart(
        int shoppingCartId,
        int productNumber)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    @"DELETE FROM ShoppingCartItem
              WHERE ShoppingCartId = @ShoppingCartId
              AND ProductNumber = @ProductNumber",
                    conn);

                cmd.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);
                cmd.Parameters.AddWithValue("@ProductNumber", productNumber);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
