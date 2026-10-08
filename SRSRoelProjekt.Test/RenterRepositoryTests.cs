
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Repositories;
namespace SRSRoelProjekt.Test
{
   
        [TestClass]
        public class RenterRepositoryTests
        {
            private IRenterRepository _repository;

            [TestInitialize]
            public void Setup()
            {
                _repository = new SqlRenterRepository();
            }

            [TestMethod]
            public void GetRenters_ReturnsList()
            {
                // Act
                var renters = _repository.GetRenters();

                // Assert
                Assert.IsNotNull(renters);
                Assert.IsTrue(renters.Count > 0);
            }

            [TestMethod]
            public void GetRenterByUsername_ExistingUser_ReturnsRenter()
            {
                // Arrange
                string username = "rm1234"; // eksisterende testdata

                // Act
                var renter = _repository.GetRenterByUsername(username);

                // Assert
                Assert.IsNotNull(renter);
                Assert.AreEqual(username, renter.Username);
            }
        }
    
}
