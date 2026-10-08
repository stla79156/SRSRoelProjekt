
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRSRoelProjekt.Core.Repositories;
namespace SRSRoelProjekt.Test
{


[TestClass]
    public class EmployeeRepositoryTests
    {
        private IEmployeeRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new SQLEmployeeRepository();
        }

        [TestMethod]
        public void GetEmployees_ReturnsEmployees()
        {
            var employees = _repository.GetEmployees();

            Assert.IsNotNull(employees);
            Assert.IsTrue(employees.Count > 0);
        }

        [TestMethod]
        public void GetEmployeeByUsername_ReturnsEmployee()
        {
            string username = "010203";

            var employee =
                _repository.GetEmployeeByUsername(username);

            Assert.IsNotNull(employee);
            Assert.AreEqual(username,
                employee.EmployeeUserName);
        }
    }
}
