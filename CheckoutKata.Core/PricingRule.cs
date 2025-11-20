namespace CheckoutKata.Core
{
    public class PricingRule
    {
        public string Item { get; }
        public int UnitPrice { get; set; }
        public SpecialPrice[]? SpecialPrice { get; set; }

        public PricingRule(string item, int unitPrice, SpecialPrice[]? specialPrice)
        {
            Item = item;
            UnitPrice = unitPrice;
            SpecialPrice = specialPrice;
        }
    }
}
