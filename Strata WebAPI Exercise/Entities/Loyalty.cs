namespace Strata_WebAPI_Exercise.Entities
{
    public class Loyalty
    {
        public int LoyaltyId { get; set; }

        public string Name { get; set; }

        public double DicountPercentage { get; set; }
        // Amount spent in the last 12 months 
        public double CategoryThreshold { get; set; }

        // Category specific extra credit that will allow negative balance
        public double ExtraCredit { get; set; }
    }
}