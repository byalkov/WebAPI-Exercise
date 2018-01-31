using System;

namespace Strata_WebAPI_Exercise.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public double AccountBalance { get; set; }

        public double LoyaltyNegativeBalance {
            get {
                return -Loyalty.CategoryThreshold;
            }
        }
        public int LoyaltyId { get; set; }        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }

        public virtual Loyalty Loyalty { get; set; }
    }
}