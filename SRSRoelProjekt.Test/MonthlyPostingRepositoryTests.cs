
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Repositories;

namespace SRSReolProjekt.Test
{
    [TestClass]
    public class MonthlyPostingRepositoryTests
    {
        private IMonthlyPostingRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository =
                new SqlMonthlyPostingRepository();
        }

        [TestMethod]
        public void GetPosting_ReturnsPostingOrNull()
        {
            var posting =
                _repository.GetPosting(
                    DateTime.Now.Month,
                    DateTime.Now.Year);

            Assert.IsTrue(
                posting == null ||
                posting.MonthNumber == DateTime.Now.Month);
        }
    }
}