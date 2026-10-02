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






        public List<Product> GetProductsByRack(int rackNumber)
        {
            var products = new List<Product>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand(
                    @"SELECT ProductNumber,
                     ProductName,
                     ProductDescription,
                     Price,
                     IsSold,
                     EAN13Number,
                     RackNumber
              FROM Products
              WHERE RackNumber = @RackNumber",
                    conn);

                cmd.Parameters.AddWithValue("@RackNumber", rackNumber);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductNumber = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        ProductDescription = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        IsSold = reader.GetBoolean(4),
                        EAN13Number = reader.GetString(5),
                        RackNumber = reader.GetInt32(6)
                    });
                }
            }

            return products;
        }

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var cmd = new SqlCommand("SELECT ProductNumber, ProductName, ProductDescription, Price, IsSold, EAN13Number, RackNumber FROM Products", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductNumber = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        ProductDescription = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                        IsSold = reader.GetBoolean(4),
                        EAN13Number = reader.GetString(5),
                        RackNumber = reader.GetInt32(6),
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

                var insertCmd = new SqlCommand(
                @"INSERT INTO Products
                (ProductName, ProductDescription, Price, IsSold, RackNumber)
                OUTPUT INSERTED.ProductNumber
                VALUES
                (@ProductName, @ProductDescription, @Price, @IsSold, @RackNumber)",
                conn);

                insertCmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                insertCmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                insertCmd.Parameters.AddWithValue("@Price", product.Price);
                insertCmd.Parameters.AddWithValue("@IsSold", product.IsSold = false);
                insertCmd.Parameters.AddWithValue("@RackNumber", product.RackNumber);

                int generatedProductNumber =
                    (int)insertCmd.ExecuteScalar();

                product.ProductNumber = generatedProductNumber;

                product.GenerateEan13();

                var updateCmd = new SqlCommand(
                @"UPDATE Products
          SET EAN13Number = @EAN13Number
          WHERE ProductNumber = @ProductNumber",
                conn);

                updateCmd.Parameters.AddWithValue("@EAN13Number", product.EAN13Number);
                updateCmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);

                updateCmd.ExecuteNonQuery();
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
                    "UPDATE Products SET ProductName=@ProductName, ProductDescription=@ProductDescription, Price=@Price, IsSold=@IsSold, EAN13Number=@EAN13Number, RackNumber=@RackNumber WHERE ProductNumber=@ProductNumber",
                    conn);

                cmd.Parameters.AddWithValue("@ProductNumber", product.ProductNumber);
                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@ProductDescription", product.ProductDescription);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@IsSold", product.IsSold);
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
                    Price, IsSold, EAN13Number, RackNumber
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
                        IsSold = reader.GetBoolean(4),
                        EAN13Number = reader.GetString(5),
                        RackNumber = reader.GetInt32(6)
                    };
                }
                return null;
            }
        }
    }
}
