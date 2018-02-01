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

        ShoppingCart GetShoppingCart(int shoppingCartId);
        Customer GetCustomer(int id);
        Product GetProduct(int productId);

        void UpdateShoppingCartRepository(ShoppingCart shoppingCart);
        void UpdateCustomer(Customer res);
        void SaveMessage(Message message);

    }
}