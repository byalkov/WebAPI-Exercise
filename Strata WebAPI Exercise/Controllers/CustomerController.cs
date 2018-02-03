using AutoMapper;
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
    [RoutePrefix("api/customer")]
    public class CustomerController : BaseAPIController
    {
        public CustomerController(ICustomerService customerService) : base(customerService)
        {
        }

        /// <summary>
        /// Get customer details (self)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCustomer()
        {
            // Get the userId from the claims token
            var userId = _customerService.GetClaimsUserId(ClaimsPrincipal.Current);

            if (!userId.HasValue)
            {
                return BadRequest();
            }
            try
            {
                var res = _customerService.GetCustomer(userId.Value);
                if (res == null) return NotFound();
                var dto = Mapper.Map<CustomerDTO>(res);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }
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
            try
            {
                var res = _customerService.GetCustomer(id);
                if (res == null) return NotFound();
                var dto = Mapper.Map<CustomerDTO>(res);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex.InnerException);
            }
        }
    }
}
