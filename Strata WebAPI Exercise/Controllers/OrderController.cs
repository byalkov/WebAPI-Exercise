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
    [RoutePrefix("api/order")]
    public class OrderController : BaseAPIController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, ICustomerService customerService):base(customerService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("{orderId:int}")]
        public IHttpActionResult GetOrder(int orderId)
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }

            var res = _orderService.GetOrder(userId.Value, orderId);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet]
        [Route("{startDate:DateTime}-{endDate:DateTime}")]
        public IHttpActionResult GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }

            var res = _orderService.GetOrders(userId.Value, startDate, endDate);
            if (res == null) return NotFound();
            return Ok(res);
        }

        [HttpGet]
        [Route("{awaiting}")]
        public IHttpActionResult GetOrdersAwaitingDispatch()
        {
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest("Can't identify client, can't proceed with purchase.");
            }

            var res = _orderService.GetOrders(userId.Value, Status.AwaitingDespatch);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}
