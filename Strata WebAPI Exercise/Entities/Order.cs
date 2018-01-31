using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public string DeliveryAddress { get; set; }
        public double DeliveryCost { get; set; }
        public double DiscountApplied { get; set; }
        public double TotalCost
        {
            get
            {
                return DeliveryCost + (OrderLines.Sum(x => x.PurchasePrice * x.Quantity) * DiscountApplied);
            }
        }
        public DateTime EstimatedDispatchDate
        {
            get
            {
                var daysExtra = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (DateCreated.AddDays(i).DayOfWeek == DayOfWeek.Saturday
                        || DateCreated.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                        daysExtra++;
                }
                return DateCreated.AddDays(3+daysExtra);
            }
        }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public List<OrderLineItem> OrderLines { get; set; }
        public virtual Customer Customer { get; set; }
    }
}