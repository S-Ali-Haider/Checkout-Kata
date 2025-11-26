using CheckoutKata.Core.Domain;

namespace CheckoutKata.Core.Interfaces
{
    public interface IPriceCalculator
    {
        int Calculate(PricingRule rule, int qty);
    }
}
