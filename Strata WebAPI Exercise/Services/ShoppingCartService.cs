using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Linq;

namespace Strata_WebAPI_Exercise.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        public ShoppingCartService(IRepositoryService repositoryService, ICustomerService customerService, IOrderService orderService) : base(repositoryService)
        {
            _customerService = customerService;
            _orderService = orderService;
        }
        /// <summary>
        /// Get the shopping cart for the customerId.
        /// Assume only one or none shopping cart exists per customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ShoppingCart GetShoppingCart(int customerId)
        {
            var res = _repositoryService.GetShoppingCartRepository().FirstOrDefault(x => x.CustomerId == customerId);
            return res;
        }

        /// <summary>
        /// Update the shopping cart with a product
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public ShoppingCart UpdateProduct(int shoppingCartId, int productId, int quantity)
        {
            var shoppingCart = _repositoryService.GetShoppingCartRepository().FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
            if (shoppingCart == null) return null;

            var product = _repositoryService.GetProductRepository().FirstOrDefault(x => x.ProductId == productId);
            if (product == null) return null;

            //If product is already in the shopping cart
            if (shoppingCart.Items.Any(x => x.ProductId == product.ProductId))
            {
                var item = shoppingCart.Items.Single(x => x.ProductId == product.ProductId);
                item.Quantity += quantity;
                //If the item quantity is 0 or negative we remove it from the shoppingCart
                if (item.Quantity <= 0) shoppingCart.Items.Remove(item);
            }
            else
                shoppingCart.Items.Add(new LineItem() { ProductId = product.ProductId, Quantity = quantity });

            return shoppingCart;
        }

        public bool CanUserBuyShoppingCart(int shoppingCartId, int userId)
        {
            var shoppingCart = _repositoryService.GetShoppingCartRepository().FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
            if (shoppingCart == null) return false;

            var customer = _customerService.GetCustomer(userId);

            if (customer.AccountBalance - shoppingCart.TotalCost > customer.LoyaltyNegativeBalance)
                return true;

            return false;
        }

        public ShoppingCart BuyShoppingCart(int shoppingCartId, int customerId)
        {
            var shoppingCart = _repositoryService.GetShoppingCartRepository().FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
            if (shoppingCart == null) return null;

            var customer = _customerService.GetCustomer(customerId);

            if (customer.AccountBalance - shoppingCart.TotalCost > customer.LoyaltyNegativeBalance)
            {
                var order = _orderService.CreateOrder(customerId, shoppingCart);
                _customerService.UpdateBalance(customerId, customer.AccountBalance - shoppingCart.TotalCost);
                EmptyShoppingCartOnPurchase(shoppingCart);
                _orderService.SendOrderMessage(order);
            }
            return null;
        }

        private void EmptyShoppingCartOnPurchase(ShoppingCart shoppingCart)
        {
            shoppingCart.Items.Clear();
            _repositoryService.UpdateShoppingCartRepository(shoppingCart);
        }
    }
}