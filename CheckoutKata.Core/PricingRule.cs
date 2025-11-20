namespace CheckoutKata.Core
{
    public class PricingRule
    {
        public string Item;
        public int UnitPrice;
        public SpecialPrice[]? SpecialPrice;

        public PricingRule(string item, int unitPrice, SpecialPrice[]? specialPrice)
        {
            Item = item;
            UnitPrice = unitPrice;
            SpecialPrice = specialPrice;
        }
    }
}
