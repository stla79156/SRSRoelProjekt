using Microsoft.Data.SqlClient;
using SRSRoelProjekt.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSRoelProjekt.Core.Repositories
{
    public class SQLPaymentRepository : IPaymentRepository
    {
        private readonly string _connectionString =
               "Server=localhost;Database=SRSRoelProjekt;Trusted_Connection=True;TrustServerCertificate=True;";

        public void CreatePayment(Payment payment)
        {
            using var conn = new SqlConnection(_connectionString);

            string query = @"
        INSERT INTO Payment
        (
            PaymentDate,
            Amount,
            ShoppingCartId,
            PaymentMethodId
        )
        VALUES
        (
            @PaymentDate,
            @Amount,
            @ShoppingCartId,
            @PaymentMethodId
        )";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
            cmd.Parameters.AddWithValue("@Amount", payment.Amount);
            cmd.Parameters.AddWithValue("@ShoppingCartId", payment.ShoppingCartId);
            cmd.Parameters.AddWithValue("@PaymentMethodId", payment.PaymentMethodId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
