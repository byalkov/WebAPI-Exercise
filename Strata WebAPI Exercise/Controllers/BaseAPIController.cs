using Strata_WebAPI_Exercise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Strata_WebAPI_Exercise.Controllers
{
    public class BaseAPIController : ApiController
    {
        protected readonly ICustomerService _customerService;
        public BaseAPIController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
    }
}
