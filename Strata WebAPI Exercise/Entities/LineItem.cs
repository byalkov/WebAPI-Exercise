using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Entities
{
    public class LineItem
    {
        public int LineItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}