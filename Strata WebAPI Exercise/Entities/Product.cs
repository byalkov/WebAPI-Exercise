using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        // Assume Name of product is not changed once set 
        public string Name { get; set; }
        // Assume Price of a product may change in time 
        public double Price { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        // Need to keep a list of all products even if they are no longer active
        public bool IsActive { get; set; }
    }
}