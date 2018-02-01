using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Strata_WebAPI_Exercise.Models.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string DeliveryAddress { get; set; }
        public double DeliveryCost { get; set; }
        public double TotalCost { get; set; }
        public DateTime EstimatedDispatchDate { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public List<LineItemDTO> OrderLineItems { get; set; }
    }
}