using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Models;
using SRSRoelProjekt.Core.Repositories;

namespace SRSRoelProjekt.Test
{
    [TestClass]
    public class PaymentRepositoryTests
    {
        private IPaymentRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new SQLPaymentRepository();
        }

        [TestMethod]
        public void CreatePayment_WithValidPayment_DoesNotThrowException()
        {
            // Arrange
            Payment payment = new Payment
            {
                PaymentDate = DateTime.Now,
                Amount = 100.00m,
                ShoppingCartId = 6,
                PaymentMethodId = 1
            };

            // Act & Assert
            try
            {
                _repository.CreatePayment(payment);

                Assert.IsTrue(true);
            }
            catch
            {
                Assert.Fail("CreatePayment threw an exception.");
            }
        }
    }
}
