using System;
using System.Collections.Generic;
using System.Linq;

namespace Strata_WebAPI_Exercise.Entities
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int CustomerId { get; set; }
        // List of products and their quantity
        public List<LineItem> Items { get; set; }

        public virtual Customer Customer{ get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // Use the live values for price and loyalty discount
        public double TotalCost
        {
            get
            {
                return (Items.Sum(x => x.Product.Price * x.Quantity) * Customer.Loyalty.DicountPercentage);
            }
        }
    }
}