
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Repositories;

namespace SRSReolProjekt.Test
{
    

    [TestClass]
    public class ProductRepositoryTests
    {
        
        private IProductRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new SQLProductRepository();
        }

        [TestMethod]
        public void GetSoldProducts_ReturnsList()
        {
            var products =
                _repository.GetSoldProducts(DateTime.Now.Month);

            Assert.IsNotNull(products);
        }

        [TestMethod]
        public void GetProductsByRack_ReturnsList()
        {
            var products =
                _repository.GetProductsByRack(1);

            Assert.IsNotNull(products);
        }
    }
}
