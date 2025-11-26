using CheckoutKata.Core.Domain;

namespace CheckoutKata.Core.Interfaces
{
    public interface IPricingRepository
    {
        void AddRule(PricingRule rule);
        bool IsValidSku(string sku);
        PricingRule GetRule(string sku);
    }
}
