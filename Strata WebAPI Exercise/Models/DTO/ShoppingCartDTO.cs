using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Models.DTO
{
    public class ShoppingCartDTO
    {
        public int ShoppingCartId { get; set; }
        public int CustomerId { get; set; }
        public List<LineItemDTO> Items { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public double TotalCost { get; set; }
    }
}