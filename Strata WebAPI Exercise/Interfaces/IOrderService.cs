using Strata_WebAPI_Exercise.Entities;
using System;
using System.Collections.Generic;

namespace Strata_WebAPI_Exercise.Interfaces
{
    public interface IOrderService
    {
        Order GetOrder(int customerId, int orderId);
        List<Order> GetOrders(int customerId, DateTime startDate, DateTime endDate);

        List<Order> GetOrders(int customerId, Status orderStatus);

        List<Order> GetOrders(int customerId);
        Order CreateOrder(int customerId, ShoppingCart shoppingCart);
        void SendOrderMessage(Order order);
    }
}
