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
            if (shoppingCartId == null)
            {
                shoppingCartId = CreateShoppingCart();
            }

            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = @"
        INSERT INTO ShoppingCartItem
        (
            ShoppingCartId,
            ProductNumber
        )
        VALUES
        (
            @ShoppingCartId,
            @ProductNumber
        )";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);
            cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public int CreateShoppingCart()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);

            string query = @"
        INSERT INTO ShoppingCart
        DEFAULT VALUES;
        
        SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);

            conn.Open();

            return Convert.ToInt32(cmd.ExecuteScalar());
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

        public void ClearShoppingCart(int shoppingCartId)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
        DELETE FROM ShoppingCartItem
        WHERE ShoppingCartId = @ShoppingCartId";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ShoppingCartId", shoppingCartId);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
