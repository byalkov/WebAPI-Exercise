using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using Strata_WebAPI_Exercise.Services;

namespace Strata_WebAPI_Exercise.Tests.Services
{
    [TestFixture]
    public class OrderServiceTest
    {
        private OrderService orderService { get; set; }
        private Customer mockCustomer { get; set; }
        private ShoppingCart mockShoppingCart { get; set; }
        private List<Product> mockProducts { get; set; }
        private List<Order> mockOrders { get; set; }

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

            mockOrders = new List<Order>() {
                new Order(){    OrderId = 1, CustomerId = 1, Customer=mockCustomer, DeliveryAddress="", Status = Status.Dispatched,
                    OrderLineItems = new List<LineItem>()
                    {
                        new LineItem() { ProductId= 1, Product=mockProducts[0], Quantity=1 },
                        new LineItem() { ProductId= 2, Product=mockProducts[1], Quantity=2 }
                    }
                },
                new Order(){    OrderId = 2, CustomerId = 1, Customer=mockCustomer, DeliveryAddress="", Status = Status.AwaitingDispatch,
                    OrderLineItems = new List<LineItem>()
                    {
                        new LineItem() { ProductId= 1, Product=mockProducts[0], Quantity=1 },
                        new LineItem() { ProductId= 2, Product=mockProducts[1], Quantity=1 }
                    }
                }
            };

            var mockDataStoreService = new Mock<IRepositoryService>();
            mockDataStoreService.Setup(x => x.GetCustomerRepository()).Returns(new List<Customer>() { mockCustomer });
            mockDataStoreService.Setup(x => x.UpdateShoppingCartRepository(It.IsAny<ShoppingCart>()));
            mockDataStoreService.Setup(x => x.AddOrder(It.IsAny<Order>()));
            mockDataStoreService.Setup(x => x.GetOrdersRepository()).Returns(mockOrders);
            mockDataStoreService.Setup(x => x.SaveMessage(It.IsAny<Message>()));

            var customerService = new Mock<ICustomerService>();
            customerService.Setup(x => x.GetCustomer(1)).Returns(mockCustomer);
            customerService.Setup(x => x.UpdateBalance(It.IsAny<int>(), It.IsAny<Order>()));

            orderService = new OrderService(mockDataStoreService.Object, customerService.Object);
        }

        [Test]
        public void CreateOrderSuccessfully()
        {
            var res = orderService.CreateOrder(mockCustomer.CustomerId, mockShoppingCart);
            //Validate all properties of the two objects
            Assert.AreEqual(mockShoppingCart.Items.Count, res.OrderLineItems.Count);
            Assert.AreEqual(mockShoppingCart.CustomerId, res.CustomerId);
            Assert.AreEqual(Status.AwaitingDispatch, res.Status);
        }

        [Test]
        public void GetOrderByIdExists()
        {
            var res = orderService.GetOrder(mockCustomer.CustomerId, mockOrders[0].OrderId);

            //Validate all properties of the two objects
            Assert.AreEqual(mockOrders[0].OrderId, res.OrderId);
            Assert.AreEqual(mockOrders[0].OrderLineItems.Count, res.OrderLineItems.Count);
        }

        [Test]
        public void GetOrderByIdExistsInvalidCustomer()
        {
            var res = orderService.GetOrder(999, mockOrders[0].OrderId);
           
            Assert.IsNull(res);
        }

        [Test]
        public void GetOrdersByStatus()
        {
            var awaitingDispatchOrders = 1;

            var res = orderService.GetOrders(mockCustomer.CustomerId, Status.AwaitingDispatch);

            Assert.AreEqual(awaitingDispatchOrders, res.Count);
        }

        [Test]
        public void GetMessage()
        {
            Assert.DoesNotThrow(delegate
            {
                orderService.SendOrderMessage(mockOrders[0]);
            });
        }
    }

}
