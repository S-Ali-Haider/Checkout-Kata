namespace CheckoutKata.Core
{
    public interface IPricingRules
    {
        public int GetPrice(string item);
        public bool IsValidItem(string item);
        public void AddRule(string item, int unitPrice, SpecialPrice[]? specialPrices);
        public SpecialPrice[]? GetOffers(string item);
    }
}
