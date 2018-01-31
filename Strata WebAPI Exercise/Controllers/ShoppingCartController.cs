using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Strata_WebAPI_Exercise.Controllers
{
    [Authorize]
    [RoutePrefix("api/shoppingCart")]
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICustomerService _customerService;
        public ShoppingCartController(IShoppingCartService shoppingCartService, ICustomerService customerService)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
        }

        /// <summary>
        /// Get the shopping cart for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetShoppingCart()
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var res = _shoppingCartService.GetShoppingCart(userId.Value);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        /// Assuming customers are only allowed to update products to their own shopping cart.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("update/{productId:int}/{quantity:int?}")]
        public IHttpActionResult UpdateProduct(int productId, int quantity = 1)
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var shoppingCart = _shoppingCartService.GetShoppingCart(userId.Value);

            // If we can't find the shopping basket the request is not valid
            if (shoppingCart == null) return BadRequest();

            shoppingCart = _shoppingCartService.UpdateProduct(shoppingCart.ShoppingCartId, productId, quantity);

            //if the service returns null either the shopping cart or product were not found
            if (shoppingCart == null) return NotFound();

            return Ok(shoppingCart);
        }

        [HttpPost]
        [Route("buy")]
        public IHttpActionResult BuyShoppingCart()
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var shoppingCart = _shoppingCartService.GetShoppingCart(userId.Value);

            // If we can't find the shopping basket the request is not valid
            if (shoppingCart == null) return BadRequest();
            
            // If the customer doesn't have sufficient balance we can't process purchase
            if (!_shoppingCartService.CanUserBuyShoppingCart(shoppingCart.ShoppingCartId, userId.Value))
                return BadRequest();

            shoppingCart = _shoppingCartService.BuyShoppingCart(shoppingCart.ShoppingCartId, userId.Value);

            // Something went wrong
            if (shoppingCart == null) return InternalServerError();

            return Ok(shoppingCart);
        }
    }
}
