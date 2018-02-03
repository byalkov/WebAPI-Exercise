using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Strata_WebAPI_Exercise.Services
{
    /// <summary>
    /// Repository service provides CRUD methods for all entities 
    /// (used a basic version of the repository pattern)
    /// </summary>
    public class RepositoryService : IRepositoryService
    {
        private static List<Customer> customers;
        private static List<Order> orders;
        private static List<ShoppingCart> shoppingCarts;
        private static List<Message> messages;
        public RepositoryService()
        {
            customers = new List<Customer>()
            {
                new Customer() { Name = "YB", Email = "yb@gmail.com", CustomerId = 1, Password = "pass", AccountBalance=100.75, Address="", LoyaltyId=1, Loyalty = GetLoyaltyRepository()[0] },
                new Customer() { Name = "YB_Gold", Email = "yb2@gmail.com", CustomerId = 2, Password = "pass", AccountBalance = 5.55, LoyaltyId = 3, Loyalty = GetLoyaltyRepository()[2] },
                new Customer() { Name = "YB_Silver", Email = "yb3@gmail.com", CustomerId = 3, Password = "pass", AccountBalance = 34, LoyaltyId = 2, Loyalty = GetLoyaltyRepository()[1] }
             };

            orders = new List<Order>() {
                new Order(){    OrderId = 1, CustomerId = 1, Customer=GetCustomerRepository()[0], DeliveryAddress="", Status = Status.Dispatched,
                    OrderLineItems = new List<LineItem>()
                    {
                        new LineItem() { ProductId= 1, Product=GetProductRepository()[0], Quantity=1 },
                        new LineItem() { ProductId= 2, Product=GetProductRepository()[1], Quantity=2 }
                    }
                },
                new Order(){    OrderId = 2, CustomerId = 1, Customer=GetCustomerRepository()[0], DeliveryAddress="", Status = Status.AwaitingDispatch,
                    OrderLineItems = new List<LineItem>()
                    {
                        new LineItem() { ProductId= 1, Product=GetProductRepository()[0], Quantity=1 },
                        new LineItem() { ProductId= 2, Product=GetProductRepository()[1], Quantity=1 }
                    }
                }
            };

            shoppingCarts = new List<ShoppingCart>() {
                new ShoppingCart()
                {
                    Customer = GetCustomerRepository()[0], CustomerId=1, ShoppingCartId=1, DateCreated = DateTime.Now.AddMonths(-1),
                    Items = new List<LineItem>(){
                        new LineItem(){Product = GetProductRepository()[0], ProductId=1, Quantity=1 }
                    }
                }
            };
        }
        public List<Loyalty> GetLoyaltyRepository()
        {
            int goldAccountCredit = 1000;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoyaltyGoldAccountBalanceCredit"]))
                if (!int.TryParse(ConfigurationManager.AppSettings["LoyaltyGoldAccountBalanceCredit"], out goldAccountCredit))
                    goldAccountCredit = 1000;

            var loyaltyList = new List<Loyalty>()
            {
                new Loyalty(){ LoyaltyId =1, Name="Standard", DicountPercentage = null, CategoryThreshold=0, ExtraCredit=0},
                new Loyalty(){ LoyaltyId =2, Name="Silver", DicountPercentage = 0.02, CategoryThreshold=500, ExtraCredit=0},
                new Loyalty(){ LoyaltyId =3, Name="Gold", DicountPercentage = 0.05, CategoryThreshold=1500, ExtraCredit=goldAccountCredit}
            };

            return loyaltyList;
        }

        public List<Customer> GetCustomerRepository()
        {
            return customers;
        }

        public List<Product> GetProductRepository()
        {
            var products = new List<Product>()
            {
                new Product(){ProductId=1, Name="Product1", IsActive=true, Price=60.10 },
                new Product(){ProductId=2, Name="Product2", IsActive=true, Price=200},
                new Product(){ProductId=3, Name="Product3", IsActive=true, Price=99.99},
            };
            return products;
        }

        public List<ShoppingCart> GetShoppingCartRepository()
        {
            return shoppingCarts;
        }

        public void UpdateShoppingCartRepository(ShoppingCart shoppingCart)
        {
            var index = shoppingCarts.FindIndex(x => x.ShoppingCartId == shoppingCart.ShoppingCartId);
            if (index < 0) throw new KeyNotFoundException();
            shoppingCarts[index] = shoppingCart;
        }

        public List<Order> GetOrdersRepository()
        {
            return orders;
        }

        public void UpdateCustomer(Customer customer)
        {
            var index = customers.FindIndex(x => x.CustomerId == customer.CustomerId);
            if (index < 0) throw new KeyNotFoundException();
            customers[index] = customer;
        }

        public void SaveMessage(Message message)
        {
            messages.Add(message);
        }

        public ShoppingCart GetShoppingCart(int shoppingCartId)
        {
            var index = shoppingCarts.FindIndex(x => x.ShoppingCartId == shoppingCartId);
            if (index < 0) throw new KeyNotFoundException();
            return shoppingCarts[index];
        }

        public Customer GetCustomer(int id)
        {
            var index = customers.FindIndex(x => x.CustomerId == id);
            if (index < 0) throw new KeyNotFoundException();
            return customers[index];
        }
        public Product GetProduct(int productId)
        {
            var index = GetProductRepository().FindIndex(x => x.ProductId == productId);
            if (index < 0) throw new KeyNotFoundException();
            return GetProductRepository()[index];
        }
    }
}