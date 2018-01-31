using Strata_WebAPI_Exercise.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Interfaces
{
    public interface IRepositoryService
    {
        List<Loyalty> GetLoyaltyRepository();
        List<Customer> GetCustomerRepository();
        List<Product> GetProductRepository();
        List<ShoppingCart> GetShoppingCartRepository();
        List<Order> GetOrdersRepository();

        void UpdateShoppingCartRepository(ShoppingCart shoppingCart);
        void UpdateCustomerRepository(Customer res);
    }
}