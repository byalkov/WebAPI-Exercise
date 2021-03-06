﻿using AutoMapper;
using Strata_WebAPI_Exercise.Entities;
using Strata_WebAPI_Exercise.Interfaces;
using Strata_WebAPI_Exercise.Models.DTO;
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
    public class ShoppingCartController : BaseAPIController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, ICustomerService customerService) : base(customerService)
        {
            _shoppingCartService = shoppingCartService;
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
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }

            var res = _shoppingCartService.GetShoppingCart(userId.Value);
            if (res == null) return NotFound();

            var dto = Mapper.Map<ShoppingCartDTO>(res);
            return Ok(dto);
        }

        /// <summary>
        /// This endpoint allows for adding the items in the shoppingCart.
        /// Assuming customers are only allowed to update products to their own shopping cart.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add/{productId:int}/{quantity:int?}")]
        public IHttpActionResult AddProduct(int productId, int quantity = 1)
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }
            try
            {
                var shoppingCart = AddEditProduct(userId.Value, productId, quantity);
                var dto = Mapper.Map<ShoppingCartDTO>(shoppingCart);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                // return the error
                return InternalServerError(ex.InnerException);
            }
        }

        /// <summary>
        /// This endpoint allows for updating/removing the items in the shoppingCart withtout the need of separate endpoints.
        /// Assuming customers are only allowed to update products to their own shopping cart.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{productId:int}/{quantity:int}")]
        public IHttpActionResult EditProductQuantity(int productId, int quantity)
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }
            try
            {
                var shoppingCart = AddEditProduct(userId.Value, productId, quantity);
                var dto = Mapper.Map<ShoppingCartDTO>(shoppingCart);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                // return the error
                return InternalServerError(ex.InnerException);
            }
        }


        [HttpPost]
        [Route("buy")]
        public IHttpActionResult BuyShoppingCart()
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }
            try
            {
                var shoppingCart = _shoppingCartService.GetShoppingCart(userId.Value);
                // If the customer doesn't have sufficient balance we can't process purchase
                if (!_shoppingCartService.CanUserBuyShoppingCart(shoppingCart.ShoppingCartId, userId.Value))
                    return BadRequest("Insufficient account balance, can't proceed with purchase.");

                shoppingCart = _shoppingCartService.BuyShoppingCart(shoppingCart.ShoppingCartId, userId.Value);
                var dto = Mapper.Map<ShoppingCartDTO>(shoppingCart);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                // return the error
                return InternalServerError(ex.InnerException);
            }
        }


        private ShoppingCart AddEditProduct(int userId, int productId, int quantity)
        {
            try
            {
                var shoppingCart = _shoppingCartService.GetShoppingCart(userId);
                shoppingCart = _shoppingCartService.UpdateProduct(shoppingCart.ShoppingCartId, productId, quantity);
                return shoppingCart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
