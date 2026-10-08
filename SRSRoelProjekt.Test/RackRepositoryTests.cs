
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Repositories;

namespace SRSReolProjekt.Test
{
  
    [TestClass]
    public class RackRepositoryTests
    {
        private IRackRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new SQLRackRepository();
        }

        [TestMethod]
        public void GetRacks_ReturnsRacks()
        {
            var racks = _repository.GetRacks();

            Assert.IsNotNull(racks);
            Assert.IsTrue(racks.Count > 0);
        }
    }
}
