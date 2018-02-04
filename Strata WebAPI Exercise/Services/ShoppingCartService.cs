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
        /// Update the shopping cart with a product with positive or negative quantity.
        /// Having multiple execution paths breaks the Single Responsibility pattern but it is used for brevity
        /// </summary>
        /// <param name="shoppingCartId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public ShoppingCart UpdateProduct(int shoppingCartId, int productId, int quantity)
        {
            try
            {
                var shoppingCart = _repositoryService.GetShoppingCart(shoppingCartId);
                var product = _repositoryService.GetProduct(productId);

                //If product is already in the shopping cart
                if (shoppingCart.Items.Any(x => x.ProductId == product.ProductId))
                {
                    var item = shoppingCart.Items.Single(x => x.ProductId == product.ProductId);
                    item.Quantity += quantity;
                    //If the item quantity is 0 or negative we remove it from the shoppingCart
                    if (item.Quantity <= 0) shoppingCart.Items.Remove(item);
                }
                else
                {
                    if (quantity > 0)
                        shoppingCart.Items.Add(new LineItem() { ProductId = product.ProductId, Quantity = quantity });
                    else
                        throw new InvalidOperationException("Can't add negative quantity to a new line item.");
                }

                return shoppingCart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CanUserBuyShoppingCart(int shoppingCartId, int userId)
        {
            try
            {
                var shoppingCart = _repositoryService.GetShoppingCart(shoppingCartId);

                var customer = _customerService.GetCustomer(userId);

                if (customer.AccountBalance - shoppingCart.TotalCost > customer.LoyaltyNegativeBalance)
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public ShoppingCart BuyShoppingCart(int shoppingCartId, int customerId)
        {
            try
            {
                var shoppingCart = _repositoryService.GetShoppingCart(shoppingCartId);

                var customer = _customerService.GetCustomer(customerId);

                //double check that the user has enough balance
                if (customer.AccountBalance - shoppingCart.TotalCost > customer.LoyaltyNegativeBalance)
                {
                    var order = _orderService.CreateOrder(customerId, shoppingCart);
                    _customerService.UpdateBalance(customerId, order);
                    EmptyShoppingCartOnPurchase(ref shoppingCart);
                    _orderService.SendOrderMessage(order);

                    return shoppingCart;
                }
            }
            catch (Exception ex)
            {
                // The controller should get the error and return it
                throw ex;
            }
            return null;
        }

        private void EmptyShoppingCartOnPurchase(ref ShoppingCart shoppingCart)
        {
            shoppingCart.Items.Clear();
            _repositoryService.UpdateShoppingCartRepository(shoppingCart);
        }
    }
}