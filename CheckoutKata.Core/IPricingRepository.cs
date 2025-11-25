namespace CheckoutKata.Core
{
    public interface IPricingRepository
    {
        void AddRule(PricingRule rule);
        bool IsValidSku(string sku);
        PricingRule GetRule(string sku);
    }
}
