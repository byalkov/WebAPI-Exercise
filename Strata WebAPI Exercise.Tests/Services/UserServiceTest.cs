using Strata_WebAPI_Exercise.Services;
using NUnit.Framework;
using Moq;
using Strata_WebAPI_Exercise.Entities;
using System.Collections.Generic;
using Strata_WebAPI_Exercise.Interfaces;

namespace Strata_WebAPI_Exercise.Tests
{
    [TestFixture]
    public class UserServiceTest
    {
        private UserService userService { get; set; }
        private Customer mockCustomer { get; set; }

        [SetUp]
        public void SetUp()
        {
            mockCustomer = new Customer()
            {
                CustomerId = 1,
                Email = "yb@gmail.com",
                Password = "pass",
                Name = "YB",
                AccountBalance = 100,
                Address = "",
                LoyaltyId = 1
            };
            var mockDataStoreService = new Mock<IRepositoryService>();
            mockDataStoreService.Setup(x => x.GetCustomerRepository()).Returns(new List<Customer>() { mockCustomer });

            userService = new UserService(mockDataStoreService.Object);
        }

        [Test]
        public void ValidateExistingUserExists()
        {
            var res = userService.ValidateUser(mockCustomer.Email, mockCustomer.Password);
            Assert.AreEqual(mockCustomer.CustomerId, res.CustomerId);
        }

        [Test]
        public void ValidateNewUserDoesntExist()
        {
            var newCustomer = new Customer()
            {
                CustomerId = 2,
                Email = "yb2@gmail.com",
                Password = "pass",
                Name = "YB2",
                AccountBalance = 100,
                Address = "",
                LoyaltyId = 1
            };

            var res = userService.ValidateUser(newCustomer.Email, newCustomer.Password);
            Assert.That(res, Is.Null);
        }
    }
}
