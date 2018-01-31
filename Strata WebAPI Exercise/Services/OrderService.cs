using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Strata_WebAPI_Exercise.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly ICustomerService _customerService;
        public OrderService(IRepositoryService repositoryService, ICustomerService customerService) : base(repositoryService)
        {
            _customerService = customerService;
        }

        public Order CreateOrder(int customerId, ShoppingCart shoppingCart)
        {
            var customer = _customerService.GetCustomer(customerId);
            var order = new Order()
            {
            
            };

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get single order for customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(int customerId, int orderId)
        {
            var res = _repositoryService.GetOrdersRepository().FirstOrDefault(x => x.OrderId == orderId && x.CustomerId == customerId);
            return res;
        }

        /// <summary>
        /// Get all customer orders filtered by time period
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Order> GetOrders(int customerId, DateTime startDate, DateTime endDate)
        {
            var res = _repositoryService.GetOrdersRepository().Where(x => x.CustomerId == customerId && x.DateCreated >= startDate && x.DateCreated <= endDate);
            return res.ToList();
        }

        /// <summary>
        /// Get all customer orders filtered by status
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public List<Order> GetOrders(int customerId, Status orderStatus)
        {
            var res = _repositoryService.GetOrdersRepository().Where(x => x.CustomerId == customerId && x.Status == orderStatus);
            return res.ToList();
        }

        /// <summary>
        /// Get all orders for a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<Order> GetOrders(int customerId)
        {
            var res = _repositoryService.GetOrdersRepository().Where(x => x.CustomerId == customerId);
            return res.ToList();
        }

        /// <summary>
        /// Generate message from Order
        /// Assume that the message needs to be stored in the db and it will be sent by a downstream service (out of scope) 
        /// </summary>
        /// <param name="order"></param>
        public void SendOrderMessage(Order order)
        {
            var estimatedDispatchDate = order.EstimatedDispatchDate.ToString("dd/MM/yyyy");
            var messageBody = "Order #{order.OrderId} - Estimated despatch {estimatedDispatchDate}\n";
            foreach (var item in order.OrderLineItems)
            {
                messageBody += item.Product.Name + " x" + item.Quantity + " = £" + item.Product.Price + "\n";
            }
            messageBody += "Total " + 
                (order.Customer.Loyalty.DicountPercentage.HasValue?"(includes "+ order.Customer.Loyalty.DicountPercentage.Value+"% "+ order.Customer.Loyalty.Name + " discount) = £"
                : string.Empty) + order.TotalCost;

            var message = new Message() {
                OrderId = order.OrderId,
                MessageBody = messageBody,
                DateCreated = DateTime.Now
            };

            _repositoryService.SaveMessage(message);
        }
    }
}