using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using Strata_WebAPI_Exercise.Services;

namespace Strata_WebAPI_Exercise.Tests
{
    [TestFixture]
    public class ShoppingCartServiceTest
    {
        private ShoppingCartService shoppingCartService { get; set; }
        private Customer mockCustomer { get; set; }
        private ShoppingCart mockShoppingCart { get; set; }
        private List<Product> mockProducts { get; set; }

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
                Loyalty =loyaltyList[0]
            };

            mockProducts = new List<Product>()
            {
                new Product(){ProductId=1, Name="Product1", IsActive=true, Price=60.10 },
                new Product(){ProductId=2, Name="Product2", IsActive=true, Price=200},
                new Product(){ProductId=3, Name="Product3", IsActive=true, Price=99.99}
            };

            mockShoppingCart = new ShoppingCart()
            {
                Customer = mockCustomer,
                CustomerId = mockCustomer.CustomerId,
                ShoppingCartId = 1,
                DateCreated = DateTime.Now.AddDays(-1),
                Items = new List<LineItem>() {
                    new LineItem() { ProductId= 1, Product=mockProducts[0], Quantity=1 },
                    new LineItem() { ProductId= 2, Product=mockProducts[1], Quantity=2 }
                }
            };

            var mockDataStoreService = new Mock<IRepositoryService>();
            mockDataStoreService.Setup(x => x.GetCustomerRepository()).Returns(new List<Customer>() { mockCustomer });
            mockDataStoreService.Setup(x => x.GetShoppingCartRepository()).Returns(new List<ShoppingCart>() { mockShoppingCart });
            mockDataStoreService.Setup(x => x.GetShoppingCart(mockShoppingCart.ShoppingCartId)).Returns(mockShoppingCart);
            mockDataStoreService.Setup(x => x.GetProduct(mockProducts[0].ProductId)).Returns(mockProducts[0]);
            mockDataStoreService.Setup(x => x.GetProduct(mockProducts[1].ProductId)).Returns(mockProducts[1]);
            mockDataStoreService.Setup(x => x.GetProduct(mockProducts[2].ProductId)).Returns(mockProducts[2]);
            mockDataStoreService.Setup(x => x.UpdateShoppingCartRepository(It.IsAny<ShoppingCart>()));


            var customerService = new Mock<ICustomerService>();
            customerService.Setup(x => x.GetCustomer(1)).Returns(mockCustomer);
            customerService.Setup(x => x.UpdateBalance(It.IsAny<int>(), It.IsAny<Order>()));

            var orderService = new Mock<IOrderService>();
            orderService.Setup(x => x.SendOrderMessage(It.IsAny<Order>()));

            shoppingCartService = new ShoppingCartService(mockDataStoreService.Object, customerService.Object, orderService.Object);
        }


        [Test]
        public void GetShoppingCartExisting()
        {
            var res = shoppingCartService.GetShoppingCart(mockShoppingCart.ShoppingCartId);

            Assert.That(mockShoppingCart, Is.EqualTo(res));
        }

        [Test]
        public void GetShoppingCartDoesntExisting()
        {
            var res = shoppingCartService.GetShoppingCart(9999);

            Assert.That(res, Is.Null);
        }

        [Test]
        public void UpdateProductWithPossitiveQuantity()
        {
            int quantity = 1;
            var previousQuantity = mockShoppingCart.Items[0].Quantity;
            var res = shoppingCartService.UpdateProduct(mockShoppingCart.ShoppingCartId, mockShoppingCart.Items[0].ProductId, quantity);

            Assert.That(previousQuantity + quantity, Is.EqualTo(res.Items[0].Quantity));
        }

        [Test]
        public void UpdateProductWithPositiveQuantityToNewProduct()
        {
            int quantity = 2;
            var previousCount = mockShoppingCart.Items.Count;
            var res = shoppingCartService.UpdateProduct(mockShoppingCart.ShoppingCartId, mockProducts[2].ProductId, quantity);

            Assert.That(previousCount + 1 == res.Items.Count);
            Assert.That(quantity == res.Items[2].Quantity);
        }


        [Test]
        public void UpdateProductWithNegativeQuantity()
        {
            int quantity = -1;
            var previousQuantity = mockShoppingCart.Items[1].Quantity;
            var res = shoppingCartService.UpdateProduct(mockShoppingCart.ShoppingCartId, mockShoppingCart.Items[1].ProductId, quantity);

            Assert.That(previousQuantity + quantity, Is.EqualTo(res.Items[1].Quantity));
        }

        [Test]
        public void UpdateProductRemoveLineItem()
        {
            int quantity = -1;
            var itemToBeRemoved = mockShoppingCart.Items[0];
            var res = shoppingCartService.UpdateProduct(mockShoppingCart.ShoppingCartId, mockShoppingCart.Items[0].ProductId, quantity);

            Assert.That(res.Items.Count, Is.EqualTo(1));
            Assert.That(!res.Items.Contains(itemToBeRemoved));
        }


        [Test]
        public void UpdateProductWithNegativeQuantityToNewProduct()
        {
            int quantity = -2;
            
            Assert.Throws<InvalidOperationException>(
                delegate
                {
                    shoppingCartService.UpdateProduct(mockShoppingCart.ShoppingCartId, mockProducts[2].ProductId, quantity);
                });
        }

        [Test]
        public void CanUserBuyShoppingCartSufficientBalance()
        {
            var previousBalance = mockCustomer.AccountBalance;
            var shoppingCartTotalPrice = mockShoppingCart.TotalCost;
            var res = shoppingCartService.CanUserBuyShoppingCart(mockShoppingCart.ShoppingCartId, mockShoppingCart.CustomerId);

            Assert.That(previousBalance - shoppingCartTotalPrice > mockCustomer.LoyaltyNegativeBalance);
            Assert.That(res);
        }


        [Test]
        public void CanUserBuyShoppingCartInSufficientBalance()
        {
            mockCustomer.AccountBalance = 0;
            var previousBalance = mockCustomer.AccountBalance;
            var shoppingCartTotalPrice = mockShoppingCart.TotalCost;
            var res = shoppingCartService.CanUserBuyShoppingCart(mockShoppingCart.ShoppingCartId, mockShoppingCart.CustomerId);

            Assert.That(mockCustomer.AccountBalance - shoppingCartTotalPrice < mockCustomer.LoyaltyNegativeBalance);
            Assert.That(!res);
        }

        [Test]
        public void BuyShoppingCartSuccessful()
        {
            var previousShoppingItemsCount = mockShoppingCart.Items.Count;
            var res = shoppingCartService.BuyShoppingCart(mockShoppingCart.ShoppingCartId, mockShoppingCart.CustomerId);

            Assert.AreEqual(0, res.Items.Count);
        }
    }
}
