using Microsoft.Data.SqlClient;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly string _connectionString =
                "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ProductNumber, ProductName, ProductDescription, Price, EAN13Number FROM Products", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductNumber = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        ProductDescription = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        EAN13Number = reader.GetString(4),
                        RackNumber = reader.GetInt32(5),
                    });

                }
            }
            return products;
        }
        public void AddProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "INSERT INTO Products (ProductName, ProductDescription, Price, EAN13Number, RackNumber) VALUES (@ProductName, @ProductDescription, @Price, @EAN13Number, @RackNumber)",
                    conn);


                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@EAN13Number", product.EAN13Number);
                cmd.Parameters.AddWithValue("@RackNumber", product.RackNumber);

                cmd.ExecuteNonQuery();
            }
        }

        public void RemoveProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("DELETE FROM Products WHERE ProductNumber = @ProductNumber", conn);
                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "UPDATE Products SET ProductName=@ProductName, ProductDescription=@ProductDescription, Price=@Price, EAN13Number=@EAN13Number, RackNumber=@RackNumber WHERE ProductNumber=@ProductNumber",
                    conn);

                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@EAN13Number", product.EAN13Number);
                cmd.Parameters.AddWithValue("@RackNumber", product.RackNumber);

                cmd.ExecuteNonQuery();
            }
        }

        public Product? GetProductByProductNumber(string productNumber)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    @"SELECT ProductNumber, ProductName, ProductDescription,
                     Price, EAN13Number, RackNumber
              FROM Products
              WHERE ProductNumber = @ProductNumber",
                    conn);

                cmd.Parameters.AddWithValue("@ProductNumber", productNumber);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Product
                    {
                        ProductNumber = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        ProductDescription = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        EAN13Number = reader.GetString(4),
                        RackNumber = reader.GetInt32(5)
                    };
                }
                return null;
            }
        }
    }
}
