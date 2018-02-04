using Moq;
using NUnit.Framework;
using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using Strata_WebAPI_Exercise.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Strata_WebAPI_Exercise.Tests.Services
{
    [TestFixture]
    public class CustomerServiceTest
    {

        private CustomerService customerService { get; set; }
        private Customer mockCustomer { get; set; }

        [SetUp]
        public void SetUp()
        {
            var loyaltyList = new List<Loyalty>()
            {
                new Loyalty(){ LoyaltyId =1, Name="Standard", DicountPercentage = null, CategoryThreshold=0, ExtraCredit=0},
                new Loyalty(){ LoyaltyId =2, Name="Silver", DicountPercentage = 0.02, CategoryThreshold=500, ExtraCredit=0},
                new Loyalty(){ LoyaltyId =3, Name="Gold", DicountPercentage = 0.05, CategoryThreshold=1500, ExtraCredit=1000}
            };

            mockCustomer = new Customer()
            {
                CustomerId = 1,
                Email = "yb@gmail.com",
                Password = "pass",
                Name = "YB",
                AccountBalance = 1000,
                Address = "",
                LoyaltyId = 1,
                Loyalty = loyaltyList[0]
            };

            var mockDataStoreService = new Mock<IRepositoryService>();
            mockDataStoreService.Setup(x => x.GetCustomerRepository()).Returns(new List<Customer>() { mockCustomer });
            mockDataStoreService.Setup(x => x.GetCustomer(mockCustomer.CustomerId)).Returns(mockCustomer);
            mockDataStoreService.Setup(x => x.GetCustomer(It.IsNotIn(mockCustomer.CustomerId))).Throws<KeyNotFoundException>();
            mockDataStoreService.Setup(x => x.UpdateCustomer(It.IsAny<Customer>()));
            customerService = new CustomerService(mockDataStoreService.Object);
        }

        [Test]
        public void GetClaimsUserIdSuccessful()
        {
            var sid = 1;
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid, sid.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var res = customerService.GetClaimsUserId(claimsPrincipal);

            Assert.AreEqual(sid, res);
        }


        [Test]
        public void GetClaimsUserIdUnSuccessful()
        {
            var invalidSid = "0a1b";
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Sid, invalidSid)
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var res = customerService.GetClaimsUserId(claimsPrincipal);

            Assert.IsNull(res);
        }

        [Test]
        public void GetClaimsUserIdMissingSidClaim()
        {
            var claims = new List<Claim>();
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var res = customerService.GetClaimsUserId(claimsPrincipal);

            Assert.IsNull(res);
        }

        [Test]
        public void GetCustomerExisting()
        {
            var res = customerService.GetCustomer(mockCustomer.CustomerId);
            Assert.AreEqual(mockCustomer.CustomerId, res.CustomerId);
        }
        [Test]
        public void GetCustomerNotExisting()
        {

            Assert.Throws<KeyNotFoundException>(
                delegate
                {
                    customerService.GetCustomer(2);
                });
        }

        [Test]
        public void UpdateBalance()
        {
            var products = new List<Product>()
            {
                new Product(){ProductId=1, Name="Product1", IsActive=true, Price=60.10 },
                new Product(){ProductId=2, Name="Product2", IsActive=true, Price=200},
                new Product(){ProductId=3, Name="Product3", IsActive=true, Price=99.99},
            };
            var order = new Order()
            {
                OrderId = 1,
                CustomerId = 1,
                Customer = mockCustomer,
                DeliveryAddress = "",
                Status = Status.Dispatched,
                OrderLineItems = new List<LineItem>()
                    {
                        new LineItem() { ProductId= 1, Product=products[0], Quantity=1 },
                        new LineItem() { ProductId= 2, Product=products[1], Quantity=2 }
                    }
            };

            var oldBalance = mockCustomer.AccountBalance;
            customerService.UpdateBalance(mockCustomer.CustomerId, order);

            Assert.AreEqual(oldBalance - order.TotalCost, mockCustomer.AccountBalance);
        }

    }
}