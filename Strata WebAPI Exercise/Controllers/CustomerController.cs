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
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerService customerService;
        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        /// <summary>
        /// Get customer details (self)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCustomer()
        {
            // Get the userId from the claims token
            var userId = customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest();
            }

            var res = customerService.GetCustomer(userId.Value);
            if (res == null) return NotFound();
            return Ok(res);
        }

        /// <summary>
        /// Get specific customer details. Assuming users can look up other users.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetCustomer(int id)
        {
            var res = customerService.GetCustomer(id);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}
