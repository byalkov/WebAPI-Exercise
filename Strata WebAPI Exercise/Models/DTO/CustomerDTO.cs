using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Models.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public double AccountBalance { get; set; }
        public string LoyaltyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }       
    }
}