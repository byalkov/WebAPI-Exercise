using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Entities
{
    public class OrderLineItem : LineItem
    {
        // The price valid for this item at the time of purchase
        public double PurchasePrice { get; set; }
    }
}